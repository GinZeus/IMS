using IMS.Utilities.Constants;

namespace IMS.WebApp.ViewModels
{
	public class CandidateDetailsVM
	{
		// personal information
		public int CandidateId { get; set; }
		public string FullName { get; set; }
		public DateTime DOB { get; set; }
		public string PhoneNumber { get; set; }
		public string Email { get; set; }
		public string Address { get; set; }
		public Gender Gender { get; set; }

		// professional information
		public bool CvAvailable { get; set; }
		public string CVAttachmentName { get; set; }
		public string PositionName { get; set; }
		public IEnumerable<string> SkillsName { get; set; }
		public string RecruiterName { get; set; }
		public CandidateStatus StatusName { get; set; }
		public int? YearOfExp { get; set; }
		public string AcademicLevelName { get; set; }
		public string? Note { get; set; }

		public DateTime CreatedOn { get; set; }
		public DateTime LastUpdatedOn { get; set; }
		public string LastUpdatedBy { get; set; }
	}
}
