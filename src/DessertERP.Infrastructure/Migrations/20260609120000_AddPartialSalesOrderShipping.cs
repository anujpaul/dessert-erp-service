using DessertERP.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations;

[DbContext(typeof(AppDbContext))]
[Migration("20260609120000_AddPartialSalesOrderShipping")]
public partial class AddPartialSalesOrderShipping : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.AddColumn<decimal>(
            name: "QuantityShipped",
            table: "sales_order_lines",
            type: "numeric(18,4)",
            nullable: false,
            defaultValue: 0m);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropColumn(
            name: "QuantityShipped",
            table: "sales_order_lines");
    }
}
