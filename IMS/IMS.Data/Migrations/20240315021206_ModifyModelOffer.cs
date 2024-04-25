using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace IMS.Data.Migrations
{
	/// <inheritdoc />
	public partial class ModifyModelOffer : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Offers_InterviewSchedules_InterviewScheduleId",
				table: "Offers");

			migrationBuilder.DropIndex(
				name: "IX_Offers_InterviewScheduleId",
				table: "Offers");

			migrationBuilder.DropColumn(
				name: "InterviewScheduleId",
				table: "Offers");

			migrationBuilder.AlterColumn<string>(
				name: "Notes",
				table: "Offers",
				type: "nvarchar(max)",
				nullable: true,
				oldClrType: typeof(string),
				oldType: "nvarchar(max)");

			migrationBuilder.CreateIndex(
				name: "IX_Offers_InterviewId",
				table: "Offers",
				column: "InterviewId");

			migrationBuilder.AddForeignKey(
				name: "FK_Offers_InterviewSchedules_InterviewId",
				table: "Offers",
				column: "InterviewId",
				principalTable: "InterviewSchedules",
				principalColumn: "InterviewScheduleId",
				onDelete: ReferentialAction.Restrict);
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.DropForeignKey(
				name: "FK_Offers_InterviewSchedules_InterviewId",
				table: "Offers");

			migrationBuilder.DropIndex(
				name: "IX_Offers_InterviewId",
				table: "Offers");

			migrationBuilder.AlterColumn<string>(
				name: "Notes",
				table: "Offers",
				type: "nvarchar(max)",
				nullable: false,
				defaultValue: "",
				oldClrType: typeof(string),
				oldType: "nvarchar(max)",
				oldNullable: true);

			migrationBuilder.AddColumn<int>(
				name: "InterviewScheduleId",
				table: "Offers",
				type: "int",
				nullable: false,
				defaultValue: 0);

			migrationBuilder.CreateIndex(
				name: "IX_Offers_InterviewScheduleId",
				table: "Offers",
				column: "InterviewScheduleId");

			migrationBuilder.AddForeignKey(
				name: "FK_Offers_InterviewSchedules_InterviewScheduleId",
				table: "Offers",
				column: "InterviewScheduleId",
				principalTable: "InterviewSchedules",
				principalColumn: "InterviewScheduleId",
				onDelete: ReferentialAction.Restrict);
		}
	}
}
