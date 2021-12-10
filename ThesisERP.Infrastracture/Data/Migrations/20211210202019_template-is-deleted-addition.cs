using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ThesisERP.Infrastracture.Data.Migrations
{
    public partial class templateisdeletedaddition : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "66ac016c-6691-400e-a495-68875bbafa59");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f5751243-b021-4d51-8e59-4396e9efb128");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "TransactionTemplates",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "36646064-b28e-466c-80e8-a8ab6a919712", "6bf21fb9-9f61-40d0-9cc2-461c3f51a9a0", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "e788421d-5a8a-44ea-91cf-2a45985aaa01", "97b5d860-4df6-459f-9df4-92c092dc97b4", "User", "USER" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "36646064-b28e-466c-80e8-a8ab6a919712");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e788421d-5a8a-44ea-91cf-2a45985aaa01");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "TransactionTemplates");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "66ac016c-6691-400e-a495-68875bbafa59", "daf03a8d-dad1-4ec0-a307-9435ef256654", "Administrator", "ADMINISTRATOR" });

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[] { "f5751243-b021-4d51-8e59-4396e9efb128", "0fe7f195-a39f-43a4-9314-8d57c83ca130", "User", "USER" });
        }
    }
}
