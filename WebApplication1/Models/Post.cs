using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Models
{
    public class Post
    {
        [Key]
        public int ID { get; set; }

        public int Post_by_id { get; set; }

        [MaxLength(100)]
        public string Post_name { get; set; } = string.Empty;

        [MaxLength]
        public string Post_Detail { get; set; } = string.Empty;

        [MaxLength]
        public string Post_img { get; set; }= string.Empty;

        public int Capacity { get; set; } = 1;

        public DateTime? Date { get; set; } = DateTime.Now;

        public string Location { get; set; }= string.Empty;

        public int Participants { get; set; } = 0;

    }
}

