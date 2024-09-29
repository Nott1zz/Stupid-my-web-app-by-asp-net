using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Linq;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationDBContext _db;

        public HomeController(ILogger<HomeController> logger, ApplicationDBContext db)
        {
            _logger = logger;
            _db = db;
        }

        public IActionResult Index()
        {
            ViewBag.UserStatus = HttpContext.Session.GetString("Status");
            int? userId = HttpContext.Session.GetInt32("ID");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            var posts = _db.Post.ToList();
            
            // Fetch usernames for all posts
            var userIds = posts.Select(p => p.Post_by_id).Distinct().ToList();
            var usernames = _db.User
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id, u => u.UserName);

            ViewBag.Usernames = usernames;

            return View(posts);
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
}