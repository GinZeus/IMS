using IMS.Models;
using IMS.Utilities.Constants;

namespace IMS.WebApp.ViewModels
{
	public class SubmitResultVM
	{
		public int InterviewScheduleId { get; set; }
		public string InterviewScheduleTitle { get; set; }
		public IEnumerable<Candidate> Candidates { get; set; }  // List Candidate posible to set
		public int SelectedCandidate { get; set; } // Selected in DB and after edit
		public DateTime ScheduleDate { get; set; }
		public DateTime? ScheduleFrom { get; set; }
		public DateTime? ScheduleTo { get; set; }
		public IEnumerable<Job> Jobs { get; set; } // List Jobs posible to set
		public int SelectedJob { get; set; } // Selected in DB and after edit
		public IEnumerable<User> Interviewers { get; set; } // List Interviewers posible to set
		public IEnumerable<string> SelectedInterviewer { get; set; }  // Selected in DB and after edit
		public string Location { get; set; }
		public IEnumerable<User> RecruiterOwners { get; set; } // List RecruiterOwners posible to set
		public string SelectedRecruiterOwner { get; set; } // Selected in DB and after edit
		public string MeetingId { get; set; }
		public string Notes { get; set; }
		public string LastUpdatedBy { get; set; }
		public DateTime CreatedOn { get; set; }
		public DateTime LastUpdatedOn { get; set; }
		public InterviewScheduleStatus Status { get; set; }

		public bool? Result { get; set; }
	}
}
