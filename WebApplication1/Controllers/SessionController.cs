using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http; 

namespace WebApplication1.Controllers
{
    public class SessionController : Controller
    {
        public IActionResult Index()
        {
            var status1 = HttpContext.Session.GetString("Status");
            var status2 = HttpContext.Session.GetString("UserName");
            var status3 = HttpContext.Session.GetInt32("ID");

            String Show = status1 + status2 + status3;
            if (status1 != null)
            {
                return Content(Show); 
            }
            else
            {
                return Content("Status not set.");
            }
        }
    }
}
