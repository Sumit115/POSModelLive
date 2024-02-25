using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSRepository.Migrations
{
    /// <inheritdoc />
    public partial class migkfirst : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "tblBank_mas",
                schema: "dbo",
                columns: table => new
                {
                    PkBankId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BankName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBank_mas", x => x.PkBankId);
                });

            migrationBuilder.CreateTable(
                name: "tblBranch_mas",
                schema: "dbo",
                columns: table => new
                {
                    PkBranchId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BranchName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FkRegId = table.Column<long>(type: "bigint", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblBranch_mas", x => x.PkBranchId);
                });

            migrationBuilder.CreateTable(
                name: "tblCategory_mas",
                schema: "dbo",
                columns: table => new
                {
                    PkCategoryId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CategoryName = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    FkCategoryId = table.Column<long>(type: "bigint", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCategory_mas", x => x.PkCategoryId);
                });

            migrationBuilder.CreateTable(
                name: "tblCompany",
                schema: "dbo",
                columns: table => new
                {
                    PkCompanyId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CompanyName = table.Column<string>(type: "nvarchar(225)", maxLength: 225, nullable: false),
                    ContactPerson = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    City = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    State = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Pin = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Country = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gstn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LogoImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ThumbnailImg = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Connection = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCompany", x => x.PkCompanyId);
                });

            migrationBuilder.CreateTable(
                name: "tblCustomer_mas",
                schema: "dbo",
                columns: table => new
                {
                    PkCustomerId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Marital = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aadhar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Panno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gstno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharCardFront = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharCardBack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAadharVerify = table.Column<int>(type: "int", nullable: true),
                    IsPanVerify = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblCustomer_mas", x => x.PkCustomerId);
                });

            migrationBuilder.CreateTable(
                name: "tblEmployee_mas",
                schema: "dbo",
                columns: table => new
                {
                    PkEmployeeId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Marital = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aadhar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Panno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gstno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharCardFront = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharCardBack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAadharVerify = table.Column<int>(type: "int", nullable: true),
                    IsPanVerify = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblEmployee_mas", x => x.PkEmployeeId);
                });

            migrationBuilder.CreateTable(
                name: "tblForm_mas",
                schema: "dbo",
                columns: table => new
                {
                    PkFormId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Caption = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FkMasterFormId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblForm_mas", x => x.PkFormId);
                });

            migrationBuilder.CreateTable(
                name: "tblGridStructer",
                schema: "dbo",
                columns: table => new
                {
                    PkGridId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FkUserId = table.Column<long>(type: "bigint", nullable: false),
                    FkFormId = table.Column<long>(type: "bigint", nullable: false),
                    JsonData = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblGridStructer", x => x.PkGridId);
                });

            migrationBuilder.CreateTable(
                name: "tblProduct_mas",
                schema: "dbo",
                columns: table => new
                {
                    PkProductId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Product = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameToDisplay = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NameToPrint = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Image = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Alias = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Strength = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Barcode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(1)", nullable: false),
                    FkprodCatgId = table.Column<long>(type: "bigint", nullable: false),
                    FKTaxID = table.Column<long>(type: "bigint", nullable: false),
                    HSNCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ShelfID = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TradeDisc = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MinStock = table.Column<int>(type: "int", nullable: false),
                    MaxStock = table.Column<int>(type: "int", nullable: false),
                    MinDays = table.Column<int>(type: "int", nullable: false),
                    MaxDays = table.Column<int>(type: "int", nullable: false),
                    CaseLot = table.Column<int>(type: "int", nullable: false),
                    BoxSize = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Unit1 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProdConv1 = table.Column<int>(type: "int", nullable: false),
                    Unit2 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProdConv2 = table.Column<int>(type: "int", nullable: false),
                    Unit3 = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MRP = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SaleRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TradeRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DistributionRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchaseRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    KeepStock = table.Column<bool>(type: "bit", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblProduct_mas", x => x.PkProductId);
                });

            migrationBuilder.CreateTable(
                name: "tblSalesOrder_dtl",
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
                    table.PrimaryKey("PK_tblSalesOrder_dtl", x => x.PkId);
                });

            migrationBuilder.CreateTable(
                name: "tblSalesOrder_trn",
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
                    table.PrimaryKey("PK_tblSalesOrder_trn", x => x.PkId);
                });

            migrationBuilder.CreateTable(
                name: "tblSeries_mas",
                schema: "dbo",
                columns: table => new
                {
                    PkSeriesId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Series = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SeriesNo = table.Column<long>(type: "bigint", nullable: false),
                    FkBranchId = table.Column<long>(type: "bigint", nullable: false),
                    BillingRate = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TranAlias = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FormatName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetNoFor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AllowWalkIn = table.Column<bool>(type: "bit", nullable: false),
                    AutoApplyPromo = table.Column<bool>(type: "bit", nullable: false),
                    RoundOff = table.Column<bool>(type: "bit", nullable: false),
                    DefaultQty = table.Column<bool>(type: "bit", nullable: false),
                    AllowZeroRate = table.Column<bool>(type: "bit", nullable: false),
                    AllowFreeQty = table.Column<bool>(type: "bit", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblSeries_mas", x => x.PkSeriesId);
                });

            migrationBuilder.CreateTable(
                name: "tblUser_mas",
                schema: "dbo",
                columns: table => new
                {
                    PkUserId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pwd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FkRegId = table.Column<long>(type: "bigint", nullable: true),
                    Usertype = table.Column<int>(type: "int", nullable: true),
                    FkBranchId = table.Column<long>(type: "bigint", nullable: true),
                    FkRoleId = table.Column<long>(type: "bigint", nullable: true),
                    Expiredt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirePwddt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FkEmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    IsAdmin = table.Column<int>(type: "int", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUser_mas", x => x.PkUserId);
                });

            migrationBuilder.CreateTable(
                name: "tblVendor_mas",
                schema: "dbo",
                columns: table => new
                {
                    PkVendorId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    Name = table.Column<string>(type: "nvarchar(125)", maxLength: 125, nullable: false),
                    FatherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    MotherName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Marital = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Dob = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Mobile = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Aadhar = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Panno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Gstno = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Passport = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharCardFront = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AadharCardBack = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PanCard = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Signature = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsAadharVerify = table.Column<int>(type: "int", nullable: true),
                    IsPanVerify = table.Column<int>(type: "int", nullable: true),
                    Status = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblVendor_mas", x => x.PkVendorId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblBank_mas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblBranch_mas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblCategory_mas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblCompany",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblCustomer_mas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblEmployee_mas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblForm_mas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblGridStructer",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblProduct_mas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblSalesOrder_dtl",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblSalesOrder_trn",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblSeries_mas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblUser_mas",
                schema: "dbo");

            migrationBuilder.DropTable(
                name: "tblVendor_mas",
                schema: "dbo");
        }
    }
}
