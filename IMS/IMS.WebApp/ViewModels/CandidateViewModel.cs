using IMS.Models;

namespace IMS.WebApp.ViewModels
{
    public class CandidateViewModel
    {
        public required Candidate Candidate { get; set; }

        public required Position Position { get; set; }

        public required User User { get; set; }

    }
}

