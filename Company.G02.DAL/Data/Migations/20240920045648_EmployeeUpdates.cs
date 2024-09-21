using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Company.G02.DAL.Data.Migations
{
    /// <inheritdoc />
    public partial class EmployeeUpdates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfCreation",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GETDate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GetDate()");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfCreation",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValueSql: "GetDate()",
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValueSql: "GETDate()");
        }
    }
}
