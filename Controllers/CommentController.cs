using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using CombustionCarGuideWeb.Data;
using CombustionCarGuideWeb.Models;

namespace CombustionCarGuideWeb.Controllers
{
    public class CommentController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CommentController(ApplicationDbContext db)
        {
            _db = db;
        }

        // ✅ ฟอร์มจาก Razor View (Details.cshtml)
        [HttpPost]
        public IActionResult AddComment(int CarId, string Text)
        {
            var userIdStr = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized("กรุณา Login ก่อนแสดงความคิดเห็น");
            }

            var car = _db.Cars.Find(CarId);
            if (car == null)
            {
                return NotFound("ไม่พบรถที่คุณต้องการคอมเมนต์");
            }

            var comment = new Comment
            {
                CarId = CarId,
                UserId = userId,
                Text = Text,
                CreatedAt = DateTime.Now
            };

            _db.Comments.Add(comment);
            _db.SaveChanges();

            return RedirectToAction("Details", "Cars", new { id = CarId });
        }

        // ✅ สำหรับ API (รับ JSON body, เช่น AJAX call)
        [HttpPost]
        [Route("api/comments/add")]
        public IActionResult AddCommentApi([FromBody] Comment model)
        {
            var userIdStr = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized(new { message = "กรุณา Login ก่อน Comment" });
            }

            var user = _db.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null) return BadRequest("User not found");

            var comment = new Comment
            {
                CarId = model.CarId,
                UserId = user.Id,
                Text = model.Text,
                CreatedAt = DateTime.Now
            };

            _db.Comments.Add(comment);
            _db.SaveChanges();

            return Ok(new
            {
                user = user.Username,
                text = comment.Text,
                date = comment.CreatedAt
            });
        }

        // ✅ สำหรับดึงคอมเมนต์ทั้งหมดของรถ
        [HttpGet]
        public IActionResult GetComments(int carId)
        {
            var comments = _db.Comments
                .Where(c => c.CarId == carId)
                .Include(c => c.User)
                .Select(c => new
                {
                    c.Text,
                    c.CreatedAt,
                    User = c.User != null ? c.User.Username : "(Unknown)"
                })
                .ToList();

            return Json(comments);
        }
    }
}
