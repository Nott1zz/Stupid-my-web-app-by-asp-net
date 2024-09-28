using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using WebApplication1.Data;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    public class PostController : Controller
    {
        private readonly ApplicationDBContext _db;

        public PostController(ApplicationDBContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            int? userId = HttpContext.Session.GetInt32("ID");

            var posts = _db.Post.Where(p => p.Post_by_id == userId).ToList();

            return View(posts);
        }

        // Approve คนที่กด join เข้ามา
        public IActionResult ApprovePost()
        {
            return View();
        }


        public IActionResult CreatePost()
        {
            int? userId = HttpContext.Session.GetInt32("ID");

            if (userId == null)
            {
                return RedirectToAction("Index", "Login");
            }
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreatePost(Post obj)
        {
            int? userId = HttpContext.Session.GetInt32("ID");

            if (userId.HasValue)
            {   
                if (string.IsNullOrEmpty(obj.Post_img))
                {
                    obj.Post_img = "https://flowbite.com/docs/images/examples/image-1@2x.jpg";
                }
                if (string.IsNullOrEmpty(obj.Post_Detail))
                {
                    obj.Post_Detail = ""; 
                }
                obj.Post_by_id = userId.Value;  
                _db.Post.Add(obj);
                _db.SaveChanges();

                return RedirectToAction("Index");
            }
            else
            {
                return View("Error", "User ID not found in session.");
            }
        }
        [HttpGet]
        public IActionResult GetPostById(int id)
        {
            var post = _db.Post.FirstOrDefault(p => p.ID == id);
            if (post == null)
            {
                return NotFound(); // Return 404 if post not found
            }
            return View(post); // Show the post if found
        }

        public IActionResult Edit(int id)
    {
        int? userId = HttpContext.Session.GetInt32("ID");
        var post = _db.Post.FirstOrDefault(p => p.ID == id && p.Post_by_id == userId);
        if (post == null)
        {
            return NotFound(); // หากไม่พบโพสต์ที่มี ID นั้น
        }
        return View(post); // ส่งข้อมูลไปยัง View Edit.cshtml
    }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Post post)
        {
            int? userId = HttpContext.Session.GetInt32("ID");
            if (ModelState.IsValid)
            {
                var postInDb = _db.Post.FirstOrDefault(p => p.ID == post.ID && p.Post_by_id == userId);
                if (postInDb == null)
                {
                    return NotFound();
                }

                // อัปเดตค่าต่าง ๆ
                postInDb.Post_name = post.Post_name;
                postInDb.Post_Detail = post.Post_Detail;
                postInDb.Capacity = post.Capacity;
                postInDb.Date = post.Date;
                postInDb.Location = post.Location;

                // ตรวจสอบว่าถ้า Post_img เป็นค่าว่างให้แทนที่ด้วยสตริงว่าง
                if (string.IsNullOrEmpty(post.Post_img))
                {
                    postInDb.Post_img = ""; // หรือค่าอื่นถ้าต้องการ
                }
                else
                {
                    postInDb.Post_img = post.Post_img;
                }
                if (string.IsNullOrEmpty(post.Post_Detail))
                {
                    postInDb.Post_Detail = ""; // หรือค่าอื่นถ้าต้องการ
                }
                else
                {
                    postInDb.Post_Detail = post.Post_Detail;
                }

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post); // ถ้าไม่สำเร็จ แสดงฟอร์มอีกครั้ง
        }
        [HttpGet]
        public IActionResult Delete_Post(int id)
        {
            var post = _db.Post.FirstOrDefault(p => p.ID == id);
            if (post == null)
            {
                return NotFound(); // Return 404 if post not found
            }
            _db.Post.Remove(post);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
