using IMS.Models;

namespace IMS.WebApp.ViewModels
{
    public class AdminHomepageVM
    {
        public User user { get; set; }

        public int UsersIsactiveCount { get; set; }

        public int UserInActiveCount { get; set; }

        public int InterviewerCount { get; set; }

        public int CandidateCount { get; set; }
        public Candidate candidate { get; set; }
    }
}
