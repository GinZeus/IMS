using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    public class Level
    {
        [Key]
        public int LevelId { get; set; }
        [Required]
        public required string LevelName { get; set; }

        public ICollection<JobLevel> JobLevels { get; set; }
    }
}
