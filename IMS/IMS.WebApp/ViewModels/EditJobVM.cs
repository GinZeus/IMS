using IMS.Models;
using IMS.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class EditJobVM
    {
        public int JobID { get; set; }

        [Required]
        [Display (Name="Job Title")]
        public  string JobTitle { get; set; }

        [Required]
        [Display(Name = "Skills")]
        public IEnumerable<Skill> Skills { get; set; }

        public IEnumerable<int> SelectedSkills { get; set; }

        public int? SalaryFrom { get; set; }
        public int SalaryTo { get; set; }

        [Required]
        [Display(Name = "Benefits")]
        public IEnumerable<Benefit> Benefits { get; set; }
        public IEnumerable<int> SelectedBenefits { get; set; }
        public string? WorkingAddress { get; set; }

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Levels")]
        public IEnumerable<Level> Levels { get; set; }
        public IEnumerable<int> SelectedLevels { get; set; }
        public string? Description { get; set; }
        public JobStatus Status { get; set; }
            
    }
}
