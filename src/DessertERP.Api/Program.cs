using System.Text;
using System.Text.Json.Serialization;
using Microsoft.OpenApi.Models;



using DessertERP.Application.Common.Interfaces;
using DessertERP.Application.Common.Security;
using DessertERP.Application.Common.Services;
using DessertERP.Application.Modules.AccountsPayable.Services;
using DessertERP.Application.Modules.AccountsReceivable.Services;
using DessertERP.Application.Modules.DataManagement.Services;
using DessertERP.Application.Modules.GeneralLedger.Services;
using DessertERP.Application.Modules.Organization.Services;
using DessertERP.Application.Modules.ProductManagement.Services;
using DessertERP.Application.Modules.InventoryManagement.Services;
using DessertERP.Application.Modules.Workflow.Services;
using DessertERP.Application.Modules.Expenses.Services;
using DessertERP.Application.Modules.CashBank;
using DessertERP.Infrastructure.Modules.CashBank;
using DessertERP.Application.Modules.FixedAssets;
using DessertERP.Infrastructure.Modules.FixedAssets;
using DessertERP.Application.Modules.WarehouseManagement;
using DessertERP.Infrastructure.Modules.WarehouseManagement;
using DessertERP.Application.Modules.Marketing.Services;
using DessertERP.Application.Modules.ProductManagement.Services;
using DessertERP.Application.Modules.Retail.Services;
using DessertERP.Application.Modules.SystemAdmin.Services;
using DessertERP.Infrastructure.Persistence;
using DessertERP.Infrastructure.Persistence.Seed;
using DessertERP.Infrastructure.Services;
using DessertERP.Infrastructure.Storage;
using DessertERP.Worker.Jobs;
using DessertERP.Worker.Workers;
using Hangfire;
using Hangfire.PostgreSql;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
//using Microsoft.OpenApi.Models;



// ── Npgsql: treat DateTime(Unspecified) as UTC globally ──────────────────────
// Angular sends dates as ISO strings (e.g. "2026-06-02") which .NET deserializes
// as Kind=Unspecified. Npgsql 6+ requires Kind=Utc for timestamptz columns.
AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

var builder = WebApplication.CreateBuilder(args);

// ── HTTP Context (needed for CurrentOrganizationService) ─────────────────────
builder.Services.AddHttpContextAccessor();


// ── Database ──────────────────────────────────────────────────────────────────
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection")
    ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseNpgsql(connectionString,
        npgsql => npgsql.MigrationsAssembly("DessertERP.Infrastructure")));

builder.Services.AddScoped<IAppDbContext>(sp => sp.GetRequiredService<AppDbContext>());

// ── Organization scope ────────────────────────────────────────────────────────
builder.Services.AddScoped<ICurrentOrganizationService, CurrentOrganizationService>();
builder.Services.AddScoped<ICurrentUserService, CurrentUserService>();
builder.Services.AddScoped<IDocumentAuditService, DocumentAuditService>();

// ── Application Services ──────────────────────────────────────────────────────
builder.Services.AddScoped<IOrganizationService, OrganizationService>();
builder.Services.AddScoped<IGeneralLedgerService, GeneralLedgerService>();
builder.Services.AddScoped<IAccountsReceivableService, AccountsReceivableService>();
builder.Services.AddScoped<IAccountsPayableService, AccountsPayableService>();
builder.Services.AddScoped<IProductManagementService, ProductManagementService>();
builder.Services.AddScoped<IInventoryManagementService, InventoryManagementService>();
builder.Services.AddScoped<IPurchaseInventoryPostingService, PurchaseInventoryPostingService>();
builder.Services.AddScoped<IWorkflowService, WorkflowService>();
builder.Services.AddScoped<IExpenseService, ExpenseService>();
builder.Services.AddScoped<IWarehouseManagementService, WarehouseManagementService>();
builder.Services.AddScoped<ICashBankService, CashBankService>();
builder.Services.AddScoped<IFixedAssetService, FixedAssetService>();

// ── Data Management ───────────────────────────────────────────────────────────
builder.Services.AddScoped<IDataManagementService, DataManagementService>();
builder.Services.AddScoped<IBatchJobService, BatchJobService>();
builder.Services.AddScoped<IRetailService, RetailService>();
builder.Services.AddScoped<IMarketingService, MarketingService>();
builder.Services.AddScoped<IPriceAgreementService, PriceAgreementService>();
builder.Services.AddSingleton<IBlobStorageService, AzureBlobStorageService>();
builder.Services.AddSingleton<IFileShareService, AzureFileShareService>();

// ── Hangfire (server runs inside the API process) ─────────────────────────────
builder.Services.AddHangfire(config =>
    config.UsePostgreSqlStorage(c =>
        c.UseNpgsqlConnection(connectionString)));

builder.Services.AddHangfireServer(options =>
{
    options.WorkerCount = 4;
    options.ServerName  = "DessertERP.Api.HangfireServer";
});

// Batch job hosted services and job classes
builder.Services.AddHostedService<BatchJobSchedulerWorker>(); // syncs DB → Hangfire every 60s
builder.Services.AddScoped<BatchImportJob>();
builder.Services.AddScoped<BatchExportJob>();
builder.Services.AddScoped<ImportBatchJob>();
builder.Services.AddScoped<ImportBatchSweepJob>();

// ── System Admin services ─────────────────────────────────────────────────────
builder.Services.AddScoped<IPasswordHasher, BcryptPasswordHasher>();
builder.Services.AddScoped<IJwtTokenService, JwtTokenService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ISystemAdminService, SystemAdminService>();

// ── JWT Authentication ────────────────────────────────────────────────────────
var jwtSection = builder.Configuration.GetSection("JwtSettings");
var jwtSecret  = jwtSection["Secret"]   ?? throw new InvalidOperationException("JwtSettings:Secret missing.");
var jwtIssuer  = jwtSection["Issuer"]   ?? "DessertERP";
var jwtAudience= jwtSection["Audience"] ?? "DessertERP";

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme    = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuerSigningKey = true,
        IssuerSigningKey         = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret)),
        ValidateIssuer           = true,
        ValidIssuer              = jwtIssuer,
        ValidateAudience         = true,
        ValidAudience            = jwtAudience,
        ValidateLifetime         = true,
        ClockSkew                = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization(options =>
{
    foreach (var permission in PermissionCatalog.PolicyKeys)
    {
        options.AddPolicy(permission, policy =>
            policy.RequireClaim("permission", permission));
    }
});

// ── API ───────────────────────────────────────────────────────────────────────
builder.Services.AddControllers()
    .AddJsonOptions(o =>
        o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()));

builder.Services.AddEndpointsApiExplorer();

// ── Swagger with JWT support ──────────────────────────────────────────────────
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Dessert ERP API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name         = "Authorization",
        Type         = SecuritySchemeType.Http,
        Scheme       = "bearer",
        BearerFormat = "JWT",
        In           = ParameterLocation.Header,
        Description  = "Enter your JWT token (without the 'Bearer ' prefix)."
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme { Reference = new OpenApiReference { Type = ReferenceType.SecurityScheme, Id = "Bearer" } },
            Array.Empty<string>()
        }
    });
});

// ── CORS (Angular dev server) ─────────────────────────────────────────────────
var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>()
    ?.Where(origin => !string.IsNullOrWhiteSpace(origin))
    .Select(origin => origin.TrimEnd('/'))
    .Distinct(StringComparer.OrdinalIgnoreCase)
    .ToArray() ?? [];

if (allowedOrigins.Length == 0)
{
    throw new InvalidOperationException(
        "At least one CORS origin must be configured in Cors:AllowedOrigins.");
}

builder.Services.AddCors(options =>
    options.AddPolicy("AllowFrontend", policy =>
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod()));

var app = builder.Build();

// ── Seed database + register Hangfire sweep job ───────────────────────────────
using (var scope = app.Services.CreateScope())
{
    var db     = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

    if (builder.Configuration.GetValue("DatabaseInitialization:ApplyMigrationsOnStartup", true))
    {
        var startedAt = System.Diagnostics.Stopwatch.GetTimestamp();
        await db.Database.MigrateAsync();
        logger.LogInformation(
            "Database migrations completed in {ElapsedMs} ms.",
            System.Diagnostics.Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds);
    }

    if (builder.Configuration.GetValue(
            "DatabaseInitialization:SeedOnStartup",
            app.Environment.IsDevelopment()))
    {
        var startedAt = System.Diagnostics.Stopwatch.GetTimestamp();
        await DatabaseSeeder.SeedAsync(db, logger);
        logger.LogInformation(
            "Database seeding completed in {ElapsedMs} ms.",
            System.Diagnostics.Stopwatch.GetElapsedTime(startedAt).TotalMilliseconds);
    }

    // Register the import sweep job once at startup
    var recurring = scope.ServiceProvider.GetRequiredService<IRecurringJobManager>();
    recurring.AddOrUpdate<ImportBatchSweepJob>(
        "sweep-queued-imports",
        j => j.SweepAsync(),
        Cron.Hourly());
}

// ── Middleware ────────────────────────────────────────────────────────────────
//app.UseDeveloperExceptionPage(); // full stack trace in response — remove before production

// Keep CORS outside the exception handler so handled application errors also
// include the appropriate Access-Control-Allow-Origin response header.
app.UseCors("AllowFrontend");

if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/error");
    app.UseHsts();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapGet("/health", () => Results.Ok(new
{
    status = "Healthy",
    timestampUtc = DateTimeOffset.UtcNow
})).AllowAnonymous();
app.MapControllers();

// Hangfire dashboard (restrict in production as needed)
app.UseHangfireDashboard("/hangfire", new DashboardOptions
{
    IsReadOnlyFunc = _ => false,  // set true to make it read-only in prod
    Authorization  = []           // no extra auth on top of the app's auth for now
});

app.Run();
