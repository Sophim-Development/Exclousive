// File: Models/HomeViewModel.cs
namespace Exclousive.Models
{
  public class HomeViewModel
  {
    public string Title { get; set; }
    public required IEnumerable<Product> FeaturedProducts { get; set; }
    // public IEnumerable<Category> Categories { get; set; 
  }
}
