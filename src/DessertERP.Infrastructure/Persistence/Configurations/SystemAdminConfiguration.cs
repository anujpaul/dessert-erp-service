using DessertERP.Domain.Modules.SystemAdmin;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DessertERP.Infrastructure.Persistence.Configurations;

public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
{
    public void Configure(EntityTypeBuilder<AppUser> b)
    {
        b.ToTable("app_users");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Username).HasMaxLength(100).IsRequired();
        b.Property(e => e.Email).HasMaxLength(200).IsRequired();
        b.Property(e => e.FullName).HasMaxLength(200).IsRequired();
        b.Property(e => e.PasswordHash).HasMaxLength(500).IsRequired();
        b.Property(e => e.Status).HasConversion<string>().HasMaxLength(20);
        b.Property(e => e.RefreshToken).HasMaxLength(500);
        b.HasIndex(e => new { e.OrganizationId, e.Username }).IsUnique();
        b.HasMany(e => e.UserRoles).WithOne()
            .HasForeignKey(ur => ur.UserId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class RoleConfiguration : IEntityTypeConfiguration<Role>
{
    public void Configure(EntityTypeBuilder<Role> b)
    {
        b.ToTable("roles");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Name).HasMaxLength(100).IsRequired();
        b.Property(e => e.Description).HasMaxLength(500);
        b.HasIndex(e => new { e.OrganizationId, e.Name }).IsUnique();
        b.HasMany(e => e.Permissions).WithOne()
            .HasForeignKey(p => p.RoleId).OnDelete(DeleteBehavior.Cascade);
    }
}

public class RolePermissionConfiguration : IEntityTypeConfiguration<RolePermission>
{
    public void Configure(EntityTypeBuilder<RolePermission> b)
    {
        b.ToTable("role_permissions");
        b.HasKey(e => e.Id);
        b.Property(e => e.Module).HasMaxLength(50).IsRequired();
        b.Property(e => e.Action).HasMaxLength(50).IsRequired();
        b.HasIndex(e => new { e.RoleId, e.Module, e.Action }).IsUnique();
    }
}

public class UserRoleConfiguration : IEntityTypeConfiguration<UserRole>
{
    public void Configure(EntityTypeBuilder<UserRole> b)
    {
        b.ToTable("user_roles");
        b.HasKey(e => e.Id);
        b.HasOne(e => e.Role).WithMany()
            .HasForeignKey(e => e.RoleId).OnDelete(DeleteBehavior.Cascade);
        b.HasIndex(e => new { e.UserId, e.RoleId }).IsUnique();
    }
}

public class AuditLogEntryConfiguration : IEntityTypeConfiguration<AuditLogEntry>
{
    public void Configure(EntityTypeBuilder<AuditLogEntry> b)
    {
        b.ToTable("audit_logs");
        b.HasKey(e => e.Id);
        b.Property(e => e.OrganizationId).IsRequired();
        b.Property(e => e.Username).HasMaxLength(100);
        b.Property(e => e.Module).HasMaxLength(50);
        b.Property(e => e.Action).HasMaxLength(100);
        b.Property(e => e.EntityId).HasMaxLength(100);
        b.Property(e => e.EntityType).HasMaxLength(100);
        b.Property(e => e.OldValues).HasMaxLength(4000);
        b.Property(e => e.NewValues).HasMaxLength(4000);
        b.Property(e => e.IpAddress).HasMaxLength(50);
        b.HasIndex(e => new { e.OrganizationId, e.OccurredAt });
        b.HasIndex(e => e.UserId);
    }
}
