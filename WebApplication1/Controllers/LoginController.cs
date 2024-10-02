using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDBContext _db;

        public LoginController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(User obj) 
        {

            if (ModelState.IsValid) 
            {
                var user = _db.User.FirstOrDefault(u => u.UserName == obj.UserName && u.Password == obj.Password);

                if (user != null)
                {
                    HttpContext.Session.SetString("Status", "Login");
                    HttpContext.Session.SetString("UserName", user.UserName);
                    HttpContext.Session.SetInt32("ID", user.Id); 
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError("", "Invalid username or password.");
                }
            }

            return View(obj); // Return ไปที่ view พร้อมกับโมเดลถ้า login ล้มเหลว
        }


        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Register(User obj)
        {
            try
            {
                _db.User.Add(obj); // Ensure you're adding to the correct DbSet
                _db.SaveChanges();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return View("Error", ex.Message);
            }

            return RedirectToAction("Index");
        }
        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Status");
            HttpContext.Session.Remove("UserName");
            HttpContext.Session.Remove("ID");
            return RedirectToAction("Index", "Home"); // เปลี่ยนไปที่หน้า Home
        }
    }
}
