using Microsoft.EntityFrameworkCore.Migrations;

namespace CikguHub.Data.Migrations
{
    public partial class customerid : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PriceId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProductId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionId",
                table: "Payments",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CustomerId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "a8cacee6-8cbe-421d-86ca-de7c0ec7873d");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b87fa7cc-bbfc-4f8c-811e-29a46244866e");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "d84a7a40-3f66-4322-9236-6fa3731a97dc", "AQAAAAEAACcQAAAAEH+lmkDAgxnkOW/cIdLaXo3mun8bYkM6Z/hPp+95bQD6RbhY2bdoCrTO+ZiB1STO7w==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9e696cad-2d6f-4b47-9edd-83ad56d6a0e1", "AQAAAAEAACcQAAAAEA8vSwBXMyD/vA3MqTn28BK0eACKME29tKzrIU0SJ2z2uAkgpcLK6Afs6T5SmdwNVQ==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "PriceId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "ProductId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "SubscriptionId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CustomerId",
                table: "AspNetUsers");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "f1bf5261-f3f8-401a-8b00-860b75e95456");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "69ef5b15-3032-4d25-8e33-ab4301dd9263");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "130e229f-fdaf-4eaa-9251-5fc26bc33ebb", "AQAAAAEAACcQAAAAEJPS9TBoas3ZFx321ZvZLWbLQ4LUSuIaKQOBLyxeC4N/iOB7TEZrMcMGmYDKhYS5ww==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e2ef7aed-37e5-43c4-a457-9b358117558c", "AQAAAAEAACcQAAAAEFu3YA7VsghmH91Zje2Bi1nH/CoKiaghcGe07GMwJLn01Xp3SXF3gLZAR6KHVdfrdg==" });
        }
    }
}
