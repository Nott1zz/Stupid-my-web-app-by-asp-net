using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Linq;
using System.Diagnostics;

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
            var posts = _db.Post.ToList();
            var userIds = posts.Select(p => p.Post_by_id).Distinct().ToList();
            var usernames = _db.User
                .Where(u => userIds.Contains(u.Id))
                .ToDictionary(u => u.Id, u => u.UserName);

            ViewBag.Usernames = usernames;

            return View(posts);
        }

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
                obj.Post_Detail ??= "";
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
                return NotFound();
            }

            var username = _db.User
                .Where(u => u.Id == post.Post_by_id)
                .Select(u => u.UserName)
                .FirstOrDefault();

            ViewBag.Username = username;

            return View(post);
        }

        public IActionResult Edit(int id)
        {
            int? userId = HttpContext.Session.GetInt32("ID");
            var post = _db.Post.FirstOrDefault(p => p.ID == id && p.Post_by_id == userId);
            if (post == null)
            {
                return NotFound();
            }
            return View(post);
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

                postInDb.Post_name = post.Post_name;
                postInDb.Post_Detail = post.Post_Detail ?? "";
                postInDb.Capacity = post.Capacity;
                postInDb.Date = post.Date;
                postInDb.Location = post.Location;
                postInDb.Post_img = string.IsNullOrEmpty(post.Post_img) ? "" : post.Post_img;

                _db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(post);
        }

        [HttpGet]
        public IActionResult Delete_Post(int id)
        {
            int? userId = HttpContext.Session.GetInt32("ID");
            var post = _db.Post.FirstOrDefault(p => p.ID == id && p.Post_by_id == userId);
            if (post == null)
            {
                return NotFound();
            }
            _db.Post.Remove(post);
            _db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}