using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AppointmentService.Domain;
using Microsoft.IdentityModel.Tokens;

namespace AppointmentService.API.Services;

public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    
    public TokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string CreateToken(AppUser user, IEnumerable<string> roles)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.UserName ?? throw new InvalidOperationException()),
            new(ClaimTypes.NameIdentifier, user.Id),
            new(ClaimTypes.Email, user.Email ?? throw new InvalidOperationException())
        };
        claims.AddRange(roles.Select(role => new Claim(ClaimTypes.Role, role)));

        var daysTokenLife = _configuration.GetSection("DaysTokenLife").Get<int?>() ?? 7;
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["TokenKey"] ?? throw new InvalidOperationException()));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddDays(daysTokenLife),
            SigningCredentials = creds
        };

        var tokenHandler = new JwtSecurityTokenHandler();

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }
}