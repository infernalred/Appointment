using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.AccountViewModel;

public class LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; set; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    public bool RememberMe { get; set; }

    [Required]
    public string ReturnUrl { get; set; } = null!;
}