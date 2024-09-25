using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class PostController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult CreatePost()
        {
            return View();
        }
    }
}
