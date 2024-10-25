using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Exclousive.Data;
using Exclousive.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System;

namespace Exclousive.Controllers
{
  public class AccountController : Controller
  {
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
      _context = context;
    }

    // Login GET
    public IActionResult Login()
    {
      return View();
    }

    // Login POST
    [HttpPost]
    public async Task<IActionResult> Login(Account model)
    {
      if (ModelState.IsValid)
      {
        // Retrieve user and check password
        var user = _context.Users.SingleOrDefault(u => u.Username == model.Username);

        if (user != null && VerifyPassword(model.Password, user.Password))
        {
          var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
          var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
          var principal = new ClaimsPrincipal(identity);

          await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

          return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Invalid login attempt.");
      }
      return View(model);
    }

    // Logout
    public async Task<IActionResult> Logout()
    {
      await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
      return RedirectToAction("Index", "Home");
    }

    // Register GET
    public IActionResult Register()
    {
      return View();
    }

    // Register POST
    [HttpPost]
    public async Task<IActionResult> Register(Account model)
    {
      if (ModelState.IsValid)
      {
        var userExists = _context.Users.Any(u => u.Username == model.Username);
        if (!userExists)
        {
          // Hash password before saving
          var hashedPassword = HashPassword(model.Password);
          var user = new User { Username = model.Username, Password = hashedPassword };
          _context.Users.Add(user);
          await _context.SaveChangesAsync();

          var claims = new[] { new Claim(ClaimTypes.Name, user.Username) };
          var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
          var principal = new ClaimsPrincipal(identity);

          await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

          return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "Username already exists.");
      }
      return View(model);
    }

    // Password hashing function
    private string HashPassword(string password)
    {
      // Hash the password using a secure method
      var salt = Convert.ToBase64String(KeyDerivation.Pbkdf2(
          password: password,
          salt: new byte[16], // Ideally generate a unique salt for each user
          prf: KeyDerivationPrf.HMACSHA256,
          iterationCount: 10000,
          numBytesRequested: 32));

      return salt;
    }

    // Password verification function
    private bool VerifyPassword(string enteredPassword, string storedPasswordHash)
    {
      // Verify password using the stored hash
      var enteredPasswordHash = HashPassword(enteredPassword);
      return enteredPasswordHash == storedPasswordHash;
    }
  }
}