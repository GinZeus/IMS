using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMS.Data.Migrations
{
    /// <inheritdoc />
    public partial class SeedContractType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "ContractTypes",
                columns: new[] { "ContractTypeId", "ContractTypeTitle" },
                values: new object[,]
                {
                    { 1, "Trial 2 months" },
                    { 2, "Trainee 3 months" },
                    { 3, "1 year" },
                    { 4, "3 years and Unlimited" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "ContractTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "ContractTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "ContractTypeId",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "ContractTypes",
                keyColumn: "ContractTypeId",
                keyValue: 4);
        }
    }
}
