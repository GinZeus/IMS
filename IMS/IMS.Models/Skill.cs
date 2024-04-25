using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    public class Skill
    {
        [Key]
        public int SkillId { get; set; }
        [Required]
        public required string SkillName { get; set; }

        public ICollection<JobSkill> JobSkills { get; set; }
    }
}
