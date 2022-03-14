using Microsoft.EntityFrameworkCore.Migrations;

namespace CikguHub.Data.Migrations
{
    public partial class enroladjust : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Classes_ClassId",
                table: "Enrolments");

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "Enrolments",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Enrolments",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "820fec7c-dd7c-4024-8820-0dc6134f1008");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "c6552ef9-a753-4ec3-94d2-c25213ea3073");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "c0644fcf-ed13-42aa-8e16-16f53179fa43", "AQAAAAEAACcQAAAAEGRsPRDSHiXwCudQTNhBv+2gv030ptYotfAWC7Ga1kI4eLMgpu89Ygqe+ikMeOLO0Q==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "9b0bac1b-03e0-4c5e-a895-2d227770aa2b", "AQAAAAEAACcQAAAAED4kRGzL3oV6GkV22+fMKrBIwJQKQJXNpy+Jf4c5gbwaEiAdx01/ACOkn9TZ7czApA==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Classes_ClassId",
                table: "Enrolments",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Enrolments_Classes_ClassId",
                table: "Enrolments");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Enrolments");

            migrationBuilder.AlterColumn<int>(
                name: "ClassId",
                table: "Enrolments",
                type: "int",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "5311e853-73f6-41d9-805f-6ed37b67a3c6");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "3a104f53-1371-4224-a9ff-e6ec2cdfa3b4");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "3a9b028c-4e4e-4837-bcee-b23b4ff47e71", "AQAAAAEAACcQAAAAEA8STdaag7AqzspDMPiWN2yGOxCPG8B5XBxH0WyDpWfkfb41MOiwCH6eXcvO/pmirA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "2fdcf757-cb67-4ff2-86a4-b636c387a60d", "AQAAAAEAACcQAAAAEArMxiaoSC+wjs112UtzkcGW93JCvaSZBfuttG2WbTDb8/y4xQSoBgTL50NqVyDw2g==" });

            migrationBuilder.AddForeignKey(
                name: "FK_Enrolments_Classes_ClassId",
                table: "Enrolments",
                column: "ClassId",
                principalTable: "Classes",
                principalColumn: "ClassId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
