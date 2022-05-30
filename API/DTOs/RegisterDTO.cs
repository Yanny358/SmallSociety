using System.ComponentModel.DataAnnotations;

namespace API.DTOs;

public class RegisterDTO
{
    [Required]
    public string DisplayName { get; set; }
    
    [Required]
    [EmailAddress]
    public string Email { get; set; }
    
    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{6,16}$", ErrorMessage = "Password must contain at least " +
        "one number, one uppercase, one lowercase, and length should be at least six characters")]
    public string Password { get; set; }
    
    [Required]
    public string Username { get; set; }
}