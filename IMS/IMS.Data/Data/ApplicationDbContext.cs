using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using IMS.Models;
using System.Reflection.Emit;
using IMS.Utilities.Constants;

namespace IMS.Data
{
	public class ApplicationDbContext : IdentityDbContext<User>
	{

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{ }

		protected override void OnModelCreating(ModelBuilder builder)
		{
			if (builder == null)
				throw new ArgumentNullException("modelBuilder");

			// for the other conventions, we do a metadata model loop
			foreach (var entityType in builder.Model.GetEntityTypes())
			{
				// equivalent of modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();
				entityType.GetForeignKeys()
					.Where(fk => !fk.IsOwnership && fk.DeleteBehavior == DeleteBehavior.Cascade)
					.ToList()
					.ForEach(fk => fk.DeleteBehavior = DeleteBehavior.Restrict);
			}

			base.OnModelCreating(builder);

			//Seeding data for Job table
			builder.Entity<Job>().HasData(
			new Job
			{
				JobId = 1,
				Title = "Software Engineer",
				StartDate = new DateTime(2024, 4, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(4954),
				EndDate = new DateTime(2025, 3, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(4974),
				SalaryFrom = 50000,
				SalaryTo = 75000,
				WorkingAddress = "Main Office",
				Description = "Develop and maintain software applications.",
				Status = (int)JobStatus.Open,
				LastUpdatedBy = "hung@gmail.com",
				CreatedOn = new DateTime(2024, 3, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(4977),
				LastUpdatedOn = new DateTime(2024, 3, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(4977)
			});

			//Seeding data for Candidate table
			builder.Entity<Candidate>().HasData(
				new Candidate
				{
					CandidateId = 1,
					FullName = "John Doe",
					DOB = new DateTime(2003, 7, 16),
					Gender = Gender.Male,
					Email = "john.doe@example.com",
					Address = "Chuong My, Ha Noi",
					PhoneNumber = "0123456789",
					CVAttachment = new byte[] { 0x20, 0x20 },
					CVMimeType = "application/pdf",
					PositionId = 1,
					AcademicLevelId = 1,
					RecruiterId = "a92ee657-1957-4e26-908e-963bd999ca08",
					YearOfExp = 1,
					Status = CandidateStatus.Open,
					CreatedOn = new DateTime(2024, 1, 1),
					LastUpdatedOn = new DateTime(2024, 1, 1),
					LastUpdatedBy = "hung@gmail.com",
					IsDeleted = false
				});

			//Seeding data for Interview Schedule
			builder.Entity<InterviewSchedule>().HasData(
				new InterviewSchedule
				{
					InterviewScheduleId = 1,
					CandidateId = 1,
					JobId = 1,
					RecruiterId = "a92ee657-1957-4e26-908e-963bd999ca08",
					Title = "Software Engineer Interview",
					DueDate = new DateTime(2024, 3, 19, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(5342),
					Status = InterviewScheduleStatus.Open,
					LastUpdatedBy = "System",
					CreatedOn = new DateTime(2024, 3, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(5364),
					LastUpdatedOn = new DateTime(2024, 3, 12, 15, 54, 46, 268, DateTimeKind.Local).AddTicks(5365)
				});

			// Seeding data for Department table
			builder.Entity<Department>().HasData(
				new Department { DepartmentId = 1, DepartmentName = "IT" },
				new Department { DepartmentId = 2, DepartmentName = "HR" },
				new Department { DepartmentId = 3, DepartmentName = "Finance" },
				new Department { DepartmentId = 4, DepartmentName = "Communication" },
				new Department { DepartmentId = 5, DepartmentName = "Marketing" },
				new Department { DepartmentId = 6, DepartmentName = "Accounting" }
				);

			// Seeding data for Level table
			builder.Entity<Level>().HasData(
				new Level { LevelId = 1, LevelName = "Fresher" },
				new Level { LevelId = 2, LevelName = "Junior" },
				new Level { LevelId = 3, LevelName = "Senior" },
				new Level { LevelId = 4, LevelName = "Leader" },
				new Level { LevelId = 5, LevelName = "Manager" },
				new Level { LevelId = 6, LevelName = "Vice Head" }
				);

			// Seeding data for Academic Level table
			builder.Entity<AcademicLevel>().HasData(
				new AcademicLevel { AcademicLevelId = 1, AcademicLevelName = "High school" },
				new AcademicLevel { AcademicLevelId = 2, AcademicLevelName = "Bachelor's Degree" },
				new AcademicLevel { AcademicLevelId = 3, AcademicLevelName = "Master Degree" },
				new AcademicLevel { AcademicLevelId = 4, AcademicLevelName = "PhD" }
				);

			// Seeding data for Position table
			builder.Entity<Position>().HasData(
				new Position { PositionId = 1, PositionName = "Backend Developer" },
				new Position { PositionId = 2, PositionName = "Business Analyst" },
				new Position { PositionId = 3, PositionName = "Tester" },
				new Position { PositionId = 4, PositionName = "HR" },
				new Position { PositionId = 5, PositionName = "Project Manager" },
				new Position { PositionId = 6, PositionName = "Not available" }
				);

			// Seeding data for Skill table
			builder.Entity<Skill>().HasData(
				new Skill { SkillId = 1, SkillName = "Java" },
				new Skill { SkillId = 2, SkillName = "NodeJS" },
				new Skill { SkillId = 3, SkillName = ".NET" },
				new Skill { SkillId = 4, SkillName = "C++" },
				new Skill { SkillId = 5, SkillName = "Business Analyst" },
				new Skill { SkillId = 6, SkillName = "Communication" }
				);

			// Seeding data for Benefit table
			builder.Entity<Benefit>().HasData(
				new Benefit { BenefitId = 1, BenefitName = "Lunch" },
				new Benefit { BenefitId = 2, BenefitName = "25-day leave" },
				new Benefit { BenefitId = 3, BenefitName = "Healthcare insurance" },
				new Benefit { BenefitId = 4, BenefitName = "Hybrid working" },
				new Benefit { BenefitId = 5, BenefitName = "Travel" }
				);

			// Seeding data for Contract Type table
			builder.Entity<ContractType>().HasData(
				new ContractType { ContractTypeId = 1, ContractTypeTitle = "Trial 2 months" },
				new ContractType { ContractTypeId = 2, ContractTypeTitle = "Trainee 3 months" },
				new ContractType { ContractTypeId = 3, ContractTypeTitle = "1 year" },
				new ContractType { ContractTypeId = 4, ContractTypeTitle = "3 years and Unlimited" }
				);

			// Seeding data for AspNetRoles table
			builder.Entity<IdentityRole>().HasData(
				new IdentityRole { Id = "1bfa4d50-b885-48b9-835a-f47ad854046b", Name = "Admin", NormalizedName = "ADMIN" },
				new IdentityRole { Id = "03482d4b-c825-4ae8-a1f1-26e98f3b8a5b", Name = "Recruiter", NormalizedName = "RECRUITER" },
				new IdentityRole { Id = "f1de4cb5-7f7c-4ba9-9453-22ec3840984b", Name = "Interviewer", NormalizedName = "INTERVIEWER" },
				new IdentityRole { Id = "2281c643-65a2-4fd6-83bc-ae240794b875", Name = "Manager", NormalizedName = "MANAGER" }
				);

			// Seeding ROOT USER for AspNetUser table
			User user = new User
			{
				Id = "ac7215cc-b3fb-41ab-a4d0-8a40798f55ee",
				FullName = "Ha Quang Thang",
				Address = "Viet Nam",
				DOB = new DateTime(2003, 7, 16),
				Gender = Gender.Male,
				IsActive = true,
				DepartmentId = 1,
				UserName = "Root",
				NormalizedUserName = "ROOT",
				EmailConfirmed = true,
				Email = "root@gmail.com",
				NormalizedEmail = "ROOT@GMAIL.COM",
				SecurityStamp = "448303c6-eacc-4616-9fe3-e51d4a8bffc1",
				ConcurrencyStamp = "965b857f-509c-4508-9f57-d33602e0de51",
				PasswordHash = "AQAAAAIAAYagAAAAEFIAshPAyXNlyLsBx6EiOFNI03BbZ2yWx5WbiHOxGdRjfsKWjxVwvJqmxfxRzdmUvg=="
			};
			User user2 = new User
			{
				Id = "a92ee657-1957-4e26-908e-963bd999ca08",
				FullName = "John Doe",
				Address = "USA",
				Gender = Gender.Other,
				IsActive = true,
				DepartmentId = 1,
				UserName = "DoeJ1",
				NormalizedUserName = "DOEJ1",
				EmailConfirmed = true,
				Email = "hung@gmail.com",
				NormalizedEmail = "HUNG@GMAIL.COM",
				ConcurrencyStamp = "b5393e82-423a-4582-b316-991b19470be0",
				SecurityStamp = "e8a3f408-0be8-40c9-9eab-4514e8a0a696",
				PasswordHash = "AQAAAAIAAYagAAAAEHe0f4CQ9BGCMxhkGWUtaIht9qw3fPfwbDszYesEMsqp2J/RAd8IM6e9PAL4DVwDVg=="
			};

			builder.Entity<User>().HasData(user, user2);

			// Seeding AspNetUserRole table
			IdentityUserRole<string> rootAdminRole = new IdentityUserRole<string>
			{
				RoleId = "1bfa4d50-b885-48b9-835a-f47ad854046b",
				UserId = "ac7215cc-b3fb-41ab-a4d0-8a40798f55ee"
			};

            IdentityUserRole<string> hungRecruiterRole = new IdentityUserRole<string>
            {
                RoleId = "03482d4b-c825-4ae8-a1f1-26e98f3b8a5b",
                UserId = "a92ee657-1957-4e26-908e-963bd999ca08"
            };

			builder.Entity<IdentityUserRole<string>>().HasData(rootAdminRole, hungRecruiterRole);
        }

		public DbSet<Department> Department { get; set; }
		public DbSet<Benefit> Benefit { get; set; }
		public DbSet<AcademicLevel> AcademicLevel { get; set; }
		public DbSet<Job> Job { get; set; }
		public DbSet<Skill> Skill { get; set; }
		public DbSet<Level> Levels { get; set; }
		public DbSet<Position> Positions { get; set; }
		public DbSet<JobSkill> JobSkill { get; set; }
		public DbSet<JobBenefit> JobBenefits { get; set; }
		public DbSet<JobLevel> JobLevels { get; set; }
		public DbSet<Candidate> Candidates { get; set; }
		public DbSet<InterviewSchedule> InterviewSchedules { get; set; }
		public DbSet<Offer> Offers { get; set; }
		public DbSet<InterviewAssign> InterviewAssigns { get; set; }
		public DbSet<ContractType> ContractTypes { get; set; }
		public DbSet<CandidateSkill> CandidateSkills { get; set; }
		public DbSet<RecoveryToken> RecoveryTokens { get; set; }

	}
}
