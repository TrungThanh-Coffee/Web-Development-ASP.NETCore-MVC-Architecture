using System.ComponentModel.DataAnnotations;

namespace project_cuoiky.Models
{
    public class Order
    {
        public int Id { get; set; }

        // IdentityUser sử dụng Id kiểu string
        [StringLength(450)]
        public string UserId { get; set; } = string.Empty;

        public DateTime OrderedDate { get; set; }

        public string Payment { get; set; } = string.Empty;

        public AppUser? User { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
            = new List<OrderDetail>();
    }
}
