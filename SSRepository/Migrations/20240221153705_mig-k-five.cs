using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSRepository.Migrations
{
    /// <inheritdoc />
    public partial class migkfive : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "tblProdLot_dtl",
                schema: "dbo",
                columns: table => new
                {
                    PkLotId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKProdID = table.Column<long>(type: "bigint", nullable: false),
                    LotAlias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<long>(type: "bigint", nullable: true),
                    Batch = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Color = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MfgDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpiryDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ProdConv1 = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    MRP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LtExtra = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AddLT = table.Column<bool>(type: "bit", nullable: false),
                    SaleRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PurchaseRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FkmfgGroupId = table.Column<long>(type: "bigint", nullable: true),
                    TradeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    DistributionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PurchaseRateUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MRPSaleRateUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    InTrnId = table.Column<long>(type: "bigint", nullable: false),
                    InTrnFKSeriesID = table.Column<long>(type: "bigint", nullable: false),
                    InTrnsno = table.Column<long>(type: "bigint", nullable: false),
                    Remarks = table.Column<string>(type: "nvarchar(max)", nullable: true),
                     DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProdLot_dtl", x => x.PkLotId);
                });

            migrationBuilder.CreateTable(
                name: "tblProdStock_dtl",
                schema: "dbo",
                columns: table => new
                {
                    PkStockId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKProdID = table.Column<long>(type: "bigint", nullable: false),
                    FKLocationID = table.Column<long>(type: "bigint", nullable: false),
                    FKLotID = table.Column<long>(type: "bigint", nullable: false),
                    OpStock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    InStock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OutStock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CurStock = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                     DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProdStock_dtl", x => x.PkStockId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblProdLot_dtl",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblProdStock_dtl",
                schema: "dbo");
        }
    }
}
