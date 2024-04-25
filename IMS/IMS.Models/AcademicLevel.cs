using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    public class AcademicLevel
    {
        [Key]
        public int AcademicLevelId { get; set; }
        [Required]
        public required string AcademicLevelName { get; set; }
    }
}
