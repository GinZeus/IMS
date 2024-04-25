using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRolesv11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ac7215cc-b3fb-41ab-a4d0-8a40798f55ee");

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "AspNetUsers",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "03482d4b-c825-4ae8-a1f1-26e98f3b8a5b", null, "Recruiter", "RECRUITER" },
                    { "1bfa4d50-b885-48b9-835a-f47ad854046b", null, "Admin", "ADMIN" },
                    { "2281c643-65a2-4fd6-83bc-ae240794b875", null, "Manager", "MANAGER" },
                    { "f1de4cb5-7f7c-4ba9-9453-22ec3840984b", null, "Interviewer", "INTERVIEWER" }
                });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DOB", "DepartmentId", "Email", "EmailConfirmed", "FullName", "Gender", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Note", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
                values: new object[] { "ac7215cc-b3fb-41ab-a4d0-8a40798f55ee", 0, "Viet Nam", "965b857f-509c-4508-9f57-d33602e0de51", null, 1, "root@gmail.com", true, "Ha Quang Thang", true, true, false, null, "ROOT@GMAIL.COM", "ROOT", null, "AQAAAAIAAYagAAAAEFIAshPAyXNlyLsBx6EiOFNI03BbZ2yWx5WbiHOxGdRjfsKWjxVwvJqmxfxRzdmUvg==", null, false, "448303c6-eacc-4616-9fe3-e51d4a8bffc1", false, "Root" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "03482d4b-c825-4ae8-a1f1-26e98f3b8a5b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "1bfa4d50-b885-48b9-835a-f47ad854046b");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "2281c643-65a2-4fd6-83bc-ae240794b875");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "f1de4cb5-7f7c-4ba9-9453-22ec3840984b");

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "ac7215cc-b3fb-41ab-a4d0-8a40798f55ee");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "AspNetUsers");

			migrationBuilder.InsertData(
				table: "AspNetUsers",
				columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DOB", "DepartmentId", "Email", "EmailConfirmed", "Gender", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Note", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
				values: new object[] { "ac7215cc-b3fb-41ab-a4d0-8a40798f55ee", 0, "Viet Nam", "965b857f-509c-4508-9f57-d33602e0de51", null, 1, "root@gmail.com", true, true, true, false, null, "ROOT@GMAIL.COM", "ROOT", null, "AQAAAAIAAYagAAAAEFIAshPAyXNlyLsBx6EiOFNI03BbZ2yWx5WbiHOxGdRjfsKWjxVwvJqmxfxRzdmUvg==", null, false, "448303c6-eacc-4616-9fe3-e51d4a8bffc1", false, "Root" });
		}
    }
}
