using Microsoft.AspNetCore.Identity;

namespace Appointment.Domain;

public class AppUser : IdentityUser
{
    public string DisplayName { get; set; } = string.Empty;
}