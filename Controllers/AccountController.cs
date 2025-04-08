using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using CombustionCarGuideWeb.Data;
using CombustionCarGuideWeb.Models;
using BCrypt.Net;

namespace CombustionCarGuideWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationDbContext db, ILogger<AccountController> logger)
        {
            _db = db;
            _logger = logger;
        }

        // GET: Login Page
        public IActionResult Login()
        {
            return View();
        }

        // POST: Login Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = _db.Users.FirstOrDefault(u => u.Username == model.Username);
            if (user != null && BCrypt.Net.BCrypt.Verify(model.Password, user.PasswordHash))
            {
                HttpContext.Session.SetString("UserID", user.Id.ToString());
                _logger.LogInformation($"User {model.Username} logged in successfully.");
                return RedirectToAction("Index", "Home");
            }

            ViewData["ErrorMessage"] = "Invalid username or password.";
            return View(model);
        }

        // GET: Register Page
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: Register Action
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUser(string Username  , string Password)
        {
            if (!ModelState.IsValid)
            {
                return View("Register");
            }

            if (_db.Users.Any(u => u.Username == Username))
            {
                ViewData["ErrorMessage"] = "Username already exists.";
                return View("Register");
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(Password);
            var user = new User
            {
                Username = Username,
                PasswordHash = hashedPassword
            };

            _db.Users.Add(user);
            _db.SaveChanges();

            TempData["SuccessMessage"] = "Registration successful. You can now log in.";
            return RedirectToAction("Login");
        }

        // Logout
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }

        // Check if user is logged in via session
        [HttpGet]
        public IActionResult CheckLogin()
        {
            var userId = HttpContext.Session.GetString("UserID");
            if (int.TryParse(userId, out int uid))
            {
                var user = _db.Users.FirstOrDefault(u => u.Id == uid);
                if (user != null)
                {
                    return Json(new { loggedIn = true, username = user.Username });
                }
            }
            return Json(new { loggedIn = false });
        }
    }
}
