using AppointmentService.Domain;

namespace AppointmentService.API.Services;

public interface ITokenService
{
    public string CreateToken(AppUser user);
}