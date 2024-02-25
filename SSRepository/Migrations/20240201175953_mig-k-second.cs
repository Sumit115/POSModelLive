using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SSRepository.Migrations
{
    /// <inheritdoc />
    public partial class migksecond : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "dbo");

            migrationBuilder.CreateTable(
                name: "tblError_Log",
                schema: "dbo",
                columns: table => new
                {
                    PkId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LogId = table.Column<long>(type: "bigint", nullable: false),
                    ErrorMessage = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DateModified = table.Column<DateTime>(type: "datetime2", nullable: true),
                    DateCreated = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Src = table.Column<int>(type: "int", nullable: false),
                    FKUserId = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_tblError_Log", x => x.PkId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "tblError_Log",
                schema: "dbo");
        }
    }
}
