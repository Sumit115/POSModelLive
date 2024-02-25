using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSRepository.Migrations
{
    /// <inheritdoc />
    public partial class migkthird : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "tblSalesInvoice_dtl",
                schema: "dbo",
                columns: table => new
                {
                    PkId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkId = table.Column<long>(type: "bigint", nullable: false),
                    FKSeriesId = table.Column<long>(type: "bigint", nullable: false),
                    sno = table.Column<int>(type: "int", nullable: false),
                    FkProductId = table.Column<long>(type: "bigint", nullable: false),
                    FkLotId = table.Column<long>(type: "bigint", nullable: false),
                    Rate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RateUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Qty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    FreeQty = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SchemeDisc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SchemeDiscType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SchemeDiscAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TradeDisc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TradeDiscType = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeDiscAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    LotDisc = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    GrossAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ICRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ICAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SCRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SCAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSalesInvoice_dtl", x => x.PkId);
                });

            migrationBuilder.CreateTable(
                name: "tblSalesInvoice_trn",
                schema: "dbo",
                columns: table => new
                {
                    PkId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FKSeriesId = table.Column<long>(type: "bigint", nullable: false),
                    EntryNo = table.Column<long>(type: "bigint", nullable: false),
                    EntryDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TranAlias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FkPartyId = table.Column<int>(type: "int", nullable: false),
                    GRNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GRDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    GrossAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SgstAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TaxAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CashDiscType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CashDiscountAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TotalDiscount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RoundOfDiff = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Shipping = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    OtherCharge = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    NetAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Cash = table.Column<bool>(type: "bit", nullable: false),
                    CashAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Credit = table.Column<bool>(type: "bit", nullable: false),
                    CreditAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Cheque = table.Column<bool>(type: "bit", nullable: false),
                    ChequeAmt = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ChequeNo = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ChequeDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FKBankChequeID = table.Column<long>(type: "bigint", nullable: false),
                    Remark = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Statu = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSalesInvoice_trn", x => x.PkId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblSalesInvoice_dtl",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblSalesInvoice_trn",
                schema: "dbo");
        }
    }
}
