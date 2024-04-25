using Microsoft.AspNetCore.Identity;
using IMS.Models;
using System.ComponentModel.DataAnnotations;
using IMS.Utilities.Constants;
namespace IMS.WebApp.ViewModels
{
    public class EditUserVM
    {
        public EditUserVM()
        {
            Genders = new List<Gender> { Gender.Male, Gender.Female, Gender.Other };
        }

        [Required]
        [Display(Name = "Full Name")]
        public string? FullName { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string? Email { get; set; }
        public DateTime? DOB { get; set; }
        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Genders")]
        public IEnumerable<Gender> Genders { get; set; }
        public Gender SelectedGender { get; set; }
        
        [Required]
        [Display(Name = "Roles")]
        public IEnumerable<IdentityRole> Roles { get; set; }
        public string SelectedRole { get; set; }

        [Required]
        [Display(Name = "Departments")]
        public IEnumerable<Department> Departments { get; set; }
        public int SelectedDepartment { get; set; }
        public string? Note { get; set; }

        public bool IsActive { get; set; }

        public string Id { get; set; }
    }
}
