using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Exclousive.Models;

namespace Exclousive.Controllers;

public class ContactController : Controller
{
  private readonly ILogger<ContactController> _logger;

  public ContactController(ILogger<ContactController> logger)
  {
    _logger = logger;
  }

  public IActionResult Index()
  {
    return View();
  }

  [HttpPost]
  public IActionResult Submit(string name, string email, string message)
  {
    // Process the form data (e.g., save to database, send email, etc.)
    ViewBag.Message = "Thank you for contacting us!";
    return View("Index");
  }

  public IActionResult Privacy()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
