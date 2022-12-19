using System.ComponentModel.DataAnnotations;

namespace Identity.API.Models.AccountViewModel;

public record LoginViewModel
{
    [Required]
    [EmailAddress]
    public string Username { get; init; } = null!;

    [Required]
    [DataType(DataType.Password)]
    public string Password { get; init; } = null!;
}