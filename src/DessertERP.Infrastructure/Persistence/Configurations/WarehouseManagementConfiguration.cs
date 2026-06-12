using DessertERP.Domain.Modules.WarehouseManagement;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class WarehouseConfiguration : IEntityTypeConfiguration<Warehouse>
{
    public void Configure(EntityTypeBuilder<Warehouse> b)
    {
        b.ToTable("warehouses");
        b.HasKey(e => e.Id);
        b.Property(e => e.Code).IsRequired().HasMaxLength(20);
        b.Property(e => e.Name).IsRequired().HasMaxLength(200);
        b.Property(e => e.Address).HasMaxLength(500);
        b.Property(e => e.City).HasMaxLength(100);
        b.Property(e => e.Country).HasMaxLength(100);
        b.HasOne(e => e.WarehouseType).WithMany()
            .HasForeignKey(e => e.WarehouseTypeId).OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.Site).WithMany()
            .HasForeignKey(e => e.SiteId).OnDelete(DeleteBehavior.Restrict);
        b.HasIndex(e => new { e.OrganizationId, e.Code }).IsUnique();
        b.HasMany(e => e.Locations)
         .WithOne(l => l.Warehouse)
         .HasForeignKey(l => l.WarehouseId)
         .OnDelete(DeleteBehavior.Restrict);
    }
}

public class WarehouseLocationConfiguration : IEntityTypeConfiguration<WarehouseLocation>
{
    public void Configure(EntityTypeBuilder<WarehouseLocation> b)
    {
        b.ToTable("warehouse_locations");
        b.HasKey(e => e.Id);
        b.Property(e => e.Code).IsRequired().HasMaxLength(50);
        b.Property(e => e.Zone).HasMaxLength(50);
        b.Property(e => e.Aisle).HasMaxLength(20);
        b.Property(e => e.Bay).HasMaxLength(20);
        b.Property(e => e.Level).HasMaxLength(20);
        b.Property(e => e.Bin).HasMaxLength(20);
        b.HasIndex(e => new { e.WarehouseId, e.Code }).IsUnique();
    }
}

public class WarehouseTypeConfiguration : IEntityTypeConfiguration<WarehouseType>
{
    public void Configure(EntityTypeBuilder<WarehouseType> b)
    {
        b.ToTable("warehouse_types");
        b.HasKey(e => e.Id);
        b.Property(e => e.Name).IsRequired().HasMaxLength(100);
        b.Property(e => e.Description).HasMaxLength(500);
        b.HasIndex(e => new { e.OrganizationId, e.Name }).IsUnique();
    }
}

public class OperationalSiteConfiguration : IEntityTypeConfiguration<OperationalSite>
{
    public void Configure(EntityTypeBuilder<OperationalSite> b)
    {
        b.ToTable("operational_sites");
        b.HasKey(e => e.Id);
        b.Property(e => e.Code).IsRequired().HasMaxLength(30);
        b.Property(e => e.Name).IsRequired().HasMaxLength(200);
        b.Property(e => e.Address).HasMaxLength(500);
        b.Property(e => e.City).HasMaxLength(100);
        b.Property(e => e.Country).HasMaxLength(100);
        b.HasIndex(e => new { e.OrganizationId, e.Code }).IsUnique();
    }
}

public class WarehouseInventoryBalanceConfiguration : IEntityTypeConfiguration<WarehouseInventoryBalance>
{
    public void Configure(EntityTypeBuilder<WarehouseInventoryBalance> b)
    {
        b.ToTable("warehouse_inventory_balances");
        b.HasKey(e => e.Id);
        b.Property(e => e.QuantityOnHand).HasColumnType("decimal(18,4)");
        b.Property(e => e.QuantityReserved).HasColumnType("decimal(18,4)");
        b.Ignore(e => e.QuantityAvailable);
        b.HasIndex(e => new
        {
            e.OrganizationId,
            e.ProductVariantId,
            e.WarehouseId,
            e.WarehouseLocationId
        }).IsUnique();
        b.HasOne(e => e.ProductVariant)
            .WithMany()
            .HasForeignKey(e => e.ProductVariantId)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.Warehouse)
            .WithMany()
            .HasForeignKey(e => e.WarehouseId)
            .OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.WarehouseLocation)
            .WithMany()
            .HasForeignKey(e => e.WarehouseLocationId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

public class InboundOrderConfiguration : IEntityTypeConfiguration<InboundOrder>
{
    public void Configure(EntityTypeBuilder<InboundOrder> b)
    {
        b.ToTable("inbound_orders");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
        b.Property(e => e.VendorName).HasMaxLength(200);
        b.Property(e => e.Notes).HasMaxLength(1000);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.HasIndex(e => new { e.OrganizationId, e.OrderNumber }).IsUnique();
        b.HasMany(e => e.Lines)
         .WithOne(l => l.InboundOrder)
         .HasForeignKey(l => l.InboundOrderId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}

public class InboundOrderLineConfiguration : IEntityTypeConfiguration<InboundOrderLine>
{
    public void Configure(EntityTypeBuilder<InboundOrderLine> b)
    {
        b.ToTable("inbound_order_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
        b.Property(e => e.ProductSku).HasMaxLength(100);
        b.Property(e => e.UnitOfMeasure).IsRequired().HasMaxLength(20);
        b.Property(e => e.LotNumber).HasMaxLength(50);
        b.Property(e => e.Notes).HasMaxLength(500);
        b.Property(e => e.OrderedQuantity).HasColumnType("decimal(18,4)");
        b.Property(e => e.ReceivedQuantity).HasColumnType("decimal(18,4)");
    }
}

public class OutboundOrderConfiguration : IEntityTypeConfiguration<OutboundOrder>
{
    public void Configure(EntityTypeBuilder<OutboundOrder> b)
    {
        b.ToTable("outbound_orders");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
        b.Property(e => e.CustomerName).HasMaxLength(200);
        b.Property(e => e.ShipToAddress).HasMaxLength(500);
        b.Property(e => e.TrackingNumber).HasMaxLength(100);
        b.Property(e => e.Carrier).HasMaxLength(100);
        b.Property(e => e.Notes).HasMaxLength(1000);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.HasIndex(e => new { e.OrganizationId, e.OrderNumber }).IsUnique();
        b.HasMany(e => e.Lines)
         .WithOne(l => l.OutboundOrder)
         .HasForeignKey(l => l.OutboundOrderId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}

public class OutboundOrderLineConfiguration : IEntityTypeConfiguration<OutboundOrderLine>
{
    public void Configure(EntityTypeBuilder<OutboundOrderLine> b)
    {
        b.ToTable("outbound_order_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
        b.Property(e => e.ProductSku).HasMaxLength(100);
        b.Property(e => e.UnitOfMeasure).IsRequired().HasMaxLength(20);
        b.Property(e => e.LotNumber).HasMaxLength(50);
        b.Property(e => e.Notes).HasMaxLength(500);
        b.Property(e => e.RequestedQuantity).HasColumnType("decimal(18,4)");
        b.Property(e => e.PickedQuantity).HasColumnType("decimal(18,4)");
        b.Property(e => e.ShippedQuantity).HasColumnType("decimal(18,4)");
    }
}

public class TransferOrderConfiguration : IEntityTypeConfiguration<TransferOrder>
{
    public void Configure(EntityTypeBuilder<TransferOrder> b)
    {
        b.ToTable("transfer_orders");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrderNumber).IsRequired().HasMaxLength(50);
        b.Property(e => e.Notes).HasMaxLength(1000);
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.HasIndex(e => new { e.OrganizationId, e.OrderNumber }).IsUnique();
        b.HasOne(e => e.FromWarehouse)
         .WithMany()
         .HasForeignKey(e => e.FromWarehouseId)
         .OnDelete(DeleteBehavior.Restrict);
        b.HasOne(e => e.ToWarehouse)
         .WithMany()
         .HasForeignKey(e => e.ToWarehouseId)
         .OnDelete(DeleteBehavior.Restrict);
        b.HasMany(e => e.Lines)
         .WithOne(l => l.TransferOrder)
         .HasForeignKey(l => l.TransferOrderId)
         .OnDelete(DeleteBehavior.Cascade);
    }
}

public class TransferOrderLineConfiguration : IEntityTypeConfiguration<TransferOrderLine>
{
    public void Configure(EntityTypeBuilder<TransferOrderLine> b)
    {
        b.ToTable("transfer_order_lines");
        b.HasKey(e => e.Id);
        b.Property(e => e.ProductName).IsRequired().HasMaxLength(200);
        b.Property(e => e.ProductSku).HasMaxLength(100);
        b.Property(e => e.UnitOfMeasure).IsRequired().HasMaxLength(20);
        b.Property(e => e.LotNumber).HasMaxLength(50);
        b.Property(e => e.Notes).HasMaxLength(500);
        b.Property(e => e.RequestedQuantity).HasColumnType("decimal(18,4)");
        b.Property(e => e.ShippedQuantity).HasColumnType("decimal(18,4)");
        b.Property(e => e.ReceivedQuantity).HasColumnType("decimal(18,4)");
        b.HasOne(e => e.FromLocation)
         .WithMany()
         .HasForeignKey(e => e.FromLocationId)
         .OnDelete(DeleteBehavior.SetNull);
        b.HasOne(e => e.ToLocation)
         .WithMany()
         .HasForeignKey(e => e.ToLocationId)
         .OnDelete(DeleteBehavior.SetNull);
    }
}
