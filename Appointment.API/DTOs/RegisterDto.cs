using System.ComponentModel.DataAnnotations;

namespace Appointment.API.DTOs;

public class RegisterDto
{
    [Required]
    public string DisplayName { get; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; } = string.Empty;
    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be stronger")]
    public string Password { get; } = string.Empty;
    [Required]
    public string UserName { get; } = string.Empty;
}