namespace Exclousive.Models
{
  public class Product
  {
    public int Id { get; set;}
    public required string Name { get; set; }
    public string ImageUrl { get; set; }
    public decimal Price { get; set; }
    public required string Category { get; set; }
    public bool IsOnSale { get; set; } = true;
    public bool IsDeleted { get; set; } = false;
    public string Description { get; set; }
    public int Stock { get; set; }
    }
}