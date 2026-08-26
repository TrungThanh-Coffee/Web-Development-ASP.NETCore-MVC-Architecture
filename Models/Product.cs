namespace project_cuoiky.Models
{
    public class Product
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public int Quantity { get; set; }

        public string Image { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public string Description { get; set; } = string.Empty;

        public int CategoryId { get; set; }

        public decimal Promotion { get; set; }

        // Dung tích
        public string Volume { get; set; } = string.Empty;

        // Nồng độ
        public string Concentration { get; set; } = string.Empty;

        // Nhóm hương
        public string FragranceFamily { get; set; } = string.Empty;

        // Độ lưu hương
        public string Longevity { get; set; } = string.Empty;

        // Độ tỏa hương
        public string Sillage { get; set; } = string.Empty;

        // Thời điểm dùng
        public string RecommendedTime { get; set; } = string.Empty;

        public Category? Category { get; set; }

        public ICollection<OrderDetail> OrderDetails { get; set; }
            = new List<OrderDetail>();
    }
}