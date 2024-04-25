using IMS.Models;
using IMS.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
	public class EditInterviewScheduleVM
	{
		public int InterviewScheduleId { get; set; }
		public string InterviewScheduleTitle { get; set; }
		public IEnumerable<Candidate> Candidates { get; set; }  // List Candidate posible to set

		[Required]
		[Display(Name ="Selected candidate")]
		public int? SelectedCandidate { get; set; } // Selected in DB and after edit

		[Required]
		[Display(Name = "Schedule date")]
		public DateTime? ScheduleDate { get; set; }
		public DateTime? ScheduleFrom { get; set; }
		public DateTime? ScheduleTo { get; set; }
		public IEnumerable<Job> Jobs { get; set; } // List Jobs posible to set

		[Required]
		[Display(Name = "Selected job")]
		public int? SelectedJob { get; set; } // Selected in DB and after edit
		public IEnumerable<User> Interviewers { get; set; } // List Interviewers posible to set

		[Required]
		[Display(Name = "Selected interviewer")]
		public IEnumerable<string>? SelectedInterviewer { get; set; }  // Selected in DB and after edit
		public string? Location { get; set;}
		public IEnumerable<User> RecruiterOwners { get; set; } // List RecruiterOwners posible to set

		[Required]
		[Display(Name = "Selected recruiter owner")]
		public string? SelectedRecruiterOwner { get; set; } // Selected in DB and after edit
		public string? MeetingId { get; set; }
		public string? Notes { get; set; }
		public InterviewScheduleStatus Status { get; set; }
	}
}
