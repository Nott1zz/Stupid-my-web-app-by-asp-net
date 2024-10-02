using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;
namespace WebApplication1.Models
{
    public class Join_Event
    {
        [Key]
        public int Join_ID { get; set; }

        public int UserID { get; set; }

        public int Post_ID { get; set; }

        public string Status { get; set; }
        
    }
}
