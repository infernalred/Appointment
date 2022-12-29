using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.AccountViewModel;

public class LoginViewModel
{
    [Required]
    public string UserName { get; set; } = null!;

    [Required]
    public string Password { get; set; } = null!;

    [Required]
    public string ReturnUrl { get; set; } = null!;
}