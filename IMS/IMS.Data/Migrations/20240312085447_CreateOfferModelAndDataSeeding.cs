using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMS.Data.Migrations
{
	/// <inheritdoc />
	public partial class CreateOfferModelAndDataSeeding : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.AlterColumn<byte[]>(
				name: "CVAttachment",
				table: "Candidates",
				type: "varbinary(max)",
				nullable: true,
				oldClrType: typeof(byte[]),
				oldType: "varbinary(max)");

			migrationBuilder.CreateTable(
				name: "InterviewAssigns",
				columns: table => new
				{
					InterviewAssignId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					InterviewScheduleId = table.Column<int>(type: "int", nullable: false),
					InterviewerId = table.Column<string>(type: "nvarchar(450)", nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_InterviewAssigns", x => x.InterviewAssignId);
					table.ForeignKey(
						name: "FK_InterviewAssigns_AspNetUsers_InterviewerId",
						column: x => x.InterviewerId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_InterviewAssigns_InterviewSchedules_InterviewScheduleId",
						column: x => x.InterviewScheduleId,
						principalTable: "InterviewSchedules",
						principalColumn: "InterviewScheduleId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateTable(
				name: "Offers",
				columns: table => new
				{
					OfferId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					CandidateId = table.Column<int>(type: "int", nullable: false),
					RecruiterOwnerId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					ApproverId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					InterviewId = table.Column<int>(type: "int", nullable: false),
					InterviewScheduleId = table.Column<int>(type: "int", nullable: false),
					ContractTypeID = table.Column<int>(type: "int", nullable: false),
					PositionId = table.Column<int>(type: "int", nullable: false),
					DepartmentId = table.Column<int>(type: "int", nullable: false),
					LevelId = table.Column<int>(type: "int", nullable: false),
					ContractFrom = table.Column<DateTime>(type: "datetime2", nullable: false),
					ContractTo = table.Column<DateTime>(type: "datetime2", nullable: false),
					DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					BasicSalary = table.Column<int>(type: "int", nullable: false),
					Notes = table.Column<string>(type: "nvarchar(max)", nullable: false),
					Status = table.Column<int>(type: "int", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_Offers", x => x.OfferId);
					table.ForeignKey(
						name: "FK_Offers_AspNetUsers_ApproverId",
						column: x => x.ApproverId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Offers_AspNetUsers_RecruiterOwnerId",
						column: x => x.RecruiterOwnerId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Offers_Candidates_CandidateId",
						column: x => x.CandidateId,
						principalTable: "Candidates",
						principalColumn: "CandidateId",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Offers_ContractTypes_ContractTypeID",
						column: x => x.ContractTypeID,
						principalTable: "ContractTypes",
						principalColumn: "ContractTypeId",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Offers_Department_DepartmentId",
						column: x => x.DepartmentId,
						principalTable: "Department",
						principalColumn: "DepartmentId",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Offers_InterviewSchedules_InterviewScheduleId",
						column: x => x.InterviewScheduleId,
						principalTable: "InterviewSchedules",
						principalColumn: "InterviewScheduleId",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Offers_Levels_LevelId",
						column: x => x.LevelId,
						principalTable: "Levels",
						principalColumn: "LevelId",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_Offers_Positions_PositionId",
						column: x => x.PositionId,
						principalTable: "Positions",
						principalColumn: "PositionId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.InsertData(
				table: "AspNetUsers",
				columns: new[] { "Id", "AccessFailedCount", "Address", "ConcurrencyStamp", "DOB", "DepartmentId", "Email", "EmailConfirmed", "FullName", "Gender", "IsActive", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "Note", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UserName" },
				values: new object[,]
				{
					{ "a92ee657-1957-4e26-908e-963bd999ca08", 0, "USA", "b5393e82-423a-4582-b316-991b19470be0", null, 1, "hung@gmail.com", true, "John Doe", true, true, false, null, "HUNG@GMAIL.COM", "HUNG@GMAIL.COM", null, "AQAAAAIAAYagAAAAEHe0f4CQ9BGCMxhkGWUtaIht9qw3fPfwbDszYesEMsqp2J/RAd8IM6e9PAL4DVwDVg==", null, false, "e8a3f408-0be8-40c9-9eab-4514e8a0a696", false, "hung@gmail.com" }
				});

			migrationBuilder.InsertData(
				table: "Job",
				columns: new[] { "JobId", "CreatedOn", "Description", "EndDate", "LastUpdatedBy", "LastUpdatedOn", "SalaryFrom", "SalaryTo", "StartDate", "Status", "Title", "WorkingAddress" },
				values: new object[] { 1, new DateTime(2024, 3, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(4977), "Develop and maintain software applications.", new DateTime(2025, 3, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(4974), "hung@gmail.com", new DateTime(2024, 3, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(4977), 50000, 75000, new DateTime(2024, 4, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(4954), 0, "Software Engineer", "Main Office" });

			migrationBuilder.InsertData(
				table: "Candidates",
				columns: new[] { "CandidateId", "AcademicLevelId", "Address", "CVAttachment", "CreatedOn", "DOB", "Email", "FullName", "Gender", "LastUpdatedBy", "LastUpdatedOn", "Note", "PhoneNumber", "PositionId", "RecruiterId", "Status", "YearOfExp" },
				values: new object[] { 1, 1, null, null, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "john.doe@example.com", "John Doe", 0, "hung@gmail.com", new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, 0, 1, "a92ee657-1957-4e26-908e-963bd999ca08", 0, 1 });

			migrationBuilder.InsertData(
				table: "InterviewSchedules",
				columns: new[] { "InterviewScheduleId", "CandidateId", "CreatedOn", "DueDate", "EndTime", "JobId", "LastUpdatedBy", "LastUpdatedOn", "Location", "MeetingId", "Notes", "RecruiterId", "Result", "StartTime", "Status", "Title" },
				values: new object[] { 1, 1, new DateTime(2024, 3, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(5364), new DateTime(2024, 3, 19, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(5342), null, 1, "System", new DateTime(2024, 3, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(5365), null, null, null, "a92ee657-1957-4e26-908e-963bd999ca08", null, null, 0, "Software Engineer Interview" });

			migrationBuilder.CreateIndex(
				name: "IX_InterviewAssigns_InterviewerId",
				table: "InterviewAssigns",
				column: "InterviewerId");

			migrationBuilder.CreateIndex(
				name: "IX_InterviewAssigns_InterviewScheduleId",
				table: "InterviewAssigns",
				column: "InterviewScheduleId");

			migrationBuilder.CreateIndex(
				name: "IX_Offers_ApproverId",
				table: "Offers",
				column: "ApproverId");

			migrationBuilder.CreateIndex(
				name: "IX_Offers_CandidateId",
				table: "Offers",
				column: "CandidateId");

			migrationBuilder.CreateIndex(
				name: "IX_Offers_ContractTypeID",
				table: "Offers",
				column: "ContractTypeID");

			migrationBuilder.CreateIndex(
				name: "IX_Offers_DepartmentId",
				table: "Offers",
				column: "DepartmentId");

			migrationBuilder.CreateIndex(
				name: "IX_Offers_InterviewScheduleId",
				table: "Offers",
				column: "InterviewScheduleId");

			migrationBuilder.CreateIndex(
				name: "IX_Offers_LevelId",
				table: "Offers",
				column: "LevelId");

			migrationBuilder.CreateIndex(
				name: "IX_Offers_PositionId",
				table: "Offers",
				column: "PositionId");

			migrationBuilder.CreateIndex(
				name: "IX_Offers_RecruiterOwnerId",
				table: "Offers",
				column: "RecruiterOwnerId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropTable(
				name: "InterviewAssigns");

			migrationBuilder.DropTable(
				name: "Offers");

			migrationBuilder.DeleteData(
				table: "InterviewSchedules",
				keyColumn: "InterviewScheduleId",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "Candidates",
				keyColumn: "CandidateId",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "Job",
				keyColumn: "JobId",
				keyValue: 1);

			migrationBuilder.DeleteData(
				table: "AspNetUsers",
				keyColumn: "Id",
				keyValue: "a92ee657-1957-4e26-908e-963bd999ca08");

			migrationBuilder.AlterColumn<byte[]>(
				name: "CVAttachment",
				table: "Candidates",
				type: "varbinary(max)",
				nullable: false,
				defaultValue: new byte[0],
				oldClrType: typeof(byte[]),
				oldType: "varbinary(max)",
				oldNullable: true);
		}
	}
}
