using System.ComponentModel.DataAnnotations;

public class Login
{
    [Key] // ระบุว่าฟิลด์นี้เป็น primary key
    public int Id { get; set; } // Primary key

    [Required(ErrorMessage = "Username is required.")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; }
}
