using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class Comment
    {
        [Key]
        public int CommentID { get; set; }

        [Required]
        public string CommentText { get; set; } = string.Empty;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public int PostID { get; set; }

        public int UserID { get; set; }
    }
}
