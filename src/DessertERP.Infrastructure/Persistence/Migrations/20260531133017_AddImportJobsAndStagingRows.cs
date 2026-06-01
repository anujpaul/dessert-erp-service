using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddImportJobsAndStagingRows : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "import_job_rows",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ImportJobId = table.Column<Guid>(type: "uuid", nullable: false),
                    RowNumber = table.Column<int>(type: "integer", nullable: false),
                    RawPayload = table.Column<string>(type: "text", nullable: false),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    ErrorMessage = table.Column<string>(type: "character varying(2000)", maxLength: 2000, nullable: true),
                    PromotedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    PromotedEntityId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_import_job_rows", x => x.Id);
                    table.ForeignKey(
                        name: "FK_import_job_rows_import_jobs_ImportJobId",
                        column: x => x.ImportJobId,
                        principalTable: "import_jobs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_import_job_rows_ImportJobId",
                table: "import_job_rows",
                column: "ImportJobId");

            migrationBuilder.CreateIndex(
                name: "IX_import_job_rows_ImportJobId_Status",
                table: "import_job_rows",
                columns: new[] { "ImportJobId", "Status" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "import_job_rows");
        }
    }
}
