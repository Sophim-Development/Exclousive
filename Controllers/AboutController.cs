using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Exclousive.Models;

namespace Exclousive.Controllers;

public class AboutController : Controller
{
  private readonly ILogger<AboutController> _logger;

  public AboutController(ILogger<AboutController> logger)
  {
    _logger = logger;
  }

  public IActionResult Index()
  {
    return View();
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
