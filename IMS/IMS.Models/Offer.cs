using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMS.Utilities.Constants;

namespace IMS.Models
{
	public class Offer
	{
		[Key]
		public int OfferId { get; set; }

		[Required]
		public int CandidateId { get; set; }
		[ForeignKey("CandidateId")]
		public Candidate Candidate { get; set; }

		[Required]
		public string RecruiterOwnerId { get; set; }
		[ForeignKey("RecruiterOwnerId")]
		public User Recruiter { get; set; }

		[Required]
		public string ApproverId { get; set; }
		[ForeignKey("ApproverId")]
		public User Manager { get; set; }

		[Required]
		public int InterviewId { get; set; }
		[ForeignKey("InterviewId")]
		public InterviewSchedule InterviewSchedule { get; set; }

		[Required]
		public int ContractTypeID { get; set; }
		[ForeignKey("ContractTypeID")]
		public ContractType ContractType { get; set; }

		[Required]
		public int PositionId { get; set; }
		[ForeignKey("PositionId")]
		public Position Position { get; set; }

		[Required]
		public int DepartmentId { get; set; }
		[ForeignKey("DepartmentId")]
		public Department Department { get; set; }

		[Required]
		public int LevelId { get; set; }
		[ForeignKey("LevelId")]
		public Level Level { get; set; }

		[Required]
		public DateTime ContractFrom { get; set; }
		[Required]
		public DateTime ContractTo { get; set; }
		[Required]
		public DateTime DueDate { get; set; }
		[Required]
		public int BasicSalary {get; set; }

		public string? Notes { get; set; }
		[Required]
		public OfferStatus Status { get; set; }

		public DateTime CreatedOn{ get; set; }
		public DateTime LastUpdatedOn { get; set; }

		[StringLength(50)]
		public required string LastUpdatedBy { get; set; }
	}
}
