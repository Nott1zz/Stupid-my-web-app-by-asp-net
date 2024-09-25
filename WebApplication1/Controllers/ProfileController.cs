using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;

namespace WebApplication1.Controllers
{
    public class ProfileController : Controller
    {

        private readonly ApplicationDBContext _db;

        public ProfileController(ApplicationDBContext db)
        {
            _db = db;
        }
        public IActionResult Index()
        {

            return View();
        }
        public IActionResult EditProfile()
        {
            return View();
        }
    }
}
