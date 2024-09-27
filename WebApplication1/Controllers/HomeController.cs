using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _db;

        // รวม logger และ db ไว้ใน constructor เดียว
        public HomeController(ILogger<HomeController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            // ตรวจสอบสถานะ session และส่งค่าไปยัง View
            ViewBag.UserStatus = HttpContext.Session.GetString("Status");
            int? userId = HttpContext.Session.GetInt32("ID");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var posts = _db.Post.ToList();
            return View(posts);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        // Action สำหรับ Logout
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
