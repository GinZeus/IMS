using IMS.Models;
using IMS.Utilities.Constants;
namespace IMS.WebApp.ViewModels
{
	public class OfferVM
	{
		public int OfferId { get; set; }
		public Candidate Candidate { get; set; }
		public ContractType ContractType { get; set; }
		public Position Position { get; set; }
		public Level Level { get; set; }
		public User Manager { get; set; }
		public Department Department { get; set; }
		public InterviewSchedule InterviewSchedule { get; set; }
		public IEnumerable<User> Interviewer { get; set; }
		public User Recruiter { get; set; }
		public DateTime ContractFrom { get; set; }
		public DateTime ContractTo { get; set; }
		public DateTime DueDate { get; set; }
		public int BasicSalary { get; set; }
		public OfferStatus Status { get; set; }
		public string Note { get; set; }
		public DateTime CreatedOn { get; set; }

		public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }
    }
}
