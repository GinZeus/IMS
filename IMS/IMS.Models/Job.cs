using IMS.Utilities.Constants;
using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    public class Job
    {
        [Key]
        public int JobId { get; set; }

        [Required]
        public required string Title { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? SalaryFrom { get; set; }

        [Required]
        public int SalaryTo { get; set; }

        public string? WorkingAddress { get; set; }

        public string? Description { get; set; }

        public JobStatus Status { get; set; }

        public DateTime CreatedOn { get; set; }
        public DateTime LastUpdatedOn { get; set; }
        public string LastUpdatedBy { get; set; }

        public ICollection<JobBenefit> JobBenefits { get; set; }
        public ICollection<JobLevel> JobLevels { get; set; }
        public ICollection<JobSkill> JobSkills { get; set; }

        public bool IsDeleted { get; set; } = false;
    }
}
