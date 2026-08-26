using Microsoft.AspNetCore.Identity;

namespace project_cuoiky.Models
{
    public class AppUser : IdentityUser
    {

        public string FristName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string Image { get; set; } = string.Empty;

        public ICollection<Order> Orders { get; set; }
            = new List<Order>();
    }
}


