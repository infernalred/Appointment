using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.AccountViewModel;

public record RegisterViewModel
{
    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;

    [Required]
    [RegularExpression("(?=.*\\d)(?=.*[a-z])(?=.*[A-Z]).{4,8}$", ErrorMessage = "Password must be complex")]
    [DataType(DataType.Password)]
    public string Password { get; init; } = null!;

    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    public string ConfirmPassword { get; init; } = null!;

    [Required] public string DisplayName { get; init; } = null!;
}