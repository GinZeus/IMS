using IMS.Models;
using IMS.Utilities.Constants;
using IMS.WebApp.CustomValidations;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
	public class CreateCandidateVM
	{
		[Display(Name = "Full Name")]
		public string FullName { get; set; }
		[Required]
		public DateTime? DOB { get; set; }
		[Phone]
		[Display(Name = "Phone Number")]
		public string PhoneNumber { get; set; }
		[EmailAddress]
		public string Email { get; set; }
		public string Address { get; set; }

		[Required]
		[Display(Name = "Gender")]
		public Gender? SelectedGender { get; set; }
		public IEnumerable<Gender> Genders { get; set; }

		[Display(Name = "CV Attachment")]
		[MaxFileSize(5 * 1024 * 1024)]
		public IFormFile CVAttachment { get; set; }

		[Required]
		[Display(Name = "Position")]
		public int? SelectedPositionId { get; set; }
		public IEnumerable<Position> Positions { get; set; }

		[Display(Name = "Skills")]
		public IEnumerable<int> SelectedSkillId { get; set; }
		public IEnumerable<Skill> Skills { get; set; }

		[Display(Name = "Owner Recruiter")]
		public string SelectedRecruiterId { get; set; }
		public IEnumerable<User> RecruiterList { get; set; }

		[Required]
		[Display(Name = "Academic Level")]
		public int? SelectedAcademicLevelId { get; set; }
		public IEnumerable<AcademicLevel> AcademicLevels { get; set; }
		public IEnumerable<CandidateStatus> CandidateStatuses { get; set; }

		[Required]
		[Display(Name = "Candidate Status")]
		public CandidateStatus? SelectedStatus { get; set; }

		public int? YearOfExp { get; set; }
		public string? Note { get; set; }

		public DateTime CreatedOn { get; set; }
		public DateTime LastUpdatedOn { get; set; }

		public string LastUpdatedBy { get; set; }
	}
}
