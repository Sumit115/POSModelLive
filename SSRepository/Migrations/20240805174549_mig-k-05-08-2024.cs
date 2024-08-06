using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSRepository.Migrations
{
    /// <inheritdoc />
    public partial class migk05082024 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.CreateTable(
                name: "tblPromotion_mas",
                schema: "dbo",
                columns: table => new
                {
                    PkPromotionId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PromotionDuring = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PromotionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PromotionFromDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromotionToDt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PromotionFromTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromotionToTime = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FKLocationId = table.Column<long>(type: "bigint", nullable: true),
                    FkVendorId = table.Column<long>(type: "bigint", nullable: true),
                    FkCustomerId = table.Column<long>(type: "bigint", nullable: true),
                    FkReferById = table.Column<long>(type: "bigint", nullable: true),
                    PromotionApplyOn = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Promotion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PromotionApplyAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PromotionApplyQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FkPromotionApplyUnitId = table.Column<long>(type: "bigint", nullable: true),
                    FKLotID = table.Column<long>(type: "bigint", nullable: true),
                    FkPromotionProdId = table.Column<long>(type: "bigint", nullable: true),
                    PromotionAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PromotionQty = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    FkPromotionUnitId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FKProdID = table.Column<long>(type: "bigint", nullable: true),
                    FkProdCatgId = table.Column<long>(type: "bigint", nullable: true),
                    FkBrandId = table.Column<long>(type: "bigint", nullable: true),
                    ModifiedDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FKCreatedByID = table.Column<long>(type: "bigint", nullable: false),
                    FKUserID = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblPromotion_mas", x => x.PkPromotionId);
                });

            
        }
    }
}
