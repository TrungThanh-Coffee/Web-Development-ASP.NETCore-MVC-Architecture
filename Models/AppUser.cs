using Microsoft.AspNetCore.Identity;

namespace project_cuoiky.Models
{
    public class AppUser : IdentityUser
    {

        public string Name { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public ICollection<Order> Orders { get; set; }
            = new List<Order>();
    }
}


