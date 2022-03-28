using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CikguHub.Data.Migrations
{
    public partial class subscription : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Courses_CaseId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Subscriptions_AspNetUsers_UserId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CaseId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CaseId",
                table: "Payments");

            migrationBuilder.AddColumn<decimal>(
                name: "AmountPaid",
                table: "Subscriptions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTime>(
                name: "CancelDate",
                table: "Subscriptions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndDate",
                table: "Subscriptions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<bool>(
                name: "Recurring",
                table: "Subscriptions",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "Subscriptions",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "Status",
                table: "Subscriptions",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TimePeriod",
                table: "Subscriptions",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalCost",
                table: "Subscriptions",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "SubscriptionStatus",
                table: "AspNetUsers",
                nullable: true);

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
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SubscriptionStatus" },
                values: new object[] { "130e229f-fdaf-4eaa-9251-5fc26bc33ebb", "AQAAAAEAACcQAAAAEJPS9TBoas3ZFx321ZvZLWbLQ4LUSuIaKQOBLyxeC4N/iOB7TEZrMcMGmYDKhYS5ww==", "Inactive" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash", "SubscriptionStatus" },
                values: new object[] { "e2ef7aed-37e5-43c4-a457-9b358117558c", "AQAAAAEAACcQAAAAEFu3YA7VsghmH91Zje2Bi1nH/CoKiaghcGe07GMwJLn01Xp3SXF3gLZAR6KHVdfrdg==", "Inactive" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AmountPaid",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "CancelDate",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "EndDate",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Recurring",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "Status",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "TimePeriod",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "Subscriptions");

            migrationBuilder.DropColumn(
                name: "SubscriptionStatus",
                table: "AspNetUsers");

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Subscriptions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "CaseId",
                table: "Payments",
                type: "int",
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

            migrationBuilder.CreateIndex(
                name: "IX_Subscriptions_UserId",
                table: "Subscriptions",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CaseId",
                table: "Payments",
                column: "CaseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Courses_CaseId",
                table: "Payments",
                column: "CaseId",
                principalTable: "Courses",
                principalColumn: "CourseId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subscriptions_AspNetUsers_UserId",
                table: "Subscriptions",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
