using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSRepository.Migrations
{
    /// <inheritdoc />
    public partial class NewMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "tblCustomer_mas",
                columns: table => new
                {
                    PkId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false),
                    FKCreatedByID = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
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
                    table.PrimaryKey("PK_tblCustomer_mas", x => x.PkId);
                });

            migrationBuilder.CreateTable(
                name: "tblUser_mas",
                columns: table => new
                {
                    PkId = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Pwd = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    FkRegId = table.Column<int>(type: "int", nullable: true),
                    Usertype = table.Column<int>(type: "int", nullable: true),
                    FkBranchId = table.Column<int>(type: "int", nullable: true),
                    FkRoleId = table.Column<int>(type: "int", nullable: true),
                    Expiredt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpirePwddt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    FkEmployeeId = table.Column<int>(type: "int", nullable: false),
                    IsAdmin = table.Column<int>(type: "int", nullable: false),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false),
                    FKCreatedByID = table.Column<long>(type: "bigint", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblUser_mas", x => x.PkId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblCustomer_mas");

            migrationBuilder.DropTable(
                name: "tblUser_mas");
        }
    }
}
