using Educonnect.Application.Dtos.AuthDto;

namespace Educonnect.Application.Services.IService.IAuthService;

public interface IAuthService
{
    Task<AuthResponseDto> SignInAsync(SignInDto signInDto);
    Task SignUpAsync(SignUpDto signUpDto);
    Task CreateAdmin(AdminCreationDto createAdminDto);
    Task<AuthResponseDto> RefreshAsync(string refreshToken);
    Task SingOutAsync(string refreshToken);
}
