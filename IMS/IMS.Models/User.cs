using IMS.Utilities.Constants;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace IMS.Models
{
    public class User : IdentityUser
    {
        [PersonalData]
        [Required]
        public string FullName { get; set; }

        public string Address { get; set; }
        public DateTime DOB { get; set; }
        public Gender Gender { get; set; }
        public string? Note { get; set; }
        public bool IsActive { get; set; }

        public int DepartmentId { get; set; }

        [ForeignKey("DepartmentId")]
        public Department Department { get; set; }
    }
}
