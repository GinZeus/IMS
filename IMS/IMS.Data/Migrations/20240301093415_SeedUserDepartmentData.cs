using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMS.Data.Migrations
{
	/// <inheritdoc />
	public partial class SeedUserDepartmentData : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.InsertData(
				table: "AcademicLevel",
				columns: new[] { "AcademicLevelId", "AcademicLevelName" },
				values: new object[,]
				{
					{ 1, "High school" },
					{ 2, "Bachelor's Degree" },
					{ 3, "Master Degree" },
					{ 4, "PhD" }
				});

			migrationBuilder.InsertData(
				table: "Benefit",
				columns: new[] { "BenefitId", "BenefitName" },
				values: new object[,]
				{
					{ 1, "Lunch" },
					{ 2, "25-day leave" },
					{ 3, "Healthcare insurance" },
					{ 4, "Hybrid working" },
					{ 5, "Travel" }
				});

			migrationBuilder.InsertData(
				table: "Department",
				columns: new[] { "DepartmentId", "DepartmentName" },
				values: new object[,]
				{
					{ 1, "IT" },
					{ 2, "HR" },
					{ 3, "Finance" },
					{ 4, "Communication" },
					{ 5, "Marketing" },
					{ 6, "Accounting" }
				});

			migrationBuilder.InsertData(
				table: "Levels",
				columns: new[] { "LevelId", "LevelName" },
				values: new object[,]
				{
					{ 1, "Fresher" },
					{ 2, "Junior" },
					{ 3, "Senior" },
					{ 4, "Leader" },
					{ 5, "Manager" },
					{ 6, "Vice Head" }
				});

			migrationBuilder.InsertData(
				table: "Positions",
				columns: new[] { "PositionId", "PositionName" },
				values: new object[,]
				{
					{ 1, "Backend Developer" },
					{ 2, "Business Analyst" },
					{ 3, "Tester" },
					{ 4, "HR" },
					{ 5, "Project Manager" },
					{ 6, "Not available" }
				});

			migrationBuilder.InsertData(
				table: "Skill",
				columns: new[] { "SkillId", "SkillName" },
				values: new object[,]
				{
					{ 1, "Java" },
					{ 2, "NodeJS" },
					{ 3, ".NET" },
					{ 4, "C++" },
					{ 5, "Business Analyst" },
					{ 6, "Communication" }
				});

			migrationBuilder.InsertData(
				table: "AspNetUsers",
				columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DOB", "DepartmentId", "Email", "EmailConfirmed", "Gender", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Note", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
				values: new object[] { "ac7215cc-b3fb-41ab-a4d0-8a40798f55ee", 0, "Viet Nam", "965b857f-509c-4508-9f57-d33602e0de51", null, 1, "root@gmail.com", true, true, true, false, null, "ROOT@GMAIL.COM", "ROOT", null, "AQAAAAIAAYagAAAAEFIAshPAyXNlyLsBx6EiOFNI03BbZ2yWx5WbiHOxGdRjfsKWjxVwvJqmxfxRzdmUvg==", null, false, "448303c6-eacc-4616-9fe3-e51d4a8bffc1", false, "Root" });
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DeleteData(
				table: "AcademicLevel",
				keyColumn: "AcademicLevelId",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "AcademicLevel",
				keyColumn: "AcademicLevelId",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "AcademicLevel",
				keyColumn: "AcademicLevelId",
				keyValue: 3);

			migrationBuilder.DeleteData(
				table: "AcademicLevel",
				keyColumn: "AcademicLevelId",
				keyValue: 4);

			migrationBuilder.DeleteData(
				table: "AspNetUsers",
				keyColumn: "Id",
				keyValue: "ac7215cc-b3fb-41ab-a4d0-8a40798f55ee");

			migrationBuilder.DeleteData(
				table: "Benefit",
				keyColumn: "BenefitId",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "Benefit",
				keyColumn: "BenefitId",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "Benefit",
				keyColumn: "BenefitId",
				keyValue: 3);

			migrationBuilder.DeleteData(
				table: "Benefit",
				keyColumn: "BenefitId",
				keyValue: 4);

			migrationBuilder.DeleteData(
				table: "Benefit",
				keyColumn: "BenefitId",
				keyValue: 5);

			migrationBuilder.DeleteData(
				table: "Department",
				keyColumn: "DepartmentId",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "Department",
				keyColumn: "DepartmentId",
				keyValue: 3);

			migrationBuilder.DeleteData(
				table: "Department",
				keyColumn: "DepartmentId",
				keyValue: 4);

			migrationBuilder.DeleteData(
				table: "Department",
				keyColumn: "DepartmentId",
				keyValue: 5);

			migrationBuilder.DeleteData(
				table: "Department",
				keyColumn: "DepartmentId",
				keyValue: 6);

			migrationBuilder.DeleteData(
				table: "Levels",
				keyColumn: "LevelId",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "Levels",
				keyColumn: "LevelId",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "Levels",
				keyColumn: "LevelId",
				keyValue: 3);

			migrationBuilder.DeleteData(
				table: "Levels",
				keyColumn: "LevelId",
				keyValue: 4);

			migrationBuilder.DeleteData(
				table: "Levels",
				keyColumn: "LevelId",
				keyValue: 5);

			migrationBuilder.DeleteData(
				table: "Levels",
				keyColumn: "LevelId",
				keyValue: 6);

			migrationBuilder.DeleteData(
				table: "Positions",
				keyColumn: "PositionId",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "Positions",
				keyColumn: "PositionId",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "Positions",
				keyColumn: "PositionId",
				keyValue: 3);

			migrationBuilder.DeleteData(
				table: "Positions",
				keyColumn: "PositionId",
				keyValue: 4);

			migrationBuilder.DeleteData(
				table: "Positions",
				keyColumn: "PositionId",
				keyValue: 5);

			migrationBuilder.DeleteData(
				table: "Positions",
				keyColumn: "PositionId",
				keyValue: 6);

			migrationBuilder.DeleteData(
				table: "Skill",
				keyColumn: "SkillId",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "Skill",
				keyColumn: "SkillId",
				keyValue: 2);

			migrationBuilder.DeleteData(
				table: "Skill",
				keyColumn: "SkillId",
				keyValue: 3);

			migrationBuilder.DeleteData(
				table: "Skill",
				keyColumn: "SkillId",
				keyValue: 4);

			migrationBuilder.DeleteData(
				table: "Skill",
				keyColumn: "SkillId",
				keyValue: 5);

			migrationBuilder.DeleteData(
				table: "Skill",
				keyColumn: "SkillId",
				keyValue: 6);

			migrationBuilder.DeleteData(
				table: "Department",
				keyColumn: "DepartmentId",
				keyValue: 1);
		}
	}
}
