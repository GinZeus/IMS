using IMS.Models;
using Microsoft.AspNetCore.Identity;

namespace IMS.WebApp.ViewModels
{
    public class DetailUserViewModel
    {
        public User User { get; set; }
        public Department Department { get; set; }

        public string Role { get; set; }

    }
}
