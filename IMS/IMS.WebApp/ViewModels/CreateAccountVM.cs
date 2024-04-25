using IMS.Models;
using IMS.Utilities.Constants;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
	public class CreateAccountVM
	{
		[Required]
		[Display(Name = "Full Name")]
		public string? FullName { get; set; }
		[Required]
		[EmailAddress]
		[Remote(action: "IsEmailExist", controller: "Account")]
		public string? Email { get; set; }
		[Required]
		public DateTime? DOB { get; set; }
		[Required]
		public string? Address { get; set; }
		[Required]
		[Phone]
		[Display(Name = "Phone Number")]
		public string? PhoneNumber { get; set; }
		public IEnumerable<Gender> Genders { get; set; }
		[Required]
		[Display(Name = "Gender")]
		public Gender? SelectedGender { get; set; }
		[Required]
		[Display(Name = "Role")]
		public string? SelectedRole { get; set; }
		public IEnumerable<IdentityRole> Roles { get; set; }
		[Required]
		[Display(Name = "Department")]
		public int? SelectedDepartmentId { get; set; }
		public IEnumerable<Department> Departments { get; set; }
		public string? Note { get; set; }
	}
}
