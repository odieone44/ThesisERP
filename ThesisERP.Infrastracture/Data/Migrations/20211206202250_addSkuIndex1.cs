using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThesisERP.Infrastracture.Data.Migrations
{
    public partial class addSkuIndex1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "85c2e106-6953-47c6-8801-3f8bfc1d1812");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "fdabb83d-e219-444d-a473-d8ff83cd2bfc");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Products",
                type: "int",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldDefaultValue: 0);

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "Products",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "21762d86-7080-4030-8d01-dbd21d7c5c40", "dbfecb05-0be7-416d-8849-038e01bd7bbd", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8916c788-cfa8-4399-b79c-9256c96feac2", "de2bf9e5-1e6d-432d-90d3-de84df7ceb53", "User", "USER" });

            migrationBuilder.CreateIndex(
                name: "IX_Products_SKU",
                table: "Products",
                column: "SKU",
                unique: true)
                .Annotation("SqlServer:Include", new[] { "Id", "Description", "Type", "DefaultPurchasePrice", "DefaultSaleSPrice", "LongDescription", "DateCreated", "DateUpdated" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Products_SKU",
                table: "Products");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21762d86-7080-4030-8d01-dbd21d7c5c40");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8916c788-cfa8-4399-b79c-9256c96feac2");

            migrationBuilder.AlterColumn<int>(
                name: "Type",
                table: "Products",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<string>(
                name: "SKU",
                table: "Products",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(40)",
                oldMaxLength: 40);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "85c2e106-6953-47c6-8801-3f8bfc1d1812", "9b994165-0f95-4c63-bd0e-7b1b8da43819", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fdabb83d-e219-444d-a473-d8ff83cd2bfc", "3afe6999-1483-4bbf-996f-b841d74a02de", "User", "USER" });
        }
    }
}
