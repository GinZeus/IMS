using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models
{
    public class JobSkill
    {
        [Key]
        public int JobSkillId { get; set; }

        public int JobId { get; set; }
        [ForeignKey("JobId")]
        public Job Job { get; set; }

        public int SkillId { get; set; }
        [ForeignKey("SkillId")]
        public Skill Skill { get; set;}
    }
}
