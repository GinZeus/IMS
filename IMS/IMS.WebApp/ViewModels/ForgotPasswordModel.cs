using System.ComponentModel.DataAnnotations;

namespace IMS.WebApp.ViewModels
{
    public class ForgotPasswordModel
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
    }
}
