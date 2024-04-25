using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    public class Benefit
    {
        [Key]
        public int BenefitId { get; set; }
        [Required]
        public required string BenefitName { get; set; }

        public ICollection<JobBenefit> JobBenefits { get; set; }
    }
}
