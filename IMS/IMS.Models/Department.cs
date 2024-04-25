using System.ComponentModel.DataAnnotations;

namespace IMS.Models
{
    public class Department
    {
        [Key]
        public int DepartmentId { get; set; }

        [Required]
        public required string DepartmentName { get; set; }
    }
}
