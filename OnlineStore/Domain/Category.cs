namespace OnlineStore.Core.Domain;

public class Category
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Discount { get; set; }
    public ICollection<Product> Products { get; set; } = new List<Product>();
}