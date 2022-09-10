using Appointment.Domain;

namespace Appointment.API.Services;

public interface ITokenService
{
    public string CreateToken(AppUser user);
}