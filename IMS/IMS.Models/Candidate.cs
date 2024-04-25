using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using IMS.Utilities.Constants;

namespace IMS.Models
{
	public class Candidate
	{
		[Key]
		public int CandidateId { get; set; }

		[Required]
		public int PositionId { get; set; }
		[ForeignKey("PositionId")]
		public Position? Position { get; set; }

		public int AcademicLevelId { get; set; }
		[ForeignKey("AcademicLevelId")]
		public AcademicLevel? AcademicLevel { get; set; }

		public required string RecruiterId { get; set; }
		[ForeignKey("RecruiterId")]
		public User? Recruiter { get; set; }

		public required string FullName { get; set; }
		public DateTime DOB { get; set; }
		public Gender Gender { get; set; }

		public required string Email { get; set; }
		public required string Address { get; set; }
		public required string PhoneNumber { get; set; }

		public required byte[] CVAttachment { get; set; }
		public required string CVMimeType { get; set; }

		public int? YearOfExp { get; set; }

		public string? Note { get; set; }

		public CandidateStatus Status { get; set; }

		public DateTime CreatedOn { get; set; }

		public DateTime LastUpdatedOn { get; set; }

		public required string LastUpdatedBy { get; set; }

		public bool IsDeleted { get; set; } = false;
	}
}
