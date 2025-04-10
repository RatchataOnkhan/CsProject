using System.Diagnostics;
using CombustionCarGuideWeb.Models;
using Microsoft.AspNetCore.Mvc;
using CombustionCarGuideWeb.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore; // ✅ เพิ่มสำหรับ Include()

namespace CombustionCarGuideWeb.Controllers
{
    namespace CombustionCarGuideWeb.Controllers
    {
        public class HomeController : Controller
        {
            private readonly ILogger<HomeController> _logger;
            private readonly ApplicationDbContext _db;

            public HomeController(ILogger<HomeController> logger, ApplicationDbContext db)
            {
                _logger = logger;
                _db = db;
            }

            public IActionResult Index()
            {
                var featuredCars = _db.Cars
                    .Include(c => c.Ratings) // สำคัญ
                    .AsEnumerable()
                    .OrderByDescending(c => c.Ratings.Any() ? c.Ratings.Average(r => r.Star) : 0)
                    .Take(3)
                    .ToList();


                var newCars = _db.Cars
                    .OrderByDescending(c => c.ReleaseYear)
                    .Take(3)
                    .ToList();

                var carList = _db.Cars
                    .OrderBy(c => c.ModelName)
                    .ToList();

                ViewBag.NewCars = newCars;
                ViewBag.ModelNameList = new SelectList(carList.Select(c => c.ModelName).Distinct());
                ViewBag.CarList = new SelectList(carList, "Id", "ModelName");

                return View(featuredCars);
            }



            public IActionResult Privacy()
            {
                return View();
            }

            public IActionResult Contact()
            {
                return View();
            }

            public IActionResult AdvancedSearch()
            {
                var cars = _db.Cars
                    .AsNoTracking()
                    .OrderBy(c => c.ModelName)
                    .ToList();

                return View(cars);
            }


            public IActionResult Compare(int? car1Id, int? car2Id)
            {
                var cars = _db.Cars
                    .AsNoTracking()
                    .OrderBy(c => c.ModelName)
                    .ToList();

                ViewBag.CarList = new SelectList(cars, "Id", "ModelName");

                List<Car> comparisonResult = new List<Car>();
                if (car1Id.HasValue) comparisonResult.Add(_db.Cars.AsNoTracking().FirstOrDefault(c => c.Id == car1Id));
                if (car2Id.HasValue) comparisonResult.Add(_db.Cars.AsNoTracking().FirstOrDefault(c => c.Id == car2Id));

                return View("Compare", comparisonResult);
            }

            public IActionResult Details(int id)
            {
                var car = _db.Cars
                    .AsNoTracking()
                    .Include(c => c.Comments).ThenInclude(c => c.User)
                    .Include(c => c.Ratings)
                    .Include(c => c.Brand)
                    .FirstOrDefault(c => c.Id == id);

                if (car == null)
                {
                    return NotFound();
                }

                var averageRating = car.Ratings?.Any() == true
                    ? car.Ratings.Average(r => r.Star)
                    : 0;

                ViewBag.AverageRating = averageRating;

                return View(car);
            }

            [HttpGet]
            public IActionResult Search(string modelName)
            {
                var results = _db.Cars
                    .Include(c => c.Brand)
                    .Where(c => string.IsNullOrEmpty(modelName) || c.ModelName.ToLower().Contains(modelName.ToLower()))
                    .ToList();

                ViewBag.ModelNameList = new SelectList(_db.Cars.Select(c => c.ModelName).Distinct());
                return View("Search", results);
            }

            [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
            public IActionResult Error()
            {
                return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
            }
        }
    }
}