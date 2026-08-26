namespace project_cuoiky.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Image { get; set; } = string.Empty;

        public decimal Price { get; set; }

        public int Quantity { get; set; }
        public int Stock { get; set; }

        public decimal Total
        {
            get
            {
                return Price * Quantity;
            }
        }
    }
}