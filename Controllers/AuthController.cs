
using System.Diagnostics;
using CombustionCarGuideWeb.Models;
using Microsoft.AspNetCore.Mvc;
using CombustionCarGuideWeb.Data;





public class AuthController : Controller
{
    private readonly ApplicationDbContext _db;

    public AuthController(ApplicationDbContext db)
    {
        _db = db;
    }

    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }

    //เสริมความปลอดภัยพื้นฐานใน Login
    [HttpPost]
    public IActionResult Login(User model)
    {
        if (string.IsNullOrWhiteSpace(model.Username) || string.IsNullOrWhiteSpace(model.PasswordHash))
        {
            ViewBag.Error = "กรุณากรอก Username และ Password";
            return View();
        }

        var dummyUsers = new List<User> {
    new User { Id = 1, Username = "admin", PasswordHash = "1234" }
};

        var user = dummyUsers.FirstOrDefault(u => u.Username == model.Username && u.PasswordHash == model.PasswordHash);

        if (user != null)
        {
            HttpContext.Session.SetString("UserID", user.Id.ToString());
            return RedirectToAction("Index", "Home");
        }

        ViewBag.Error = "Username หรือ Password ไม่ถูกต้อง";
        return View();
    }


    [HttpPost]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Index", "Home");
    }

    //เพิ่มตรวจสอบ null-safe ใน CheckLogin
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
