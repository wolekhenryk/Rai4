using Rai4.Domain.Models;

namespace Rai4.Application.Services.Interfaces;

public interface IJwtTokenService
{
    string GenerateToken(User user);
}
