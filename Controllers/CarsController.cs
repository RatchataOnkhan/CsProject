// ========== CarsController.cs (อัปเดตล่าสุด) ==============
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using CombustionCarGuideWeb.Models;
using Microsoft.AspNetCore.Http;
using CombustionCarGuideWeb.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CombustionCarGuideWeb.Controllers
{
    public class CarsController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CarsController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult GetCarNames()
        {
            var carList = _db.Cars
                .Include(c => c.Brand)
                .AsNoTracking()
                .Select(c => new
                {
                    brand = c.Brand != null ? c.Brand.Name : "",
                    modelName = c.ModelName ?? ""
                })
                .Take(100)
                .ToList();

            return Json(carList);
        }

        public IActionResult Featured()
        {
            var featuredCars = _db.Cars
                .AsNoTracking()
                .Take(6)
                .ToList();

            return View(featuredCars);
        }


        public IActionResult New()
        {
            ViewBag.LoginUrl = "";
            var newCars = _db.Cars
                .AsNoTracking()
                .OrderByDescending(c => c.ReleaseYear)
                .Take(6)
                .ToList();

            var userIdStr = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIdStr, out int userId)) ViewBag.LoginUrl = Url.Action("Login", "Account");

            return View(newCars);
        }

        public IActionResult Brands()
        {
            var brands = _db.Cars
                .AsNoTracking()
                .Where(c => !string.IsNullOrEmpty(c.CarBrand))
                .Select(c => c.CarBrand)
                .Distinct()
                .OrderBy(b => b)
                .ToList();

            return View(brands); // View expects List<string>
        }

        [HttpGet]
        public JsonResult GetCarsByBrand(string brand)
        {
            var cars = _db.Cars
                .AsNoTracking()
                .Where(c => c.CarBrand == brand)
                .AsEnumerable()
                .GroupBy(c => c.ModelName)
                .Select(g => g.First())
                .Select(c => new
                {
                    id = c.Id,
                    modelName = c.ModelName,
                    engineType = c.EngineType,
                    releaseYear = c.ReleaseYear,
                    msrp = c.MSRP
                })
                .ToList();

            return Json(cars);
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

        public IActionResult Search(string modelName)
        {
            var modelNames = _db.Cars
                .AsNoTracking()
                .Select(c => c.ModelName)
                .Distinct()
                .OrderBy(name => name)
                .ToList();

            ViewBag.ModelNameList = new SelectList(modelNames);

            var cars = _db.Cars.AsNoTracking().AsQueryable();

            if (!string.IsNullOrEmpty(modelName))
            {
                cars = cars.Where(c => c.ModelName == modelName);
            }

            return View(cars.ToList());
        }

        public IActionResult Compare(int? car1Id, int? car2Id)
        {
            var cars = _db.Cars.AsNoTracking().OrderBy(c => c.ModelName).ToList();
            ViewBag.CarList = new SelectList(cars, "Id", "ModelName");

            List<Car> comparisonResult = new List<Car>();
            if (car1Id.HasValue) comparisonResult.Add(_db.Cars.AsNoTracking().FirstOrDefault(c => c.Id == car1Id));
            if (car2Id.HasValue) comparisonResult.Add(_db.Cars.AsNoTracking().FirstOrDefault(c => c.Id == car2Id));

            return View("Compare", comparisonResult);
        }

        [HttpPost]
        public IActionResult RateCar(int carId, int rating)
        {
            var userIdStr = HttpContext.Session.GetString("UserID");

            if (!int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized("กรุณา Login ก่อนให้ Rating");
            }

            var carExists = _db.Cars.Any(c => c.Id == carId);
            if (!carExists)
            {
                return NotFound("ไม่พบรถที่ให้คะแนน");
            }

            var existingRating = _db.Ratings.FirstOrDefault(r => r.CarId == carId && r.UserId == userId);

            if (existingRating != null)
            {
                // แก้คะแนน
                existingRating.Star = rating;
            }
            else
            {
                _db.Ratings.Add(new Rating
                {
                    CarId = carId,
                    UserId = userId,
                    Star = rating
                });
            }

            _db.SaveChanges();

            return RedirectToAction("Details", new { id = carId });
        }

        [HttpPost]
        [Route("api/cars/rate")]
        public IActionResult RateCarApi(int carId, int rating)
        {
            var userIdStr = HttpContext.Session.GetString("UserID");

            if (!int.TryParse(userIdStr, out int userId))
            {
                return Unauthorized(new { message = "กรุณา Login ก่อนให้ Rating" });
            }

            var carExists = _db.Cars.Any(c => c.Id == carId);
            if (!carExists)
            {
                return NotFound(new { message = "Car not found" });
            }

            var existingRating = _db.Ratings.FirstOrDefault(r => r.CarId == carId && r.UserId == userId);

            if (existingRating != null)
            {
                existingRating.Star = rating;
            }
            else
            {
                _db.Ratings.Add(new Rating
                {
                    CarId = carId,
                    UserId = userId,
                    Star = rating
                });
            }

            _db.SaveChanges();
            return Ok(new { message = "Rating Updated" });
        }

        public IActionResult ICE()
        {
            var iceCars = _db.Cars
                .AsNoTracking()
                .Where(c => c.Type == "ICE")
                .ToList();

            ViewData["Title"] = "รถยนต์เครื่องยนต์สันดาปภายใน (ICE)";
            return View("ICE", iceCars);
        }

        public IActionResult Hybrid()
        {
            var hybridCars = _db.Cars
                .AsNoTracking()
                .Where(c => c.Type == "Hybrid")
                .ToList();

            ViewData["Title"] = "รถยนต์ Hybrid";
            return View("Hybrid", hybridCars);
        }

        public IActionResult EV()
        {
            var evCars = _db.Cars
                .AsNoTracking()
                .Where(c => c.Type == "EV")
                .ToList();

            ViewData["Title"] = "รถยนต์ไฟฟ้า (EV)";
            return View("EV", evCars);
        }

        // ⭐ แสดงรถที่คุณถูกใจไว้
        public IActionResult Favorites()
        {
            var userIdStr = HttpContext.Session.GetString("UserID");
            if (!int.TryParse(userIdStr, out int userId)) return RedirectToAction("Login", "Account");


            var connectionString = _db.Database.GetDbConnection().ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            List<int> carIdList = new List<int>();

            try
            {
                conn.Open();
                string checkFavoriteSql = "select * from FavoriteCar where UserId = @UserId ";
                SqlCommand cmd = new SqlCommand(checkFavoriteSql, conn);
                cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserID"));


                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtCheck = new DataTable("CHECK");
                da.Fill(dtCheck);

                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    foreach (DataRow dr in dtCheck.Rows)
                    {
                        carIdList.Add(int.Parse(dr["CarId"].ToString()));
                    }



                }


            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }




            var favoriteCars = _db.Cars
                .Where(c => carIdList.Contains(c.Id))
                .ToList();

            return View(favoriteCars);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public JsonResult ToggleFavorite(string carId)
        {
            var connectionString = _db.Database.GetDbConnection().ConnectionString;
            SqlConnection conn = new SqlConnection(connectionString);
            bool isFavorite = false;
            try
            {
                conn.Open();
                string checkFavoriteSql = "select * from FavoriteCar where UserId = @UserId and CarId = @CarId";
                SqlCommand cmd = new SqlCommand(checkFavoriteSql, conn);
                cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserID"));
                cmd.Parameters.AddWithValue("@CarId", carId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dtCheck = new DataTable("CHECK");
                da.Fill(dtCheck);

                if (dtCheck != null && dtCheck.Rows.Count > 0)
                {
                    string updateSql = "";
                    if (dtCheck.Rows[0]["Flag"].Equals(0))
                    {
                        isFavorite = true;
                        updateSql = "update FavoriteCar set Flag = 1 where  UserId = @UserId and CarId = @CarId";
                        cmd = new SqlCommand(updateSql, conn);
                        cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserID"));
                        cmd.Parameters.AddWithValue("@CarId", carId);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        isFavorite = false;
                        updateSql = "update FavoriteCar set Flag = 0 where  UserId = @UserId and CarId = @CarId";
                        cmd = new SqlCommand(updateSql, conn);
                        cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserID"));
                        cmd.Parameters.AddWithValue("@CarId", carId);
                        cmd.ExecuteNonQuery();

                    }
                }
                else
                {
                    isFavorite = true;
                    string insertNewFavoriteSql = "insert into FavoriteCar (UserId , CarId , Flag) values  (@UserId , @CarId , @Flag)  ";

                    cmd = new SqlCommand(insertNewFavoriteSql, conn);
                    cmd.Parameters.AddWithValue("@UserId", HttpContext.Session.GetString("UserID"));
                    cmd.Parameters.AddWithValue("@CarId", carId);
                    cmd.Parameters.AddWithValue("@Flag", 1);
                    cmd.ExecuteNonQuery();
                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (conn.State == System.Data.ConnectionState.Open)
                    conn.Close();
            }



            return Json(new
            {
                Result = true,
                isFavorite = isFavorite
            });
        }







        public IActionResult AdminPanel()
        {
            var cars = _db.Cars.AsNoTracking().OrderBy(c => c.Id).ToList();
            return View(cars);
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var car = _db.Cars.Find(id);
            return View(car);
        }

        [HttpPost]
        public IActionResult Edit(Car car)
        {
            _db.Cars.Update(car);
            _db.SaveChanges();
            return RedirectToAction("AdminPanel");
        }

        [HttpGet]
        public IActionResult Add()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Car car)
        {
            _db.Cars.Add(car);
            _db.SaveChanges();
            return RedirectToAction("AdminPanel");
        }

        public IActionResult Delete(int id)
        {
            var car = _db.Cars.Find(id);
            if (car != null)
            {
                _db.Cars.Remove(car);
                _db.SaveChanges();
            }
            return RedirectToAction("AdminPanel");
        }
    }
}