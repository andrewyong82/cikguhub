using Microsoft.EntityFrameworkCore.Migrations;

namespace CikguHub.Data.Migrations
{
    public partial class course : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Type",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "ChatChannel",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Content",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ImageResourceId",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Tags",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VideoUrl",
                table: "Courses",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "Deleted",
                table: "Classes",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "e21d03b2-0f2f-4666-ba90-659683c78f20");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "b9a6b9f1-ae01-47f8-a8f3-f10237f7d9a7");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "e041642f-4848-4be6-90c6-3250edcfbfbe", "AQAAAAEAACcQAAAAEPcP5TPLB2ZXechiYds7Ql6Eqk9b9wSTEv4eUvC0tgtFLbUCXe0Ef/Wliao4A8EWoA==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "03bcd478-0343-44c8-82ee-0d36d4a63407", "AQAAAAEAACcQAAAAEPRY9H6+IcRksO4uxRY+X/yxX4EOtLlA6Q8vxPU5x4R8Mhj+eN/0tJxEOidFFH3Few==" });

            migrationBuilder.CreateIndex(
                name: "IX_Courses_ImageResourceId",
                table: "Courses",
                column: "ImageResourceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Resources_ImageResourceId",
                table: "Courses",
                column: "ImageResourceId",
                principalTable: "Resources",
                principalColumn: "ResourceId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Resources_ImageResourceId",
                table: "Courses");

            migrationBuilder.DropIndex(
                name: "IX_Courses_ImageResourceId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ChatChannel",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Content",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ImageResourceId",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "VideoUrl",
                table: "Courses");

            migrationBuilder.DropColumn(
                name: "Deleted",
                table: "Classes");

            migrationBuilder.AddColumn<string>(
                name: "Type",
                table: "Courses",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 1,
                column: "ConcurrencyStamp",
                value: "7781df8e-27e2-4848-a942-4d50ac268900");

            migrationBuilder.UpdateData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: 2,
                column: "ConcurrencyStamp",
                value: "af6895ce-98fa-43df-8344-7960da795730");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "56372718-cb0a-4613-8a80-d3f6d1a4effa", "AQAAAAEAACcQAAAAEKSdYo0Ip4qq3Jbaa2lGdTYSOiRj695pewVFQAgnZMGt0083lpCURpIK2TgftLnH0Q==" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "ConcurrencyStamp", "PasswordHash" },
                values: new object[] { "a56bba2b-9ce6-4bb4-a7ad-67994ab1250e", "AQAAAAEAACcQAAAAEGaKvqEHgGsZgjqkrtalsnDG7zWfPEC4/SG7wJ60yvk8GmMSBfohv5PnMVRPlcFiCg==" });
        }
    }
}
