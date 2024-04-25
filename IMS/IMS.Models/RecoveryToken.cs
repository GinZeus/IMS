using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models
{
    public class RecoveryToken 
    {
        [Key]
        public required string UserID { get; set; }
        [ForeignKey("UserID")]
        public required User User { get; set; }

        [Required]
        public string? Code { get; set; }
        [Required]
        public DateTime ExpiredTime { get; set; }

    }
}
