using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThesisERP.Infrastracture.Data.Migrations
{
    public partial class minorChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21762d86-7080-4030-8d01-dbd21d7c5c40");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "8916c788-cfa8-4399-b79c-9256c96feac2");

            migrationBuilder.AddColumn<string>(
                name: "Comments",
                table: "Documents",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "66ac016c-6691-400e-a495-68875bbafa59", "daf03a8d-dad1-4ec0-a307-9435ef256654", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f5751243-b021-4d51-8e59-4396e9efb128", "0fe7f195-a39f-43a4-9314-8d57c83ca130", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "66ac016c-6691-400e-a495-68875bbafa59");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5751243-b021-4d51-8e59-4396e9efb128");

            migrationBuilder.DropColumn(
                name: "Comments",
                table: "Documents");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "21762d86-7080-4030-8d01-dbd21d7c5c40", "dbfecb05-0be7-416d-8849-038e01bd7bbd", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "8916c788-cfa8-4399-b79c-9256c96feac2", "de2bf9e5-1e6d-432d-90d3-de84df7ceb53", "User", "USER" });
        }
    }
}
