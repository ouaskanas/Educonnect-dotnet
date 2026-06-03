using Educonnect.Domain.Entities;

namespace Educonnect.Application.Services.IService.IAuthService;

public interface ITokenService
{
    string GenerateAccessToken(User user);
    string GenerateRefreshToken();
}
