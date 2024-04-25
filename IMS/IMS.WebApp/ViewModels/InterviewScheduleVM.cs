using IMS.Models;

namespace IMS.WebApp.ViewModels
{
    public class InterviewScheduleVM
    {
        public InterviewSchedule InterviewSchedule { get; set; }
        public Candidate Candidate { get; set; }
        public Job Job { get; set; }
        public IEnumerable<User> Interviewers { get; set; }

        public IEnumerable<User> Recruiters { get; set; }
    }
}
