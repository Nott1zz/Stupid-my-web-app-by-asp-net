using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Diagnostics;
using WebApplication1.Data;
using WebApplication1.Models;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Controllers
{
    public class CommunityController : Controller
    {
        private readonly ApplicationDBContext _db;

        public CommunityController(ApplicationDBContext db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index()
        {
            int? userId = HttpContext.Session.GetInt32("ID");
            ViewBag.UserStatus = HttpContext.Session.GetString("Status");
            ViewBag.UserName = HttpContext.Session.GetString("UserName");

            // Fetch the join events asynchronously
            var joinEvents = await _db.Join_Event.Where(u => u.UserID == userId && u.Status == "Approve").ToListAsync();
            ViewBag.myjoin = joinEvents;

            // Fetch posts asynchronously
            var posts = await _db.Post
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
                .ToListAsync();

            // Fetch distinct user IDs and their corresponding usernames asynchronously
            var userIds = posts.Select(p => p.Post_by_id).Distinct().ToList();
            var usernames = await _db.User
                .AsNoTracking()
                .Where(u => userIds.Contains(u.Id))
                .ToDictionaryAsync(u => u.Id, u => u.UserName ?? "Unknown User");

            // Fetch comments asynchronously
            var comment_PostIds = posts.Select(p => p.ID).Distinct().ToList();
            var comments = await _db.Comments
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
                .ToListAsync();

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

        public IActionResult GetComments(int id)
        {
            var comments = _db.Comments
                .AsNoTracking()
                .Where(c => c.PostID == id)
                .OrderBy(c => c.CreatedAt)
                .Select(c => new Comment
                {
                    CommentID = c.CommentID,
                    CommentText = c.CommentText ?? string.Empty,
                    CreatedAt = c.CreatedAt,
                    PostID = c.PostID,
                    UserID = c.UserID
                })
                .ToList();

            var usernames = _db.User
                .AsNoTracking()
                .Where(u => comments.Select(c => c.UserID).Contains(u.Id))
                .ToDictionary(u => u.Id, u => u.UserName ?? "Unknown User");

            ViewBag.Usernames = usernames;
            return PartialView("_CommentsPartial", comments);  // Create a partial view for comments
        }

        [HttpPost]
        public IActionResult RequestJoin(int? postId)
        {
            int? userId = HttpContext.Session.GetInt32("ID");

            if (postId == null || userId == null)
            {
                return Json(new { success = false, message = "Post ID or User ID is missing." });
            }

            // Check if the user has already requested to join
            var existingRequest = _db.Join_Event
                .FirstOrDefault(r => r.Post_ID == postId.Value && r.UserID == userId.Value);

            if (existingRequest != null)
            {
                return Json(new { success = false, message = "You have already requested to join this post." });
            }

            // Save a new join request
            var request = new Join_Event
            {
                Post_ID = postId.Value, // Use Post_ID here
                UserID = userId.Value,
                Status = "Request"
            };

            _db.Join_Event.Add(request);
            _db.SaveChanges();

            return Json(new { success = true, message = "Your request to join has been sent successfully." });
        }
    }
}