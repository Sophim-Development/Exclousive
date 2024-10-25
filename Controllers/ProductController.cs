using Exclousive.Data;
using Exclousive.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Threading.Tasks;

namespace Exclousive.Controllers
{
  public class ProductsController : Controller
  {
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _hostingEnvironment;

    // Single constructor that accepts both dependencies
    public ProductsController(AppDbContext context, IWebHostEnvironment hostingEnvironment)
    {
      _context = context;
      _hostingEnvironment = hostingEnvironment;
    }

    // GET: Products
    public async Task<IActionResult> Index()
    {
      var products = await _context.Products
          .Where(p => !p.IsDeleted) // Only get products that are not marked as deleted
          .ToListAsync();
      return View(products);
    }

    // GET: Products/Details/5
    public async Task<IActionResult> Details(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }
      var product = await _context.Products
          .FirstOrDefaultAsync(m => m.Id == id);
      if (product == null)
      {
        return NotFound();
      }
      return View(product);
    }

    // GET: Products/Create
    public IActionResult Create()
    {
      return View();
    }

    // POST: Products/Create
    [HttpPost]
    public async Task<IActionResult> Create(Product product, IFormFile imageFile)
    {
      if (ModelState.IsValid)
      {
        if (imageFile != null && imageFile.Length > 0)
        {
          // Ensure the directory exists
          var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
          Directory.CreateDirectory(uploadsFolder);

          var fileName = Path.GetFileName(imageFile.FileName);
          var filePath = Path.Combine(uploadsFolder, fileName);

          // Save the file to the server
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await imageFile.CopyToAsync(stream);
          }

          product.ImageUrl = "/uploads/" + fileName; // Update the ImageUrl to the relative path
        }

        _context.Products.Add(product);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(product);
    }

    // GET: Products/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
      if (id == null)
      {
        return NotFound();
      }
      var product = await _context.Products.FindAsync(id);
      if (product == null)
      {
        return NotFound();
      }
      return View(product);
    }

    // POST: Products/Edit/5
    [HttpPost]
    public async Task<IActionResult> Edit(Product product, IFormFile imageFile)
    {
      if (ModelState.IsValid)
      {
        // Retrieve the existing product from the database
        var existingProduct = await _context.Products.FindAsync(product.Id);
        if (existingProduct == null)
        {
          return NotFound();
        }

        // Update existing product properties
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.Stock = product.Stock;
        existingProduct.Category = product.Category;
        existingProduct.IsOnSale = product.IsOnSale;

        // Handle image upload if a new image file is provided
        if (imageFile != null && imageFile.Length > 0)
        {
          var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "uploads");
          Directory.CreateDirectory(uploadsFolder); // Ensure the directory exists

          var fileName = Path.GetFileName(imageFile.FileName);
          var filePath = Path.Combine(uploadsFolder, fileName);
          // Save the new file
          using (var stream = new FileStream(filePath, FileMode.Create))
          {
            await imageFile.CopyToAsync(stream);
          }
          existingProduct.ImageUrl = "/uploads/" + fileName; // Update ImageUrl to the new file path
        }
        else
        {
          // If no new image is uploaded, retain the old ImageUrl
          existingProduct.ImageUrl = existingProduct.ImageUrl;
        }
        // Save changes to the database
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
      }
      return View(product);
    }

    // POST: Products/DeleteConfirmed/5
    [HttpPost, ActionName("DeleteConfirmed")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
      var product = await _context.Products.FindAsync(id);
      if (product != null)
      {
        // Set IsDeleted to true instead of removing the product
        product.IsDeleted = true;
        _context.Products.Update(product); // Update the product
        await _context.SaveChangesAsync();
      }
      return RedirectToAction(nameof(Index));
    }

    private bool ProductExists(int id)
    {
      return _context.Products.Any(e => e.Id == id);
    }
  }
}
