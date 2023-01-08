using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.AccountViewModel;

public record RegisterViewModel
{
    [Required]
    public string UserName { get; init; } = null!;

    [Required]
    [EmailAddress]
    public string Email { get; init; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; init; } = null!;

    [DataType(DataType.Password)]
    [Compare("Password")]
    public string ConfirmPassword { get; init; } = null!;
}