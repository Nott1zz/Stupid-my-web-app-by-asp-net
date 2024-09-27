using System;
using System.ComponentModel.DataAnnotations;


namespace WebApplication1.Models
{
    public class Post
    {
        [Key]
        public int ID { get; set; }

        public int Post_by_id { get; set; }

        [MaxLength]
        public string Post_name { get; set; }

        [MaxLength]
        public string Post_Detail { get; set; } = string.Empty;

        [MaxLength]
        public string Post_img { get; set; }= string.Empty;

        public int Capacity { get; set; }

        public DateTime? Date { get; set; }

        public int Location { get; set; }

        public int Participants { get; set; } = 0;
    }
}

