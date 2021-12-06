using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThesisERP.Infrastracture.Data.Migrations
{
    public partial class updatesCheck : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "109f7dbc-c7c5-4871-8283-078b5ef27020");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "274bbb30-07c5-42fb-9477-0b019373e67f");

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
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "LongDescription",
                table: "Products",
                type: "nvarchar(4000)",
                maxLength: 4000,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(200)",
                maxLength: 200,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "85c2e106-6953-47c6-8801-3f8bfc1d1812", "9b994165-0f95-4c63-bd0e-7b1b8da43819", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "fdabb83d-e219-444d-a473-d8ff83cd2bfc", "3afe6999-1483-4bbf-996f-b841d74a02de", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "LongDescription",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(4000)",
                oldMaxLength: 4000,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Products",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(200)",
                oldMaxLength: 200);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "109f7dbc-c7c5-4871-8283-078b5ef27020", "56d9e7c8-feb7-4bd1-8e06-b3d2ab4cbbf6", "User", "USER" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "274bbb30-07c5-42fb-9477-0b019373e67f", "8b0ce168-0f9f-4113-a55d-4745af0953b4", "Administrator", "ADMINISTRATOR" });
        }
    }
}
