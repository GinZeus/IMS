using IMS.Models;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
	public class CreateInterviewScheduleVM
	{
		public string InterviewScheduleTitle { get; set; }
		public IEnumerable<Candidate> Candidates { get; set; }

		[Required]
		[Display(Name = "Selected Candidate")]
		public int? SelectedCandidate { get; set; }
		public DateTime ScheduleDate { get; set; }
		public DateTime? ScheduleFrom { get; set; }
		public DateTime? ScheduleTo { get; set; }
		public IEnumerable<Job> Jobs { get; set; }

		[Required]
		[Display(Name = "Selected Job")]
		public int? SelectedJob { get; set; }
		public IEnumerable<User> Interviewers { get; set; }

		[Required]
		[Display(Name = "Selected Interviewer")]
		public IEnumerable<string>? SelectedInterviewer { get; set; }
		public string? Location { get; set; }
		public IEnumerable<User> RecruiterOwners { get; set; }

		[Required]
		[Display(Name = "Selected Recruiter Owner")]
		public string? SelectedRecruiterOwner { get; set; }
		public string? MeetingId {  get; set; }
		public string? Notes {  get; set; }
	}
}
