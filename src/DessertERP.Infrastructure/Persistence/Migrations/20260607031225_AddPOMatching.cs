using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddPOMatching : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BypassReason",
                table: "ap_invoices",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "InvoiceType",
                table: "ap_invoices",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "LinkedPrepaymentInvoiceId",
                table: "ap_invoices",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchNotes",
                table: "ap_invoices",
                type: "character varying(2000)",
                maxLength: 2000,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MatchStatus",
                table: "ap_invoices",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "PrepaymentApplied",
                table: "ap_invoices",
                type: "numeric(18,4)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BypassReason",
                table: "ap_invoices");

            migrationBuilder.DropColumn(
                name: "InvoiceType",
                table: "ap_invoices");

            migrationBuilder.DropColumn(
                name: "LinkedPrepaymentInvoiceId",
                table: "ap_invoices");

            migrationBuilder.DropColumn(
                name: "MatchNotes",
                table: "ap_invoices");

            migrationBuilder.DropColumn(
                name: "MatchStatus",
                table: "ap_invoices");

            migrationBuilder.DropColumn(
                name: "PrepaymentApplied",
                table: "ap_invoices");
        }
    }
}
