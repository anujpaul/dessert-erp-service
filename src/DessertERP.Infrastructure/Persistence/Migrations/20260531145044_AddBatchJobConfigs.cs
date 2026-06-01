using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddBatchJobConfigs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "batch_job_configs",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    JobType = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    IsEnabled = table.Column<bool>(type: "boolean", nullable: false),
                    CronExpression = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    StorageType = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: false),
                    ContainerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    InboxPrefix = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ProcessedPrefix = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    ErrorPrefix = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false),
                    FileFormat = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: false),
                    ExportPrefix = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    ExportFileNamePattern = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    AutoConfirmSalesOrders = table.Column<bool>(type: "boolean", nullable: false),
                    LastRunAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    LastRunStatus = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LastRunMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    LastRunFilesProcessed = table.Column<int>(type: "integer", nullable: false),
                    LastRunRowsPromoted = table.Column<int>(type: "integer", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_batch_job_configs", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_batch_job_configs_OrganizationId",
                table: "batch_job_configs",
                column: "OrganizationId");

            migrationBuilder.CreateIndex(
                name: "IX_batch_job_configs_OrganizationId_IsEnabled",
                table: "batch_job_configs",
                columns: new[] { "OrganizationId", "IsEnabled" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "batch_job_configs");
        }
    }
}
