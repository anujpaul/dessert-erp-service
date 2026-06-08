using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DessertERP.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddWarehouseManagement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Warehouses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Address = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    City = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Country = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsDefault = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Warehouses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "InboundOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    PurchaseOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    VendorId = table.Column<Guid>(type: "uuid", nullable: true),
                    VendorName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ExpectedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ReceivedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InboundOrders_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OutboundOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    SalesOrderId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerId = table.Column<Guid>(type: "uuid", nullable: true),
                    CustomerName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: true),
                    ShipToAddress = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    RequestedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ShippedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    TrackingNumber = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Carrier = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboundOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutboundOrders_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TransferOrders",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OrganizationId = table.Column<Guid>(type: "uuid", nullable: false),
                    OrderNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    FromWarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ToWarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    RequestedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    ShippedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    ReceivedDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOrders", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferOrders_Warehouses_FromWarehouseId",
                        column: x => x.FromWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TransferOrders_Warehouses_ToWarehouseId",
                        column: x => x.ToWarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WarehouseLocations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WarehouseId = table.Column<Guid>(type: "uuid", nullable: false),
                    Code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    Zone = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Aisle = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Bay = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Level = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    Bin = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    IsPickable = table.Column<bool>(type: "boolean", nullable: false),
                    IsReceivable = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WarehouseLocations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WarehouseLocations_Warehouses_WarehouseId",
                        column: x => x.WarehouseId,
                        principalTable: "Warehouses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "InboundOrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    InboundOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNumber = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProductSku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    LocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    OrderedQuantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    ReceivedQuantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LotNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InboundOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InboundOrderLines_InboundOrders_InboundOrderId",
                        column: x => x.InboundOrderId,
                        principalTable: "InboundOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_InboundOrderLines_WarehouseLocations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "OutboundOrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    OutboundOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNumber = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProductSku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FromLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestedQuantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    PickedQuantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    ShippedQuantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LotNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OutboundOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OutboundOrderLines_OutboundOrders_OutboundOrderId",
                        column: x => x.OutboundOrderId,
                        principalTable: "OutboundOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OutboundOrderLines_WarehouseLocations_FromLocationId",
                        column: x => x.FromLocationId,
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "TransferOrderLines",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TransferOrderId = table.Column<Guid>(type: "uuid", nullable: false),
                    LineNumber = table.Column<int>(type: "integer", nullable: false),
                    ProductId = table.Column<Guid>(type: "uuid", nullable: false),
                    ProductName = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    ProductSku = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    FromLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    ToLocationId = table.Column<Guid>(type: "uuid", nullable: true),
                    RequestedQuantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    ShippedQuantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    ReceivedQuantity = table.Column<decimal>(type: "numeric(18,4)", nullable: false),
                    UnitOfMeasure = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    LotNumber = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    Notes = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp without time zone", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransferOrderLines", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TransferOrderLines_TransferOrders_TransferOrderId",
                        column: x => x.TransferOrderId,
                        principalTable: "TransferOrders",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TransferOrderLines_WarehouseLocations_FromLocationId",
                        column: x => x.FromLocationId,
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_TransferOrderLines_WarehouseLocations_ToLocationId",
                        column: x => x.ToLocationId,
                        principalTable: "WarehouseLocations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateIndex(
                name: "IX_InboundOrderLines_InboundOrderId",
                table: "InboundOrderLines",
                column: "InboundOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_InboundOrderLines_LocationId",
                table: "InboundOrderLines",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_InboundOrders_OrganizationId_OrderNumber",
                table: "InboundOrders",
                columns: new[] { "OrganizationId", "OrderNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_InboundOrders_WarehouseId",
                table: "InboundOrders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundOrderLines_FromLocationId",
                table: "OutboundOrderLines",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundOrderLines_OutboundOrderId",
                table: "OutboundOrderLines",
                column: "OutboundOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_OutboundOrders_OrganizationId_OrderNumber",
                table: "OutboundOrders",
                columns: new[] { "OrganizationId", "OrderNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_OutboundOrders_WarehouseId",
                table: "OutboundOrders",
                column: "WarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderLines_FromLocationId",
                table: "TransferOrderLines",
                column: "FromLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderLines_ToLocationId",
                table: "TransferOrderLines",
                column: "ToLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrderLines_TransferOrderId",
                table: "TransferOrderLines",
                column: "TransferOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrders_FromWarehouseId",
                table: "TransferOrders",
                column: "FromWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrders_OrganizationId_OrderNumber",
                table: "TransferOrders",
                columns: new[] { "OrganizationId", "OrderNumber" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_TransferOrders_ToWarehouseId",
                table: "TransferOrders",
                column: "ToWarehouseId");

            migrationBuilder.CreateIndex(
                name: "IX_WarehouseLocations_WarehouseId_Code",
                table: "WarehouseLocations",
                columns: new[] { "WarehouseId", "Code" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Warehouses_OrganizationId_Code",
                table: "Warehouses",
                columns: new[] { "OrganizationId", "Code" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InboundOrderLines");

            migrationBuilder.DropTable(
                name: "OutboundOrderLines");

            migrationBuilder.DropTable(
                name: "TransferOrderLines");

            migrationBuilder.DropTable(
                name: "InboundOrders");

            migrationBuilder.DropTable(
                name: "OutboundOrders");

            migrationBuilder.DropTable(
                name: "TransferOrders");

            migrationBuilder.DropTable(
                name: "WarehouseLocations");

            migrationBuilder.DropTable(
                name: "Warehouses");
        }
    }
}
