using Microsoft.AspNetCore.Mvc;

namespace CombustionCarGuideWeb.Controllers
{
    public class ServiceController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
