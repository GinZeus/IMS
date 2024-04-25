using IMS.Models;
using IMS.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class EditCandidateVM
    {
        public Candidate? SelectedCandidate { get; set; }

        [Required]
        [Display(Name = "Full Name")]
        public string FullName { get; set; }

        [Required]
        [Display(Name = "Date of Birth")]
        public DateTime? DOB { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string? Address { get; set; }

        [Required]
        [Display(Name = "Gender")]
        public Gender SelectedGender { get; set; }

        public IEnumerable<Gender> Genders { get; set; }

        public IFormFile? CVAttachment { get; set; }

        [Display(Name = "Position")]
        public int SelectedPositionId { get; set; }

        public IEnumerable<Position> Positions { get; set; }

        [Display(Name = "Skills")]
        public IEnumerable<int> SelectedSkillsId { get; set; }

        public IEnumerable<Skill> Skills { get; set; }

        [Display(Name = "Owner Recruiter")]
        public string SelectedRecruiterId { get; set; }

        public IEnumerable<User> RecruiterList { get; set; }

        [Display(Name = "Academic Level")]
        public int SelectedAcademicLevelId { get; set; }

        public IEnumerable<AcademicLevel> AcademicLevels { get; set; }

        [Display(Name = "Status")]
        public IEnumerable<CandidateStatus> CandidateStatuses { get; set; }

        public CandidateStatus SelectedStatus { get; set; }

		public int? YearOfExp { get; set; }
		public string? Note { get; set; }
		[Required]
		public DateTime CreatedOn { get; set; }
		public DateTime LastUpdatedOn { get; set; }
		[Required]
		public string LastUpdatedBy { get; set; }
	}
}
