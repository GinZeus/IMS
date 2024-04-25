using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoleForUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[,]
                {
                    { "03482d4b-c825-4ae8-a1f1-26e98f3b8a5b", "a92ee657-1957-4e26-908e-963bd999ca08" },
                    { "1bfa4d50-b885-48b9-835a-f47ad854046b", "ac7215cc-b3fb-41ab-a4d0-8a40798f55ee" }
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a92ee657-1957-4e26-908e-963bd999ca08",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "DOEJ1", "DoeJ1" });

            migrationBuilder.UpdateData(
                table: "Candidates",
                keyColumn: "CandidateId",
                keyValue: 1,
                column: "Status",
                value: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "03482d4b-c825-4ae8-a1f1-26e98f3b8a5b", "a92ee657-1957-4e26-908e-963bd999ca08" });

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { "1bfa4d50-b885-48b9-835a-f47ad854046b", "ac7215cc-b3fb-41ab-a4d0-8a40798f55ee" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: "a92ee657-1957-4e26-908e-963bd999ca08",
                columns: new[] { "NormalizedUserName", "UserName" },
                values: new object[] { "HUNG@GMAIL.COM", "hung@gmail.com" });

            migrationBuilder.UpdateData(
                table: "Candidates",
                keyColumn: "CandidateId",
                keyValue: 1,
                column: "Status",
                value: 1);
        }
    }
}
