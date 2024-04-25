using IMS.Models;
using Microsoft.AspNetCore.Identity;

namespace IMS.WebApp.ViewModels
{
    public class UserViewModel
    {
        public IEnumerable<User> User { get; set; }
        public IEnumerable<IdentityRole> Roles { get; set; }
        public Department Department { get; set; }
    }
}

