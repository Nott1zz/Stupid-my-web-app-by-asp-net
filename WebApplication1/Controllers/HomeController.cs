using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

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

    var posts = _db.Post
        .AsNoTracking()
        .Select(p => new Post
        {
            ID = p.ID,
            Post_name = p.Post_name ?? string.Empty,
            Post_img = p.Post_img ?? string.Empty,
            Post_by_id = p.Post_by_id,
            Date = p.Date,
            Participants = p.Participants,
            Capacity = p.Capacity,
            Location = p.Location,
            Post_Detail = p.Post_Detail ?? string.Empty
        })
        .ToList();
    
    var userIds = posts.Select(p => p.Post_by_id).Distinct().ToList();
    var usernames = _db.User
        .AsNoTracking()
        .Where(u => userIds.Contains(u.Id))
        .ToDictionary(u => u.Id, u => u.UserName ?? "Unknown User");

    var comment_PostIds = posts.Select(p => p.ID).Distinct().ToList();
    var comments = _db.Comments
        .AsNoTracking()
        .Where(c => comment_PostIds.Contains(c.PostID))
        .Select(c => new Comment
        {
            CommentID = c.CommentID,
            CommentText = c.CommentText ?? string.Empty,
            CreatedAt = c.CreatedAt,
            PostID = c.PostID,
            UserID = c.UserID
        })
        .ToList();

    ViewBag.Usernames = usernames;
    ViewBag.Id = userId;
    ViewBag.Comments = comments;

    return View(posts);
}

        [HttpPost]
public IActionResult CreateComment(string CommentText, int? id)
{
    int? userId = HttpContext.Session.GetInt32("ID");

    if (id == null || userId == null)
    {
        return Json(new { success = false, message = "Post ID or User ID is missing." });
    }

    if (string.IsNullOrWhiteSpace(CommentText))
    {
        return Json(new { success = false, message = "Comment text is required." });
    }

    int maxCommentId = _db.Comments.Any() ? _db.Comments.Max(c => c.CommentID) : 0;

    Comment obj = new Comment
    {
        CommentID = maxCommentId + 1,
        PostID = id.Value,
        UserID = userId.Value,
        CommentText = CommentText,
        CreatedAt = DateTime.Now
    };

    _db.Comments.Add(obj);
    _db.SaveChanges();

    var user = _db.User.FirstOrDefault(u => u.Id == userId);
    string username = user != null ? user.UserName : "Unknown User";

    return Json(new
    {
        success = true,
        comment = new
        {
            commentId = obj.CommentID,
            commentText = obj.CommentText,
            userID = obj.UserID,
            createdAt = obj.CreatedAt
        },
        username = username
    });
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