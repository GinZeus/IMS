using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    public class JobBenefit
    {
        [Key]
        public int JobBenefitId { get; set; }

        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; }

        public int BenefitId { get; set; }
        [ForeignKey("BenefitId")]
        public Benefit Benefit { get; set; }
    }
}
