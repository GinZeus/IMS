using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    public class Position
    {
        [Key] 
        public int PositionId { get; set; }
        [Required]
        public required string PositionName { get; set; }
    }
}
