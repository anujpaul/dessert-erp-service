using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations;

/// <summary>
/// Synchronizes EF's generated model snapshot after the preceding manually
/// authored migrations. Those migrations already applied the current schema.
/// </summary>
public partial class SyncModelSnapshotAfterManualMigrations : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
    }
}
