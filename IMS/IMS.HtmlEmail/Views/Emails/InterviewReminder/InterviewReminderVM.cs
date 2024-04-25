

namespace IMS.HtmlEmail.Views.Emails.InterviewReminder
{
	public class InterviewReminderVM
	{
        public DateTime? StartTime {  get; set; }
        public DateTime? EndTime { get; set; }
        public string CandidateName {  get; set; }
        public string? Position { get; set; }
        public string? Recruiter { get; set; }
        public string InterviewDetailURL { get; set; }
        public string MeetingId { get; set; }

    }
}
