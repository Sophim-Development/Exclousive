using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Exclousive.Models;

namespace Exclousive.Controllers;

public class BannerController : Controller
{
  private readonly ILogger<HomeController> _logger;

  public BannerController(ILogger<HomeController> logger)
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
