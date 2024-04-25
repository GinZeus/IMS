using IMS.Models;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class CreateJobVM
    {
		[Required]
		[Display(Name = "Job title")]
		public string JobTitle { get; set; }

        public IEnumerable<Skill> Skills { get; set; }

        [Required]
        [Display(Name = "Selected skills")]
        public IEnumerable<int>? SelectedSkills { get; set; }

        public int? SalaryFrom { get; set; }
		[Required]
		[Display(Name = "Salary to")]
		public int SalaryTo { get; set; }
        public IEnumerable<Benefit> Benefits { get; set; }

		[Required]
		[Display(Name = "Selected benefits")]
		public IEnumerable<int>? SelectedBenefits { get; set; }
        public string? WorkingAddress { get; set; }

		[Required]
		[Display(Name = "Start date")]
		public DateTime StartDate { get; set; }
		[Required]
		[Display(Name = "End date")]
		public DateTime EndDate { get; set; }

        public IEnumerable<Level> Levels { get; set; }

		[Required]
		[Display(Name = "Selected levels")]
		public IEnumerable<int>? SelectedLevels { get; set; }
        public string? Description { get; set; }
    }
}
