using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMS.Data.Migrations
{
	/// <inheritdoc />
	public partial class AddInterviewSchedule : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AspNetUsers_Department_DepartmentId",
				table: "AspNetUsers");

			migrationBuilder.DropForeignKey(
				name: "FK_Candidates_AcademicLevel_AcademicLevelId",
				table: "Candidates");

			migrationBuilder.DropForeignKey(
				name: "FK_Candidates_AspNetUsers_RecruiterId",
				table: "Candidates");

			migrationBuilder.DropForeignKey(
				name: "FK_Candidates_Positions_PositionId",
				table: "Candidates");

			migrationBuilder.DropForeignKey(
				name: "FK_CandidateSkills_Candidates_CandidateId",
				table: "CandidateSkills");

			migrationBuilder.DropForeignKey(
				name: "FK_CandidateSkills_Skill_SkillId",
				table: "CandidateSkills");

			migrationBuilder.DropForeignKey(
				name: "FK_JobBenefits_Benefit_BenefitId",
				table: "JobBenefits");

			migrationBuilder.DropForeignKey(
				name: "FK_JobBenefits_Job_JobId",
				table: "JobBenefits");

			migrationBuilder.DropForeignKey(
				name: "FK_JobLevels_Job_JobId",
				table: "JobLevels");

			migrationBuilder.DropForeignKey(
				name: "FK_JobLevels_Levels_LevelId",
				table: "JobLevels");

			migrationBuilder.DropForeignKey(
				name: "FK_JobSkill_Job_JobId",
				table: "JobSkill");

			migrationBuilder.DropForeignKey(
				name: "FK_JobSkill_Skill_SkillId",
				table: "JobSkill");

			migrationBuilder.DropForeignKey(
				name: "FK_RecoveryTokens_AspNetUsers_UserID",
				table: "RecoveryTokens");

			migrationBuilder.CreateTable(
				name: "InterviewSchedules",
				columns: table => new
				{
					InterviewScheduleId = table.Column<int>(type: "int", nullable: false)
						.Annotation("SqlServer:Identity", "1, 1"),
					CandidateId = table.Column<int>(type: "int", nullable: false),
					JobId = table.Column<int>(type: "int", nullable: false),
					RecruiterId = table.Column<string>(type: "nvarchar(450)", nullable: false),
					Title = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
					Location = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: true),
					DueDate = table.Column<DateTime>(type: "datetime2", nullable: false),
					StartTime = table.Column<DateTime>(type: "datetime2", nullable: true),
					EndTime = table.Column<DateTime>(type: "datetime2", nullable: true),
					MeetingId = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Notes = table.Column<string>(type: "nvarchar(max)", nullable: true),
					Result = table.Column<bool>(type: "bit", nullable: true),
					Status = table.Column<int>(type: "int", nullable: false),
					CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
					LastUpdatedBy = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
				},
				constraints: table =>
				{
					table.PrimaryKey("PK_InterviewSchedules", x => x.InterviewScheduleId);
					table.ForeignKey(
						name: "FK_InterviewSchedules_AspNetUsers_RecruiterId",
						column: x => x.RecruiterId,
						principalTable: "AspNetUsers",
						principalColumn: "Id",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_InterviewSchedules_Candidates_CandidateId",
						column: x => x.CandidateId,
						principalTable: "Candidates",
						principalColumn: "CandidateId",
						onDelete: ReferentialAction.Restrict);
					table.ForeignKey(
						name: "FK_InterviewSchedules_Job_JobId",
						column: x => x.JobId,
						principalTable: "Job",
						principalColumn: "JobId",
						onDelete: ReferentialAction.Restrict);
				});

			migrationBuilder.CreateIndex(
				name: "IX_InterviewSchedules_CandidateId",
				table: "InterviewSchedules",
				column: "CandidateId");

			migrationBuilder.CreateIndex(
				name: "IX_InterviewSchedules_JobId",
				table: "InterviewSchedules",
				column: "JobId");

			migrationBuilder.CreateIndex(
				name: "IX_InterviewSchedules_RecruiterId",
				table: "InterviewSchedules",
				column: "RecruiterId");

			migrationBuilder.AddForeignKey(
				name: "FK_AspNetUsers_Department_DepartmentId",
				table: "AspNetUsers",
				column: "DepartmentId",
				principalTable: "Department",
				principalColumn: "DepartmentId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Candidates_AcademicLevel_AcademicLevelId",
				table: "Candidates",
				column: "AcademicLevelId",
				principalTable: "AcademicLevel",
				principalColumn: "AcademicLevelId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Candidates_AspNetUsers_RecruiterId",
				table: "Candidates",
				column: "RecruiterId",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_Candidates_Positions_PositionId",
				table: "Candidates",
				column: "PositionId",
				principalTable: "Positions",
				principalColumn: "PositionId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_CandidateSkills_Candidates_CandidateId",
				table: "CandidateSkills",
				column: "CandidateId",
				principalTable: "Candidates",
				principalColumn: "CandidateId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_CandidateSkills_Skill_SkillId",
				table: "CandidateSkills",
				column: "SkillId",
				principalTable: "Skill",
				principalColumn: "SkillId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_JobBenefits_Benefit_BenefitId",
				table: "JobBenefits",
				column: "BenefitId",
				principalTable: "Benefit",
				principalColumn: "BenefitId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_JobBenefits_Job_JobId",
				table: "JobBenefits",
				column: "JobId",
				principalTable: "Job",
				principalColumn: "JobId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_JobLevels_Job_JobId",
				table: "JobLevels",
				column: "JobId",
				principalTable: "Job",
				principalColumn: "JobId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_JobLevels_Levels_LevelId",
				table: "JobLevels",
				column: "LevelId",
				principalTable: "Levels",
				principalColumn: "LevelId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_JobSkill_Job_JobId",
				table: "JobSkill",
				column: "JobId",
				principalTable: "Job",
				principalColumn: "JobId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_JobSkill_Skill_SkillId",
				table: "JobSkill",
				column: "SkillId",
				principalTable: "Skill",
				principalColumn: "SkillId",
				onDelete: ReferentialAction.Restrict);

			migrationBuilder.AddForeignKey(
				name: "FK_RecoveryTokens_AspNetUsers_UserID",
				table: "RecoveryTokens",
				column: "UserID",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Restrict);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_AspNetUsers_Department_DepartmentId",
				table: "AspNetUsers");

			migrationBuilder.DropForeignKey(
				name: "FK_Candidates_AcademicLevel_AcademicLevelId",
				table: "Candidates");

			migrationBuilder.DropForeignKey(
				name: "FK_Candidates_AspNetUsers_RecruiterId",
				table: "Candidates");

			migrationBuilder.DropForeignKey(
				name: "FK_Candidates_Positions_PositionId",
				table: "Candidates");

			migrationBuilder.DropForeignKey(
				name: "FK_CandidateSkills_Candidates_CandidateId",
				table: "CandidateSkills");

			migrationBuilder.DropForeignKey(
				name: "FK_CandidateSkills_Skill_SkillId",
				table: "CandidateSkills");

			migrationBuilder.DropForeignKey(
				name: "FK_JobBenefits_Benefit_BenefitId",
				table: "JobBenefits");

			migrationBuilder.DropForeignKey(
				name: "FK_JobBenefits_Job_JobId",
				table: "JobBenefits");

			migrationBuilder.DropForeignKey(
				name: "FK_JobLevels_Job_JobId",
				table: "JobLevels");

			migrationBuilder.DropForeignKey(
				name: "FK_JobLevels_Levels_LevelId",
				table: "JobLevels");

			migrationBuilder.DropForeignKey(
				name: "FK_JobSkill_Job_JobId",
				table: "JobSkill");

			migrationBuilder.DropForeignKey(
				name: "FK_JobSkill_Skill_SkillId",
				table: "JobSkill");

			migrationBuilder.DropForeignKey(
				name: "FK_RecoveryTokens_AspNetUsers_UserID",
				table: "RecoveryTokens");

			migrationBuilder.DropTable(
				name: "InterviewSchedules");

			migrationBuilder.AddForeignKey(
				name: "FK_AspNetUsers_Department_DepartmentId",
				table: "AspNetUsers",
				column: "DepartmentId",
				principalTable: "Department",
				principalColumn: "DepartmentId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Candidates_AcademicLevel_AcademicLevelId",
				table: "Candidates",
				column: "AcademicLevelId",
				principalTable: "AcademicLevel",
				principalColumn: "AcademicLevelId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Candidates_AspNetUsers_RecruiterId",
				table: "Candidates",
				column: "RecruiterId",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_Candidates_Positions_PositionId",
				table: "Candidates",
				column: "PositionId",
				principalTable: "Positions",
				principalColumn: "PositionId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_CandidateSkills_Candidates_CandidateId",
				table: "CandidateSkills",
				column: "CandidateId",
				principalTable: "Candidates",
				principalColumn: "CandidateId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_CandidateSkills_Skill_SkillId",
				table: "CandidateSkills",
				column: "SkillId",
				principalTable: "Skill",
				principalColumn: "SkillId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_JobBenefits_Benefit_BenefitId",
				table: "JobBenefits",
				column: "BenefitId",
				principalTable: "Benefit",
				principalColumn: "BenefitId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_JobBenefits_Job_JobId",
				table: "JobBenefits",
				column: "JobId",
				principalTable: "Job",
				principalColumn: "JobId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_JobLevels_Job_JobId",
				table: "JobLevels",
				column: "JobId",
				principalTable: "Job",
				principalColumn: "JobId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_JobLevels_Levels_LevelId",
				table: "JobLevels",
				column: "LevelId",
				principalTable: "Levels",
				principalColumn: "LevelId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_JobSkill_Job_JobId",
				table: "JobSkill",
				column: "JobId",
				principalTable: "Job",
				principalColumn: "JobId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_JobSkill_Skill_SkillId",
				table: "JobSkill",
				column: "SkillId",
				principalTable: "Skill",
				principalColumn: "SkillId",
				onDelete: ReferentialAction.Cascade);

			migrationBuilder.AddForeignKey(
				name: "FK_RecoveryTokens_AspNetUsers_UserID",
				table: "RecoveryTokens",
				column: "UserID",
				principalTable: "AspNetUsers",
				principalColumn: "Id",
				onDelete: ReferentialAction.Cascade);
		}
	}
}
