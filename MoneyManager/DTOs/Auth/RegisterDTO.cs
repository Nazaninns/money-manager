using System.ComponentModel.DataAnnotations;

namespace MoneyManager.DTOs.Auth;

public class RegisterDTO
{
    [Required(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Invalid Email Address")]
    public string Email { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required")]
    [MinLength(6, ErrorMessage = "Password must have at least 6 characters")]
    public string Password { get; set; } = string.Empty;
}