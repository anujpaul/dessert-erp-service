using DessertERP.Domain.Modules.AccountsPayable;
using DessertERP.Domain.Modules.GeneralLedger;
using DessertERP.Domain.Modules.Organization;
using DessertERP.Domain.Modules.ProductManagement;
using DessertERP.Domain.Modules.SystemAdmin;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
// AR namespace excluded — seeder only uses PM.Product; AR.Product is an obsolete tombstone
using Customer  = DessertERP.Domain.Modules.AccountsReceivable.Customer;
using Vendor    = DessertERP.Domain.Modules.AccountsPayable.Vendor;

namespace DessertERP.Infrastructure.Persistence.Seed;

public static class DatabaseSeeder
{
    public static async Task SeedAsync(AppDbContext db, ILogger logger)
    {
        await SeedAccountTypesAsync(db, logger);
        var orgId = await SeedDefaultOrganizationAsync(db, logger);
        await SeedChartOfAccountsAsync(db, logger, orgId);
        await SeedFiscalYearAsync(db, logger, orgId);
        await SeedCurrenciesAsync(db, logger, orgId);
        await SeedCatalogAsync(db, logger, orgId);   // Categories, Brands, Products, Variants, Inventory
        await SeedCustomersAsync(db, logger, orgId);
        await SeedVendorsAsync(db, logger, orgId);
        await SeedSystemAdminAsync(db, logger, orgId);
        await RemoveLegacySystemAdminRoleAsync(db, logger);
    }

    // ── Default Organization ───────────────────────────────────────────────────

    private static async Task<Guid> SeedDefaultOrganizationAsync(AppDbContext db, ILogger logger)
    {
        var existing = await db.Organizations.IgnoreQueryFilters()
            .FirstOrDefaultAsync(o => o.Code == "DEFAULT");
        if (existing is not null) return existing.Id;

        // Also accept the old DEMO01 code so existing databases are not duplicated
        var legacy = await db.Organizations.IgnoreQueryFilters()
            .FirstOrDefaultAsync(o => o.Code == "DEMO01");
        if (legacy is not null) return legacy.Id;

        logger.LogInformation("Seeding default organization...");
        var org = Organization.Create(
            "DEFAULT",
            "Default Organization",
            "USD",
            fiscalYearStartMonth: 1,
            address:  "1 Main Street, New York, NY 10001",
            phone:    "+1-800-555-0100",
            email:    "admin@dessert-erp.com",
            taxId:    "US-00-0000000");
        db.Organizations.Add(org);
        await db.SaveChangesAsync();
        return org.Id;
    }

    // ── Account Types ─────────────────────────────────────────────────────────

    private static async Task SeedAccountTypesAsync(AppDbContext db, ILogger logger)
    {
        if (await db.AccountTypes.AnyAsync()) return;
        logger.LogInformation("Seeding account types...");
        db.AccountTypes.AddRange(
            new AccountType("ASSET",     "Asset",              AccountNature.Debit,  1),
            new AccountType("LIABILITY", "Liability",          AccountNature.Credit, 2),
            new AccountType("EQUITY",    "Equity",             AccountNature.Credit, 3),
            new AccountType("REVENUE",   "Revenue",            AccountNature.Credit, 4),
            new AccountType("EXPENSE",   "Expense",            AccountNature.Debit,  5),
            new AccountType("COGS",      "Cost of Goods Sold", AccountNature.Debit,  6)
        );
        await db.SaveChangesAsync();
    }

    // ── Currencies ────────────────────────────────────────────────────────────

    private static async Task SeedCurrenciesAsync(AppDbContext db, ILogger logger, Guid orgId)
    {
        if (await db.Currencies.IgnoreQueryFilters().AnyAsync(c => c.OrganizationId == orgId)) return;
        logger.LogInformation("Seeding default currencies (ISO 4217)...");

        // Helper: build a Currency via reflection-friendly construction
        // The domain constructor: Currency(orgId, code, name, symbol, decimalPlaces, exchangeRate, isBase, numericCode, country)
        // We use the static factory or public ctor — check what the domain exposes.
        // Based on summary: Currency has properties set via domain methods; assume a public ctor.
        // isBase=true means this is the functional currency; ExchangeRate=1 for base.
        // Rates below are approximate mid-market rates vs USD (as of seed time — update via API later).
        var usd = new Currency(orgId, "USD", "US Dollar",         "$",   2, 1.00m,   isBase: true,  numericCode: 840, country: "United States");
        var eur = new Currency(orgId, "EUR", "Euro",              "€",   2, 1.08m,   isBase: false, numericCode: 978, country: "European Union");
        var gbp = new Currency(orgId, "GBP", "British Pound",    "£",   2, 1.27m,   isBase: false, numericCode: 826, country: "United Kingdom");
        var cad = new Currency(orgId, "CAD", "Canadian Dollar",  "C$",  2, 0.74m,   isBase: false, numericCode: 124, country: "Canada");
        var aud = new Currency(orgId, "AUD", "Australian Dollar","A$",  2, 0.65m,   isBase: false, numericCode:  36, country: "Australia");
        var jpy = new Currency(orgId, "JPY", "Japanese Yen",     "¥",   0, 0.0067m, isBase: false, numericCode: 392, country: "Japan");
        var chf = new Currency(orgId, "CHF", "Swiss Franc",      "Fr",  2, 1.12m,   isBase: false, numericCode: 756, country: "Switzerland");
        var cny = new Currency(orgId, "CNY", "Chinese Yuan",     "¥",   2, 0.138m,  isBase: false, numericCode: 156, country: "China");
        var aed = new Currency(orgId, "AED", "UAE Dirham",       "د.إ", 2, 0.272m,  isBase: false, numericCode: 784, country: "United Arab Emirates");
        var sar = new Currency(orgId, "SAR", "Saudi Riyal",      "﷼",   2, 0.267m,  isBase: false, numericCode: 682, country: "Saudi Arabia");

        db.Currencies.AddRange(usd, eur, gbp, cad, aud, jpy, chf, cny, aed, sar);
        await db.SaveChangesAsync();
        logger.LogInformation("Seeded 10 currencies. Base: USD.");
    }

    // ── Chart of Accounts ─────────────────────────────────────────────────────

    private static async Task SeedChartOfAccountsAsync(AppDbContext db, ILogger logger, Guid orgId)
    {
        if (await db.Accounts.IgnoreQueryFilters().AnyAsync(a => a.OrganizationId == orgId)) return;
        logger.LogInformation("Seeding chart of accounts...");

        var asset     = await db.AccountTypes.FirstAsync(t => t.Code == "ASSET");
        var liability = await db.AccountTypes.FirstAsync(t => t.Code == "LIABILITY");
        var equity    = await db.AccountTypes.FirstAsync(t => t.Code == "EQUITY");
        var revenue   = await db.AccountTypes.FirstAsync(t => t.Code == "REVENUE");
        var expense   = await db.AccountTypes.FirstAsync(t => t.Code == "EXPENSE");
        var cogs      = await db.AccountTypes.FirstAsync(t => t.Code == "COGS");

        db.Accounts.AddRange(
            new Account(orgId, "1000", "Assets",                    asset.Id,     true),
            new Account(orgId, "1100", "Cash & Bank",               asset.Id,     true),
            new Account(orgId, "1110", "Cash on Hand",              asset.Id,     false),
            new Account(orgId, "1120", "Bank Account - Operating",  asset.Id,     false),
            new Account(orgId, "1200", "Accounts Receivable",       asset.Id,     true),
            new Account(orgId, "1210", "Trade Receivables",         asset.Id,     false),
            new Account(orgId, "1300", "Inventory",                 asset.Id,     true),
            new Account(orgId, "1310", "Finished Goods",            asset.Id,     false),
            new Account(orgId, "1500", "Other Current Assets",      asset.Id,     true),
            new Account(orgId, "1510", "Prepaid Expenses",          asset.Id,     false),
            new Account(orgId, "2000", "Liabilities",               liability.Id, true),
            new Account(orgId, "2100", "Accounts Payable",          liability.Id, true),
            new Account(orgId, "2110", "Trade Payables",            liability.Id, false),
            new Account(orgId, "2200", "Tax Liabilities",           liability.Id, true),
            new Account(orgId, "2210", "Sales Tax Payable",         liability.Id, false),
            new Account(orgId, "2300", "Other Current Liabilities", liability.Id, true),
            new Account(orgId, "3000", "Equity",                    equity.Id,    true),
            new Account(orgId, "3100", "Owner's Equity",            equity.Id,    false),
            new Account(orgId, "3200", "Retained Earnings",         equity.Id,    false),
            new Account(orgId, "4000", "Revenue",                   revenue.Id,   true),
            new Account(orgId, "4100", "Sales Revenue",             revenue.Id,   false),
            new Account(orgId, "4200", "Service Revenue",           revenue.Id,   false),
            new Account(orgId, "4900", "Other Revenue",             revenue.Id,   false),
            new Account(orgId, "5000", "Cost of Goods Sold",        cogs.Id,      true),
            new Account(orgId, "5100", "COGS - Products",           cogs.Id,      false),
            new Account(orgId, "6000", "Operating Expenses",        expense.Id,   true),
            new Account(orgId, "6100", "Salaries & Wages",          expense.Id,   false),
            new Account(orgId, "6200", "Rent & Utilities",          expense.Id,   false),
            new Account(orgId, "6300", "Marketing & Advertising",   expense.Id,   false),
            new Account(orgId, "6400", "General & Administrative",  expense.Id,   false),
            new Account(orgId, "6900", "Other Expenses",            expense.Id,   false)
        );
        await db.SaveChangesAsync();
    }

    // ── Fiscal Years ──────────────────────────────────────────────────────────

    private static async Task SeedFiscalYearAsync(AppDbContext db, ILogger logger, Guid orgId)
    {
        if (await db.FiscalYears.IgnoreQueryFilters().AnyAsync(f => f.OrganizationId == orgId)) return;
        logger.LogInformation("Seeding fiscal years...");

        var calendar = new FiscalCalendar(
            orgId, "Corporate Calendar", "Primary monthly posting calendar",
            FiscalCalendarTypes.Monthly, true);
        db.FiscalCalendars.Add(calendar);

        var fy2025 = new FiscalYear(orgId, calendar.Id, "FY2025", "Fiscal Year 2025",
            new DateTime(2025, 1, 1), new DateTime(2025, 12, 31), calendar.CalendarType);
        fy2025.GenerateMonthlyPeriods();
        db.FiscalYears.Add(fy2025);

        var fy2026 = new FiscalYear(orgId, calendar.Id, "FY2026", "Fiscal Year 2026",
            new DateTime(2026, 1, 1), new DateTime(2026, 12, 31), calendar.CalendarType);
        fy2026.GenerateMonthlyPeriods();
        db.FiscalYears.Add(fy2026);

        await db.SaveChangesAsync();
    }

    // ── Full Product Catalog ───────────────────────────────────────────────────

    private static async Task SeedCatalogAsync(AppDbContext db, ILogger logger, Guid orgId)
    {
        // Only skip if variants already exist — products without variants means a partial seed
        if (await db.ProductVariants.IgnoreQueryFilters().AnyAsync(v => v.OrganizationId == orgId)) return;
        logger.LogInformation("Seeding product catalog (Duluth-style) — variants missing, re-seeding...");

        // ── Helper: get-or-create category by code ───────────────────────────
        async Task<Category> GetOrCreateCategory(string code, string name, Guid? parentId, string? imageUrl, int order)
        {
            var existing = await db.Categories.IgnoreQueryFilters()
                .FirstOrDefaultAsync(c => c.Code == code && c.OrganizationId == orgId);
            if (existing != null) return existing;
            var cat = new Category(orgId, code, name, parentId, imageUrl, order);
            db.Categories.Add(cat);
            await db.SaveChangesAsync();
            return cat;
        }

        // ── Helper: get-or-create brand by code ─────────────────────────────
        async Task<Brand> GetOrCreateBrand(string code, string name, string? desc, string? country, string? website)
        {
            var existing = await db.Brands.IgnoreQueryFilters()
                .FirstOrDefaultAsync(b => b.Code == code && b.OrganizationId == orgId);
            if (existing != null) return existing;
            var brand = new Brand(orgId, code, name, desc, country, website);
            db.Brands.Add(brand);
            await db.SaveChangesAsync();
            return brand;
        }

        // ── Categories — Full Retail Hierarchy (GS1-inspired, A–L top-level) ─
        var catClothing   = await GetOrCreateCategory("A",    "Clothing & Apparel",     null,           null, 1);
        var catFootwear   = await GetOrCreateCategory("B",    "Footwear",               null,           null, 2);
        var catUnderwearT = await GetOrCreateCategory("C",    "Underwear & Intimates",  null,           null, 3);
        var catSports     = await GetOrCreateCategory("D",    "Sports & Outdoors",      null,           null, 4);
        var catToys       = await GetOrCreateCategory("E",    "Toys & Games",           null,           null, 5);
        var catAccessory  = await GetOrCreateCategory("F",    "Accessories & Bags",     null,           null, 6);
        var catFood       = await GetOrCreateCategory("G",    "Food & Beverage",        null,           null, 7);
        var catBeauty     = await GetOrCreateCategory("H",    "Beauty & Personal Care", null,           null, 8);
        var catHome       = await GetOrCreateCategory("I",    "Home & Living",          null,           null, 9);
        var catElectronics= await GetOrCreateCategory("J",    "Electronics",            null,           null, 10);
        var catHealth     = await GetOrCreateCategory("K",    "Health & Wellness",      null,           null, 11);
        var catPets       = await GetOrCreateCategory("L",    "Pet Supplies",           null,           null, 12);

        // A — Clothing subcategories
        var catShirts     = await GetOrCreateCategory("A-01", "Shirts & Tops",          catClothing.Id, null, 1);
        var catPants      = await GetOrCreateCategory("A-02", "Pants & Trousers",       catClothing.Id, null, 2);
        var catShorts     = await GetOrCreateCategory("A-03", "Shorts",                 catClothing.Id, null, 3);
        var catJackets    = await GetOrCreateCategory("A-04", "Jackets & Outerwear",    catClothing.Id, null, 4);
        var catDresses    = await GetOrCreateCategory("A-05", "Dresses & Skirts",       catClothing.Id, null, 5);
        var catSweaters   = await GetOrCreateCategory("A-06", "Sweaters & Hoodies",     catClothing.Id, null, 6);
        var catSuits      = await GetOrCreateCategory("A-07", "Suits & Formal",         catClothing.Id, null, 7);
        var catSleepwear  = await GetOrCreateCategory("A-08", "Sleepwear",              catClothing.Id, null, 8);
        var catSwimwear   = await GetOrCreateCategory("A-09", "Swimwear",               catClothing.Id, null, 9);
        var catWorkwear   = await GetOrCreateCategory("A-10", "Workwear & Uniforms",    catClothing.Id, null, 10);

        // B — Footwear subcategories
        var catSneakers   = await GetOrCreateCategory("B-01", "Sneakers & Athletic",    catFootwear.Id, null, 1);
        var catBoots      = await GetOrCreateCategory("B-02", "Boots",                  catFootwear.Id, null, 2);
        var catSandals    = await GetOrCreateCategory("B-03", "Sandals & Flip Flops",   catFootwear.Id, null, 3);
        var catDressShoes = await GetOrCreateCategory("B-04", "Dress Shoes",            catFootwear.Id, null, 4);
        var catSlippers   = await GetOrCreateCategory("B-05", "Slippers & Casual",      catFootwear.Id, null, 5);
        var catHikingBoots= await GetOrCreateCategory("B-06", "Hiking & Trail",         catFootwear.Id, null, 6);

        // C — Underwear subcategories
        var catMensUnderwear   = await GetOrCreateCategory("C-01", "Men's Underwear",       catUnderwearT.Id, null, 1);
        var catWomensUnderwear = await GetOrCreateCategory("C-02", "Women's Underwear",     catUnderwearT.Id, null, 2);
        var catBras            = await GetOrCreateCategory("C-03", "Bras & Bralettes",      catUnderwearT.Id, null, 3);
        var catSocks           = await GetOrCreateCategory("C-04", "Socks & Hosiery",       catUnderwearT.Id, null, 4);
        var catBaseLayer       = await GetOrCreateCategory("C-05", "Base Layer & Thermals", catUnderwearT.Id, null, 5);

        // D — Sports subcategories
        var catGym        = await GetOrCreateCategory("D-01", "Gym & Fitness",          catSports.Id,   null, 1);
        var catRunning    = await GetOrCreateCategory("D-02", "Running & Jogging",      catSports.Id,   null, 2);
        var catHiking     = await GetOrCreateCategory("D-03", "Hiking & Camping",       catSports.Id,   null, 3);
        var catTeamSports = await GetOrCreateCategory("D-04", "Team Sports",            catSports.Id,   null, 4);
        var catYoga       = await GetOrCreateCategory("D-05", "Yoga & Pilates",         catSports.Id,   null, 5);

        // E — Toys subcategories
        var catToysInfant = await GetOrCreateCategory("E-01", "Infant & Toddler",       catToys.Id,     null, 1);
        var catToysPuzzle = await GetOrCreateCategory("E-02", "Puzzles & Board Games",  catToys.Id,     null, 2);
        var catToysAction = await GetOrCreateCategory("E-03", "Action Figures",         catToys.Id,     null, 3);
        var catToysDolls  = await GetOrCreateCategory("E-04", "Dolls & Plush",          catToys.Id,     null, 4);
        var catToysSTEM   = await GetOrCreateCategory("E-05", "STEM & Educational",     catToys.Id,     null, 5);

        // F — Accessories subcategories
        var catBags       = await GetOrCreateCategory("F-01", "Bags & Backpacks",       catAccessory.Id,null, 1);
        var catWallets    = await GetOrCreateCategory("F-02", "Wallets & Cardholders",  catAccessory.Id,null, 2);
        var catHats       = await GetOrCreateCategory("F-03", "Hats & Caps",            catAccessory.Id,null, 3);
        var catBelts      = await GetOrCreateCategory("F-04", "Belts & Suspenders",     catAccessory.Id,null, 4);
        var catSunglasses = await GetOrCreateCategory("F-05", "Sunglasses & Eyewear",   catAccessory.Id,null, 5);
        var catJewelry    = await GetOrCreateCategory("F-06", "Jewelry & Watches",      catAccessory.Id,null, 6);

        // G — Food subcategories
        var catCandy      = await GetOrCreateCategory("G-01", "Candy & Chocolate",      catFood.Id,     null, 1);
        var catSnacks     = await GetOrCreateCategory("G-02", "Snacks & Chips",         catFood.Id,     null, 2);
        var catBeverages  = await GetOrCreateCategory("G-03", "Beverages",              catFood.Id,     null, 3);
        var catBaked      = await GetOrCreateCategory("G-04", "Baked Goods",            catFood.Id,     null, 4);

        // H — Beauty subcategories
        var catCream      = await GetOrCreateCategory("H-01", "Creams & Moisturizers",  catBeauty.Id,   null, 1);
        var catHairCare   = await GetOrCreateCategory("H-02", "Hair Care",              catBeauty.Id,   null, 2);
        var catMakeup     = await GetOrCreateCategory("H-03", "Makeup & Cosmetics",     catBeauty.Id,   null, 3);
        var catSkinCare   = await GetOrCreateCategory("H-04", "Skin Care",              catBeauty.Id,   null, 4);
        var catOralCare   = await GetOrCreateCategory("H-05", "Oral Care",              catBeauty.Id,   null, 5);

        // Backward-compat aliases for existing seeded products
        var catMensClothing   = catShirts;
        var catWomensClothing = catShirts;
        var catUnderwear      = catMensUnderwear;
        var catMensFootwear   = catHikingBoots;
        var catWomensFootwear = catSneakers;
        var catCare           = catBeauty;

        // ── Brands ──────────────────────────────────────────────────────────
        var brandDuluth      = await GetOrCreateBrand("DULUTH",  "Duluth Trading Co.",    "House brand — built for hard work",          "USA", "https://duluthtrading.com");
        var brandDarnTough   = await GetOrCreateBrand("DARN",    "Darn Tough Vermont",    "Made in Vermont, guaranteed for life",        "USA", "https://darntough.com");
        var brandFarmFeet    = await GetOrCreateBrand("FARM2FT", "Farm to Feet",          "American-made merino wool socks",             "USA", "https://farmtofeet.com");
        var brandGhirardelli = await GetOrCreateBrand("GHIR",    "Ghirardelli",           "Premium chocolate since 1852",                "USA", "https://ghirardelli.com");
        var brandBalega      = await GetOrCreateBrand("BALEGA",  "Balega",                "Performance running & outdoor socks",         "USA", "https://balega.com");

        // ── Helper to add product with variants + inventory (idempotent) ────
        async Task AddProduct(
            Product productTemplate,
            IEnumerable<(string Size, string? Color, string? Material, string? Barcode, decimal Stock)> variants)
        {
            // Reuse existing product if SKU already exists
            var product = await db.CatalogProducts.IgnoreQueryFilters()
                .FirstOrDefaultAsync(p => p.Sku == productTemplate.Sku && p.OrganizationId == orgId)
                ?? productTemplate;

            if (product == productTemplate)
            {
                db.CatalogProducts.Add(product);
                await db.SaveChangesAsync();
            }

            // Only add variants that don't exist yet (match by SKU prefix + size)
            var variantStocks = new List<(ProductVariant Variant, decimal Stock)>();
            foreach (var (size, color, material, barcode, stock) in variants)
            {
                var variant = product.AddVariant(size, color, material, barcode);
                var exists = await db.ProductVariants.IgnoreQueryFilters()
                    .AnyAsync(v => v.Sku == variant.Sku && v.OrganizationId == orgId);
                if (!exists)
                {
                    db.ProductVariants.Add(variant);
                    variantStocks.Add((variant, stock));
                }
            }

            if (variantStocks.Count > 0)
            {
                await db.SaveChangesAsync(); // persist new variants

                foreach (var (variant, stock) in variantStocks)
                {
                    var inv = new InventoryRecord(orgId, variant.Id,
                        quantityOnHand: stock, reorderPoint: 5, minimumStock: 0, maximumStock: 150);
                    db.InventoryRecords.Add(inv);
                }
                await db.SaveChangesAsync();
            }
        }

        // ── Men's Clothing ───────────────────────────────────────────────────

        await AddProduct(
            new Product(orgId, "DTC-PANT-001", "Fire Hose Work Pants",
                catMensClothing.Id, ProductType.Clothing, 84.50m, 32.00m, 
                "Each", brandDuluth.Id, GenderTarget.Men,
                "Tough as iron, comfortable all day. Built from our exclusive Fire Hose canvas.",
                "workwear,pants,tough"),
            new[]
            {
                ("30x30", "Dark Brown", "Canvas", "884592001010", 18m),
                ("30x30", "Black",      "Canvas", "884592001027", 22m),
                ("32x30", "Dark Brown", "Canvas", "884592001034", 25m),
                ("32x30", "Black",      "Canvas", "884592001041", 30m),
                ("34x30", "Dark Brown", "Canvas", "884592001058", 20m),
                ("34x32", "Black",      "Canvas", "884592001065", 15m),
                ("36x32", "Dark Brown", "Canvas", "884592001072", 12m),
                ("38x32", "Black",      "Canvas", "884592001089", 10m),
            }
        );

        await AddProduct(
            new Product(orgId, "DTC-SHIRT-001", "Longtail T-Shirt",
                catMensClothing.Id, ProductType.Clothing, 34.50m, 12.00m, 
                "Each", brandDuluth.Id, GenderTarget.Men,
                "Extra-long tails stay tucked. Heavyweight cotton for durability.",
                "t-shirt,longtail,cotton"),
            new[]
            {
                ("S",   "River Blue",  "Cotton", "884592002010", 30m),
                ("M",   "River Blue",  "Cotton", "884592002027", 45m),
                ("L",   "River Blue",  "Cotton", "884592002034", 40m),
                ("XL",  "River Blue",  "Cotton", "884592002041", 35m),
                ("2XL", "River Blue",  "Cotton", "884592002058", 20m),
                ("M",   "Midnight",    "Cotton", "884592002065", 42m),
                ("L",   "Midnight",    "Cotton", "884592002072", 38m),
                ("XL",  "Midnight",    "Cotton", "884592002089", 28m),
                ("M",   "Stone Gray",  "Cotton", "884592002096", 35m),
                ("L",   "Stone Gray",  "Cotton", "884592002103", 32m),
            }
        );

        await AddProduct(
            new Product(orgId, "DTC-FLANNEL-001", "Alaskan Hardgear Flannel Shirt",
                catMensClothing.Id, ProductType.Clothing, 89.50m, 34.00m, 
                "Each", brandDuluth.Id, GenderTarget.Men,
                "Heavyweight flannel for serious outdoor work. Double-woven for warmth.",
                "flannel,shirt,heavyweight,outdoor"),
            new[]
            {
                ("S",   "Red Plaid",   "Flannel", "884592003010", 14m),
                ("M",   "Red Plaid",   "Flannel", "884592003027", 22m),
                ("L",   "Red Plaid",   "Flannel", "884592003034", 20m),
                ("XL",  "Red Plaid",   "Flannel", "884592003041", 18m),
                ("M",   "Blue Plaid",  "Flannel", "884592003058", 20m),
                ("L",   "Blue Plaid",  "Flannel", "884592003065", 18m),
                ("XL",  "Blue Plaid",  "Flannel", "884592003072", 15m),
                ("2XL", "Blue Plaid",  "Flannel", "884592003089", 10m),
            }
        );

        // ── Women's Clothing ─────────────────────────────────────────────────

        await AddProduct(
            new Product(orgId, "DTC-WPANT-001", "Women's Flex Fire Hose Work Pants",
                catWomensClothing.Id, ProductType.Clothing, 79.50m, 30.00m, 
                "Each", brandDuluth.Id, GenderTarget.Women,
                "All the toughness of Fire Hose with a cut designed for women.",
                "workwear,pants,women,flex"),
            new[]
            {
                ("2",  "Midnight", "Canvas", "884592004010", 12m),
                ("4",  "Midnight", "Canvas", "884592004027", 18m),
                ("6",  "Midnight", "Canvas", "884592004034", 20m),
                ("8",  "Midnight", "Canvas", "884592004041", 18m),
                ("10", "Midnight", "Canvas", "884592004058", 15m),
                ("12", "Midnight", "Canvas", "884592004065", 12m),
                ("6",  "Khaki",    "Canvas", "884592004072", 16m),
                ("8",  "Khaki",    "Canvas", "884592004089", 14m),
                ("10", "Khaki",    "Canvas", "884592004096", 12m),
            }
        );

        await AddProduct(
            new Product(orgId, "DTC-WSHIRT-001", "Women's Longtail T-Shirt",
                catWomensClothing.Id, ProductType.Clothing, 32.50m, 11.00m, 
                "Each", brandDuluth.Id, GenderTarget.Women,
                "Longer length stays tucked, no matter how you move.",
                "t-shirt,longtail,women"),
            new[]
            {
                ("XS", "Blushing Taupe", "Cotton", "884592005010", 20m),
                ("S",  "Blushing Taupe", "Cotton", "884592005027", 25m),
                ("M",  "Blushing Taupe", "Cotton", "884592005034", 28m),
                ("L",  "Blushing Taupe", "Cotton", "884592005041", 22m),
                ("XL", "Blushing Taupe", "Cotton", "884592005058", 15m),
                ("S",  "Navy",           "Cotton", "884592005065", 22m),
                ("M",  "Navy",           "Cotton", "884592005072", 26m),
                ("L",  "Navy",           "Cotton", "884592005089", 20m),
            }
        );

        // ── Underwear & Base Layer ───────────────────────────────────────────

        await AddProduct(
            new Product(orgId, "DTC-BUCK-M-001", "Buck Naked Performance Underwear — Men",
                catUnderwear.Id, ProductType.Clothing, 24.50m, 8.50m, 
                "Each", brandDuluth.Id, GenderTarget.Men,
                "No chafing, no bunching, no baloney. Our most popular underwear.",
                "underwear,buck-naked,men,performance"),
            new[]
            {
                ("S",   "Slate Gray",  "Nylon/Spandex", "884592006010", 40m),
                ("M",   "Slate Gray",  "Nylon/Spandex", "884592006027", 55m),
                ("L",   "Slate Gray",  "Nylon/Spandex", "884592006034", 50m),
                ("XL",  "Slate Gray",  "Nylon/Spandex", "884592006041", 42m),
                ("2XL", "Slate Gray",  "Nylon/Spandex", "884592006058", 30m),
                ("M",   "Navy",        "Nylon/Spandex", "884592006065", 48m),
                ("L",   "Navy",        "Nylon/Spandex", "884592006072", 45m),
                ("XL",  "Navy",        "Nylon/Spandex", "884592006089", 35m),
                ("M",   "Black",       "Nylon/Spandex", "884592006096", 52m),
                ("L",   "Black",       "Nylon/Spandex", "884592006103", 48m),
            }
        );

        await AddProduct(
            new Product(orgId, "DTC-BUCK-W-001", "Buck Naked Performance Underwear — Women",
                catUnderwear.Id, ProductType.Clothing, 22.50m, 8.00m, 
                "Each", brandDuluth.Id, GenderTarget.Women,
                "Silky-smooth, zero chafe. Designed specifically for women.",
                "underwear,buck-naked,women,performance"),
            new[]
            {
                ("XS", "Orchid",    "Nylon/Spandex", "884592007010", 35m),
                ("S",  "Orchid",    "Nylon/Spandex", "884592007027", 40m),
                ("M",  "Orchid",    "Nylon/Spandex", "884592007034", 45m),
                ("L",  "Orchid",    "Nylon/Spandex", "884592007041", 38m),
                ("XL", "Orchid",    "Nylon/Spandex", "884592007058", 25m),
                ("S",  "Black",     "Nylon/Spandex", "884592007065", 42m),
                ("M",  "Black",     "Nylon/Spandex", "884592007072", 50m),
                ("L",  "Black",     "Nylon/Spandex", "884592007089", 40m),
            }
        );

        // ── Footwear ─────────────────────────────────────────────────────────

        await AddProduct(
            new Product(orgId, "DTC-BOOT-001", "Duluth XTRATUF Ankle Deck Boot — Men",
                catMensFootwear.Id, ProductType.Footwear, 119.00m, 52.00m, 
                "Pair", brandDuluth.Id, GenderTarget.Men,
                "100% waterproof. Non-marking, slip-resistant sole. Built for wet conditions.",
                "boots,waterproof,men,work"),
            new[]
            {
                ("8",  "Brown", "Rubber/Neoprene", "884592008010", 8m),
                ("9",  "Brown", "Rubber/Neoprene", "884592008027", 10m),
                ("10", "Brown", "Rubber/Neoprene", "884592008034", 12m),
                ("11", "Brown", "Rubber/Neoprene", "884592008041", 10m),
                ("12", "Brown", "Rubber/Neoprene", "884592008058", 8m),
                ("13", "Brown", "Rubber/Neoprene", "884592008065", 5m),
                ("10", "Black", "Rubber/Neoprene", "884592008072", 10m),
                ("11", "Black", "Rubber/Neoprene", "884592008089", 8m),
                ("12", "Black", "Rubber/Neoprene", "884592008096", 6m),
            }
        );

        await AddProduct(
            new Product(orgId, "DTC-TRAIL-W-001", "Women's Dry on the Fly Trail Runner",
                catWomensFootwear.Id, ProductType.Footwear, 99.00m, 42.00m, 
                "Pair", brandDuluth.Id, GenderTarget.Women,
                "Lightweight, breathable trail runner with moisture-wicking tech.",
                "trail,running,women,breathable"),
            new[]
            {
                ("6",   "Slate/Teal", "Mesh/Rubber", "884592009010", 8m),
                ("7",   "Slate/Teal", "Mesh/Rubber", "884592009027", 12m),
                ("8",   "Slate/Teal", "Mesh/Rubber", "884592009034", 14m),
                ("9",   "Slate/Teal", "Mesh/Rubber", "884592009041", 12m),
                ("10",  "Slate/Teal", "Mesh/Rubber", "884592009058", 8m),
                ("7",   "Navy/Pink",  "Mesh/Rubber", "884592009065", 10m),
                ("8",   "Navy/Pink",  "Mesh/Rubber", "884592009072", 12m),
                ("9",   "Navy/Pink",  "Mesh/Rubber", "884592009089", 10m),
            }
        );

        // ── Socks ─────────────────────────────────────────────────────────────

        await AddProduct(
            new Product(orgId, "DT-SOCK-HIKE-001", "Darn Tough Vermont Hiker Micro Crew",
                catSocks.Id, ProductType.Accessory, 26.00m, 10.00m, 
                "Pair", brandDarnTough.Id, GenderTarget.Unisex,
                "Merino wool, guaranteed for life. The last hiking sock you'll ever buy.",
                "socks,merino,hiking,lifetime-guarantee"),
            new[]
            {
                ("S",  "Olive",  "Merino Wool", "885577001010", 30m),
                ("M",  "Olive",  "Merino Wool", "885577001027", 45m),
                ("L",  "Olive",  "Merino Wool", "885577001034", 40m),
                ("XL", "Olive",  "Merino Wool", "885577001041", 25m),
                ("S",  "Black",  "Merino Wool", "885577001058", 28m),
                ("M",  "Black",  "Merino Wool", "885577001065", 42m),
                ("L",  "Black",  "Merino Wool", "885577001072", 38m),
            }
        );

        await AddProduct(
            new Product(orgId, "F2F-SOCK-RUN-001", "Farm to Feet Burlington Running Sock",
                catSocks.Id, ProductType.Accessory, 20.00m, 7.50m, 
                "Pair", brandFarmFeet.Id, GenderTarget.Unisex,
                "American-made merino wool. Cushioned, moisture-wicking running sock.",
                "socks,merino,running,american-made"),
            new[]
            {
                ("S",  "Navy",   "Merino Wool", "886488001010", 22m),
                ("M",  "Navy",   "Merino Wool", "886488001027", 35m),
                ("L",  "Navy",   "Merino Wool", "886488001034", 30m),
                ("M",  "Gray",   "Merino Wool", "886488001041", 32m),
                ("L",  "Gray",   "Merino Wool", "886488001058", 28m),
            }
        );

        // ── Wallets ───────────────────────────────────────────────────────────

        await AddProduct(
            new Product(orgId, "DTC-WALL-001", "Duluth RFID-Blocking Leather Bifold Wallet",
                catWallets.Id, ProductType.Accessory, 49.50m, 18.00m, 
                "Each", brandDuluth.Id, GenderTarget.Unisex,
                "Full-grain leather. RFID-blocking tech protects your cards. Made to last.",
                "wallet,leather,rfid,bifold"),
            new[]
            {
                ("One Size", "Dark Brown", "Full-Grain Leather", "884592010010", 30m),
                ("One Size", "Black",      "Full-Grain Leather", "884592010027", 35m),
                ("One Size", "Tan",        "Full-Grain Leather", "884592010034", 20m),
            }
        );

        await AddProduct(
            new Product(orgId, "DTC-WALL-002", "Duluth Slim Card Holder Wallet",
                catWallets.Id, ProductType.Accessory, 34.50m, 12.00m, 
                "Each", brandDuluth.Id, GenderTarget.Unisex,
                "Holds up to 8 cards. Minimalist design with rugged leather construction.",
                "wallet,slim,minimalist,leather"),
            new[]
            {
                ("One Size", "Black",  "Full-Grain Leather", "884592011010", 25m),
                ("One Size", "Brown",  "Full-Grain Leather", "884592011027", 20m),
            }
        );

        // ── Candy & Chocolates ───────────────────────────────────────────────

        await AddProduct(
            new Product(orgId, "GHI-CHOC-001", "Ghirardelli Dark Chocolate Square — 72%",
                catCandy.Id, ProductType.Food, 4.99m, 1.80m, 
                "Each", brandGhirardelli.Id, GenderTarget.None,
                "Individually wrapped dark chocolate. Rich, intense flavor. 72% cacao.",
                "chocolate,dark,72-percent,ghirardelli"),
            new (string, string?, string?, string?, decimal)[]
            {
                ("Single",    null, null, "747599310019", 200m),
                ("Box of 6",  null, null, "747599310026", 80m),
                ("Box of 12", null, null, "747599310033", 50m),
            }
        );

        await AddProduct(
            new Product(orgId, "GHI-CHOC-002", "Ghirardelli Milk Chocolate Caramel Square",
                catCandy.Id, ProductType.Food, 4.99m, 1.80m, 
                "Each", brandGhirardelli.Id, GenderTarget.None,
                "Creamy milk chocolate wrapped around soft caramel filling.",
                "chocolate,milk,caramel,ghirardelli"),
            new (string, string?, string?, string?, decimal)[]
            {
                ("Single",    null, null, "747599320018", 180m),
                ("Box of 6",  null, null, "747599320025", 70m),
                ("Box of 12", null, null, "747599320032", 45m),
            }
        );

        await AddProduct(
            new Product(orgId, "DTC-CANDY-001", "Duluth Trail Mix Chocolate Bark",
                catCandy.Id, ProductType.Food, 12.99m, 4.50m, 
                "Each", brandDuluth.Id, GenderTarget.None,
                "Dark chocolate bark loaded with nuts, seeds, and dried fruit. Trail-ready energy.",
                "candy,chocolate,trail-mix,bark"),
            new (string, string?, string?, string?, decimal)[]
            {
                ("4 oz",  null, null, "884592020010", 60m),
                ("8 oz",  null, null, "884592020027", 45m),
                ("16 oz", null, null, "884592020034", 25m),
            }
        );

        // ── Creams & Lotions ─────────────────────────────────────────────────

        await AddProduct(
            new Product(orgId, "DTC-CREAM-001", "Duluth Trading Lumber Liquidator Hand Cream",
                catCream.Id, ProductType.PersonalCare, 14.99m, 5.50m, 
                "Each", brandDuluth.Id, GenderTarget.Unisex,
                "Heavy-duty hand cream. Works fast for cracked, rough hands. Unscented.",
                "cream,hand,heavy-duty,unscented"),
            new (string, string?, string?, string?, decimal)[]
            {
                ("2 oz", null, null, "884592030010", 50m),
                ("4 oz", null, null, "884592030027", 60m),
                ("8 oz", null, null, "884592030034", 40m),
            }
        );

        await AddProduct(
            new Product(orgId, "DTC-CREAM-002", "Duluth Aloe Vera After-Sun Lotion",
                catCream.Id, ProductType.PersonalCare, 11.99m, 4.00m, 
                "Each", brandDuluth.Id, GenderTarget.Unisex,
                "Soothing aloe vera lotion for sun-exposed skin. Lightweight, fast absorbing.",
                "lotion,aloe-vera,after-sun,soothing"),
            new (string, string?, string?, string?, decimal)[]
            {
                ("4 oz", null, null, "884592031010", 40m),
                ("8 oz", null, null, "884592031027", 35m),
            }
        );

        logger.LogInformation("Product catalog seeded.");
    }

    // ── Customers ─────────────────────────────────────────────────────────────

    private static async Task SeedCustomersAsync(AppDbContext db, ILogger logger, Guid orgId)
    {
        if (await db.Customers.IgnoreQueryFilters().AnyAsync(c => c.OrganizationId == orgId)) return;
        logger.LogInformation("Seeding customers...");
        db.Customers.AddRange(
            new Customer(orgId, "CUST-00001", "OutdoorGear Wholesale",   "orders@outdoorgear.com",    "+1-555-0101", "123 Main St, Denver, CO",   "USD", 30, 100000m),
            new Customer(orgId, "CUST-00002", "Rocky Mountain Outfitters","purchase@rmo.com",          "+1-555-0102", "456 Trail Blvd, Boulder",   "USD", 14,  25000m),
            new Customer(orgId, "CUST-00003", "Backcountry Direct",       "buying@backcountry.com",    "+1-555-0103", "789 Summit Ave, Salt Lake", "USD", 45,  75000m),
            new Customer(orgId, "CUST-00004", "Farm & Ranch Supply",      "info@farmranch.com",        "+1-555-0104", "12 Prairie Rd, Sioux City", "USD", 30,  40000m),
            new Customer(orgId, "CUST-00005", "Work Wear Direct",         "workwear@wwd.com",          "+1-555-0105", "88 Industrial Pkwy, Omaha", "USD",  7,  10000m)
        );
        await db.SaveChangesAsync();
    }

    // ── Vendors ───────────────────────────────────────────────────────────────

    private static async Task SeedVendorsAsync(AppDbContext db, ILogger logger, Guid orgId)
    {
        if (await db.Vendors.IgnoreQueryFilters().AnyAsync(v => v.OrganizationId == orgId)) return;
        logger.LogInformation("Seeding vendors...");
        db.Vendors.AddRange(
            new Vendor(orgId, "VEND-00001", "American Canvas Mills",   "orders@acmills.com",      "+1-555-0201", "1 Mill Rd, Greenville, SC",  "USD", 30, "SC-1234567"),
            new Vendor(orgId, "VEND-00002", "Pacific Rim Garments",    "supply@pacrim.com",       "+1-555-0202", "200 Harbor Dr, Los Angeles", "USD", 45, "CA-7654321"),
            new Vendor(orgId, "VEND-00003", "Vermont Wool Collective",  "info@vtwool.com",         "+1-555-0203", "50 Sheep Ln, Montpelier, VT","USD", 15, "VT-1122334"),
            new Vendor(orgId, "VEND-00004", "Sole Craft Footwear",     "sales@solecraft.com",     "+1-555-0204", "300 Boot Ave, Portland, OR", "USD", 30, "OR-9988776"),
            new Vendor(orgId, "VEND-00005", "Premium Confections Co.", "import@premconfect.com",  "+1-555-0205", "75 Cocoa Blvd, San Fran, CA","USD", 60, "CA-5544332")
        );
        await db.SaveChangesAsync();
    }

    // ── System Admin — Roles, Permissions, Admin User ─────────────────────────

    private static async Task SeedSystemAdminAsync(AppDbContext db, ILogger logger, Guid orgId)
    {
        // Idempotency: skip if any roles already exist for this org
        if (await db.Roles.IgnoreQueryFilters().AnyAsync(r => r.OrganizationId == orgId)) return;

        logger.LogInformation("Seeding system admin roles, permissions, and admin user...");

        // ── Roles ─────────────────────────────────────────────────────────────
        string[] allModules      = ["GL", "AR", "AP", "PM", "SysAdmin"];
        string[] bizModules      = ["GL", "AR", "AP", "PM"];
        string[] allActions      = ["Read", "Write", "Delete", "Approve"];
        string[] rwActions       = ["Read", "Write"];

        // 1. Admin — unrestricted
        var adminRole = new Role(orgId, "Admin",
            "Full system access — all modules and actions", isSystemRole: true);
        foreach (var m in allModules) foreach (var a in allActions) adminRole.GrantPermission(m, a);

        // 2. Manager — full business access, read-only SysAdmin
        var managerRole = new Role(orgId, "Manager",
            "Approve transactions and manage day-to-day operations");
        foreach (var m in bizModules) foreach (var a in allActions) managerRole.GrantPermission(m, a);
        managerRole.GrantPermission("SysAdmin", "Read");

        // 3. Accountant — post/write GL+AR+AP, read PM, no delete
        var accountantRole = new Role(orgId, "Accountant",
            "Create, edit, and post financial transactions");
        foreach (var m in new[] { "GL", "AR", "AP" }) foreach (var a in rwActions) accountantRole.GrantPermission(m, a);
        accountantRole.GrantPermission("PM", "Read");

        // 4. Viewer — read-only everywhere
        var viewerRole = new Role(orgId, "Viewer",
            "Read-only access to all modules");
        foreach (var m in allModules) viewerRole.GrantPermission(m, "Read");

        // 5. Security Ops — full SysAdmin (users, roles, audit), read-only business data
        var secOpsRole = new Role(orgId, "SecurityOps",
            "Manage user accounts, roles, permissions, and audit trails");
        foreach (var a in new[] { "Read", "Write", "Delete" }) secOpsRole.GrantPermission("SysAdmin", a);
        foreach (var m in bizModules) secOpsRole.GrantPermission(m, "Read");

        // 6. Application Support — read all + SysAdmin read+write for troubleshooting
        var appSupportRole = new Role(orgId, "AppSupport",
            "Application support and troubleshooting — read all, manage users");
        foreach (var m in allModules) appSupportRole.GrantPermission(m, "Read");
        appSupportRole.GrantPermission("SysAdmin", "Write");

        // 7. End User — read business data, no admin access
        var endUserRole = new Role(orgId, "EndUser",
            "Standard ERP user — read-only access to business modules");
        foreach (var m in bizModules) endUserRole.GrantPermission(m, "Read");

        db.Roles.AddRange(adminRole, managerRole, accountantRole, viewerRole,
                          secOpsRole, appSupportRole, endUserRole);
        await db.SaveChangesAsync();

        // ── Admin user ────────────────────────────────────────────────────────
        var adminExists = await db.AppUsers.IgnoreQueryFilters()
            .AnyAsync(u => u.Username == "admin" && u.OrganizationId == orgId);
        if (!adminExists)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword("Admin@123!", workFactor: 12);
            var adminUser = new AppUser(orgId, "admin", "admin@dessert-erp.com",
                "System Administrator", passwordHash);
            adminUser.AssignRole(adminRole.Id);
            db.AppUsers.Add(adminUser);
            await db.SaveChangesAsync();
            logger.LogInformation("Default admin user created — username: admin, password: Admin@123!");
        }
    }

    private static async Task RemoveLegacySystemAdminRoleAsync(AppDbContext db, ILogger logger)
    {
        var legacyRoles = await db.Roles.IgnoreQueryFilters()
            .Where(role => role.Name == "SystemAdmin")
            .ToListAsync();

        if (legacyRoles.Count == 0)
            return;

        db.Roles.RemoveRange(legacyRoles);
        await db.SaveChangesAsync();
        logger.LogInformation("Removed the obsolete SystemAdmin role; Admin is the full-access role.");
    }

}
