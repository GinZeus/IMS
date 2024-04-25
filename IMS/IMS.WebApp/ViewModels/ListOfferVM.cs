using IMS.Models;

namespace IMS.WebApp.ViewModels
{
    public class ListOfferVM
    {
        public Offer Offer { get; set; }
        public Candidate Candidate { get; set; }
        public User Approver { get; set; }
        public User Recruiter { get; set; }

        public Department Department { get; set; }
    }
}
