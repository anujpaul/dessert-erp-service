using DessertERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260609153000_AddOrganizationRoundingRules")]
public partial class AddOrganizationRoundingRules : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<int>(
            name: "money_decimal_places",
            table: "organizations",
            type: "integer",
            nullable: false,
            defaultValue: 4);

        migrationBuilder.AddColumn<string>(
            name: "money_rounding_method",
            table: "organizations",
            type: "character varying(20)",
            maxLength: 20,
            nullable: false,
            defaultValue: "HalfUp");

        migrationBuilder.AddColumn<string>(
            name: "money_rounding_level",
            table: "organizations",
            type: "character varying(20)",
            maxLength: 20,
            nullable: false,
            defaultValue: "Line");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(name: "money_decimal_places", table: "organizations");
        migrationBuilder.DropColumn(name: "money_rounding_method", table: "organizations");
        migrationBuilder.DropColumn(name: "money_rounding_level", table: "organizations");
    }
}
