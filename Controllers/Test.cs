using Microsoft.AspNetCore.Mvc;
using CombustionCarGuideWeb.Data;

namespace CombustionCarGuideWeb.Controllers
{
    public class TestController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TestController(ApplicationDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var brands = _context.Brands.ToList(); // ดึงข้อมูลจากฐานข้อมูล
            return View(brands); // ส่งไปแสดงใน View
        }
    }
}
