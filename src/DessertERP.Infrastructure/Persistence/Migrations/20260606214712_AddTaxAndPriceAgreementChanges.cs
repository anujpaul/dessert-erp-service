using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddTaxAndPriceAgreementChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PriceAgreements_categories_CategoryId",
                table: "PriceAgreements");

            migrationBuilder.DropIndex(
                name: "IX_PriceAgreements_CategoryId",
                table: "PriceAgreements");

            migrationBuilder.DropIndex(
                name: "IX_PriceAgreements_ProductId",
                table: "PriceAgreements");

            migrationBuilder.DropIndex(
                name: "IX_PriceAgreements_VariantId",
                table: "PriceAgreements");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "PriceAgreements");

            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "catalog_products");

            migrationBuilder.AlterColumn<string>(
                name: "PriceType",
                table: "PriceAgreements",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30);

            migrationBuilder.AddColumn<string>(
                name: "TaxCode",
                table: "categories",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxRate",
                table: "categories",
                type: "numeric(8,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxRateOverride",
                table: "catalog_products",
                type: "numeric(8,4)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PriceAgreements_ProductId_PriceType_IsActive",
                table: "PriceAgreements",
                columns: new[] { "ProductId", "PriceType", "IsActive" });

            migrationBuilder.CreateIndex(
                name: "IX_PriceAgreements_VariantId_PriceType_IsActive",
                table: "PriceAgreements",
                columns: new[] { "VariantId", "PriceType", "IsActive" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_PriceAgreements_ProductId_PriceType_IsActive",
                table: "PriceAgreements");

            migrationBuilder.DropIndex(
                name: "IX_PriceAgreements_VariantId_PriceType_IsActive",
                table: "PriceAgreements");

            migrationBuilder.DropColumn(
                name: "TaxCode",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "TaxRate",
                table: "categories");

            migrationBuilder.DropColumn(
                name: "TaxRateOverride",
                table: "catalog_products");

            migrationBuilder.AlterColumn<string>(
                name: "PriceType",
                table: "PriceAgreements",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "PriceAgreements",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TaxRate",
                table: "catalog_products",
                type: "numeric(8,4)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.CreateIndex(
                name: "IX_PriceAgreements_CategoryId",
                table: "PriceAgreements",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceAgreements_ProductId",
                table: "PriceAgreements",
                column: "ProductId");

            migrationBuilder.CreateIndex(
                name: "IX_PriceAgreements_VariantId",
                table: "PriceAgreements",
                column: "VariantId");

            migrationBuilder.AddForeignKey(
                name: "FK_PriceAgreements_categories_CategoryId",
                table: "PriceAgreements",
                column: "CategoryId",
                principalTable: "categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
