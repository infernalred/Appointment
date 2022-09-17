using Microsoft.AspNetCore.Identity;

namespace AppointmentService.Domain;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
    public string? Image { get; set; }
}