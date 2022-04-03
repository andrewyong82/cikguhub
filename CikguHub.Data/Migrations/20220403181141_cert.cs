using Microsoft.EntityFrameworkCore.Migrations;

namespace CikguHub.Data.Migrations
{
    public partial class cert : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CertificateUrl",
                table: "Enrolments",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "c63cb94b-8e65-4d87-89f9-af3a0a7acd29");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "d9287a98-9a3b-47e6-b55d-25c7b59a8230");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "c78160f8-0c80-4bf2-8b0a-38dd5ef7e77b", "admin@cikguhub.my", "ADMIN@CIKGUHUB.MY", "ADMIN@CIKGUHUB.MY", "AQAAAAEAACcQAAAAEFHwbRjFaHB+Gi49MZJT+DRos6tPPsXjcXgdrGMzB9CRumRF+jr1lPCWfqoYBdEjTg==", "admin@cikguhub.my" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "c815b00a-7aa8-4672-9ce6-2586784c1d0f", "member@cikguhub.my", "MEMBER@CIKGUHUB.MY", "MEMBER@CIKGUHUB.MY", "AQAAAAEAACcQAAAAEEcrYHnni5gpCAqmxWaHc4BvW8xGouFJgSIejlUfKcY62rZ5TJ225T3TN3GuQ0cEqA==", "member@cikguhub.my" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CertificateUrl",
                table: "Enrolments");

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
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "d84a7a40-3f66-4322-9236-6fa3731a97dc", "admin@fastLaw.my", "ADMIN@FASTLAW.MY", "ADMIN@FASTLAW.MY", "AQAAAAEAACcQAAAAEH+lmkDAgxnkOW/cIdLaXo3mun8bYkM6Z/hPp+95bQD6RbhY2bdoCrTO+ZiB1STO7w==", "admin@fastlaw.my" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "Email", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "UserName" },
                values: new object[] { "9e696cad-2d6f-4b47-9edd-83ad56d6a0e1", "member@fastLaw.my", "MEMBER@FASTLAW.MY", "MEMBER@FASTLAW.MY", "AQAAAAEAACcQAAAAEA8vSwBXMyD/vA3MqTn28BK0eACKME29tKzrIU0SJ2z2uAkgpcLK6Afs6T5SmdwNVQ==", "member@fastLaw.my" });
        }
    }
}
