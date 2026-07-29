namespace project_cuoiky.Models
{
    public class Category
    {
        public int Id { get; set; }

        public string Description { get; set; } = string.Empty;

        public ICollection<Product> Products { get; set; }
            = new List<Product>();
    }
}