using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        public string Email { get; set; } = string.Empty;

        public string Role { get; set; } = string.Empty;

        public int Age { get; set; } = 0;

        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Username is required")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;

    }
}
