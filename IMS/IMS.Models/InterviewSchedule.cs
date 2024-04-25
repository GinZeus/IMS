using IMS.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMS.Models
{
	public class InterviewSchedule
	{
		[Key]
		public int InterviewScheduleId { get; set; }

		[Required]
		public int CandidateId { get; set; }
		[ForeignKey("CandidateId")]
		public Candidate Candidate { get; set; }

		[Required]
		public int JobId { get; set; }
		[ForeignKey("JobId")]
		public Job Job { get; set; }

		[Required]
		public string RecruiterId { get; set; }
		[ForeignKey("RecruiterId")]
		public User Recruiter { get; set; }

		[Required]
		[StringLength(150)]
		public string Title { get; set;}

		[StringLength(300)]
		public string? Location { get; set;}

		[Required]
		public DateTime DueDate { get; set;}
		public DateTime? StartTime  { get; set;}
		public DateTime? EndTime { get; set;}
		public string? MeetingId { get; set;}
		public string? Notes { get; set;}
		public bool? Result { get; set;}

		[Required]
		public InterviewScheduleStatus Status { get; set;}
		public DateTime CreatedOn { get; set; }
		public DateTime LastUpdatedOn { get; set; }

		[StringLength(50)]
		public required string LastUpdatedBy { get; set; }

		public ICollection<InterviewAssign> InterviewAssigns { get; set; }


	}
}
