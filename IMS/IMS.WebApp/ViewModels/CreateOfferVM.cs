using IMS.Models;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class CreateOfferVM
    {
        public IEnumerable<Candidate> Candidates { get; set; }

        [Required]
		[Display(Name = "Candidate")]
		public int? SelectedCandidate {  get; set; }

        public IEnumerable<Position> Positions { get; set; }

		[Required]
		[Display(Name = "Position")]
		public int? SelectedPosition { get; set; }

        public IEnumerable<ContractType> ContractTypes { get; set; }

		[Required]
		[Display(Name = "Contract Type")]
		public int? SelectedContractTypes { get; set;}

        public IEnumerable<Level> Levels { get; set; }

		[Required]
		[Display(Name = "Level")]
		public int? SelectedLevels { get; set; }

        public IEnumerable<User> Manager { get; set; }

		[Required]
		[Display(Name = "Approver")]
		public string? SelectedManager { get; set; }

        public IEnumerable<Department> Departments { get; set; }

		[Required]
		[Display(Name = "Department")]
		public int? SelectedDepartment {  get; set; }

        public IEnumerable<User> Recruiter { get; set; }

		[Required]
		[Display(Name = "Recruiter")]
		public string? SelectedRecruiter { get;set; }

        public IEnumerable<InterviewSchedule> Interviews { get; set; }

		[Required]
		[Display(Name = "Interview Schedule")]
		public int? SelectedInterviewSchedule { get; set; }

		[Required]
		[Display(Name = "Contract From")]
		public DateTime? ContractFrom { get; set; }

		[Required]
		[Display(Name = "Contract To")]
		public DateTime? ContractTo{ get; set; }

		[Required]
		[Display(Name = "Due Date")]
		public DateTime? DueDate { get; set; }

		[Required]
		[Display(Name = "Basic Salary")]
        public int? BasicSalary { get; set; }
        public string? Notes { get; set; }
		public string LastUpdatedBy { get; set; }

	}
}
