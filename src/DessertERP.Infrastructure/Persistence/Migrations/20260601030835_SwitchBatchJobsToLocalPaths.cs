using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SwitchBatchJobsToLocalPaths : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContainerName",
                table: "batch_job_configs");

            migrationBuilder.DropColumn(
                name: "StorageType",
                table: "batch_job_configs");

            migrationBuilder.RenameColumn(
                name: "ProcessedPrefix",
                table: "batch_job_configs",
                newName: "LocalProcessedPath");

            migrationBuilder.RenameColumn(
                name: "InboxPrefix",
                table: "batch_job_configs",
                newName: "LocalInboxPath");

            migrationBuilder.RenameColumn(
                name: "ExportPrefix",
                table: "batch_job_configs",
                newName: "LocalExportPath");

            migrationBuilder.RenameColumn(
                name: "ErrorPrefix",
                table: "batch_job_configs",
                newName: "LocalErrorPath");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LocalProcessedPath",
                table: "batch_job_configs",
                newName: "ProcessedPrefix");

            migrationBuilder.RenameColumn(
                name: "LocalInboxPath",
                table: "batch_job_configs",
                newName: "InboxPrefix");

            migrationBuilder.RenameColumn(
                name: "LocalExportPath",
                table: "batch_job_configs",
                newName: "ExportPrefix");

            migrationBuilder.RenameColumn(
                name: "LocalErrorPath",
                table: "batch_job_configs",
                newName: "ErrorPrefix");

            migrationBuilder.AddColumn<string>(
                name: "ContainerName",
                table: "batch_job_configs",
                type: "character varying(200)",
                maxLength: 200,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "StorageType",
                table: "batch_job_configs",
                type: "character varying(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "");
        }
    }
}
