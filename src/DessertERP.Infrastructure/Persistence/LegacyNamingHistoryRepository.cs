using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Migrations.Internal;

namespace DessertERP.Infrastructure.Persistence;

#pragma warning disable EF1001 // Required to preserve the existing EF migrations history schema.
public sealed class LegacyNamingHistoryRepository(HistoryRepositoryDependencies dependencies)
    : NpgsqlHistoryRepository(dependencies)
{
    protected override void ConfigureTable(EntityTypeBuilder<HistoryRow> history)
    {
        base.ConfigureTable(history);
        history.Property(row => row.MigrationId).HasColumnName("MigrationId");
        history.Property(row => row.ProductVersion).HasColumnName("ProductVersion");
    }
}
#pragma warning restore EF1001
