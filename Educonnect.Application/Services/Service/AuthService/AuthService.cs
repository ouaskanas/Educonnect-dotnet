using Educonnect.Application.Dtos.AuthDto;
using Educonnect.Application.Services.IService;
using Educonnect.Application.Services.IService.IAuthService;
using Educonnect.Domain.Entities;
using Educonnect.Domain.Enums;
using Educonnect.Infrastructure.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Educonnect.Application.Services.Service.AuthService;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly ITokenService _tokenService;
    private readonly ApplicationDbContext _context;
    private readonly IConfiguration _config;
    private readonly IProfileService _profileService;

    public AuthService(
        UserManager<User> userManager,
        ITokenService tokenService,
        ApplicationDbContext context,
        IProfileService profileService,
        IConfiguration config)
    {
        _userManager  = userManager;
        _tokenService = tokenService;
        _context      = context;
        _config       = config;
        _profileService = profileService;
    }

    public async Task SignUpAsync(SignUpDto dto)
    {
        if (dto.Password != dto.PasswordConfirmation)
            throw new ArgumentException("Passwords do not match.");

        var user = new User
        {
            UserName    = dto.Username,
            Email       = dto.Email,
            Name        = dto.Name,
            PhoneNumber = dto.PhoneNumber,
            Role        = Role.User,
            CreateAt    = DateTime.UtcNow,
        };
        var result = await _userManager.CreateAsync(user, dto.Password);
        
        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(e => e.Description)));

        var profile = await _profileService.CreateProfile(user.Id);
    }

    public async Task<AuthResponseDto> SignInAsync(SignInDto dto)
    {
        var user = await _userManager.FindByNameAsync(dto.Username);

        if (user is null || !await _userManager.CheckPasswordAsync(user, dto.Password))
            throw new UnauthorizedAccessException("Invalid username or password.");

        return new AuthResponseDto
        {
            AccessToken  = _tokenService.GenerateAccessToken(user),
            RefreshToken = await IssueRefreshTokenAsync(user.Id),
            Message      = "Sign in successful.",
        };
    }

    public async Task<AuthResponseDto> RefreshAsync(string refreshToken)
    {
        var stored = await _context.RefreshTokens
            .Include(t => t.User)
            .SingleOrDefaultAsync(t => t.Token == refreshToken);

        if (stored is null || !stored.IsActive)
            throw new UnauthorizedAccessException("Invalid or expired refresh token.");

        stored.IsRevoked = true;
        stored.RevokedAt = DateTime.UtcNow;

        return new AuthResponseDto
        {
            AccessToken  = _tokenService.GenerateAccessToken(stored.User),
            RefreshToken = await IssueRefreshTokenAsync(stored.UserId),
            Message      = "Token refreshed.",
        };
    }

    public async Task SingOutAsync(string refreshToken)
    {
        var stored = await _context.RefreshTokens
            .SingleOrDefaultAsync(t => t.Token == refreshToken);

        if (stored is null || stored.IsRevoked) return;

        stored.IsRevoked = true;
        stored.RevokedAt = DateTime.UtcNow;
        await _context.SaveChangesAsync();
    }

    public async Task CreateAdmin(AdminCreationDto dto)
    {
        var user = new User
        {
            UserName = dto.Email,
            Email = dto.Email,
            Name = dto.Name,
            Role = Role.Admin,
            CreateAt = DateTime.UtcNow,
            Profile = new Profile { Username = dto.Name },
        };

        var result = await _userManager.CreateAsync(user, dto.Password);

        if (!result.Succeeded)
            throw new InvalidOperationException(
                string.Join(", ", result.Errors.Select(e => e.Description)));
    }

    private async Task<string> IssueRefreshTokenAsync(Guid userId)
    {
        var expiryDays = _config.GetValue<int>("Jwt:RefreshTokenExpiryDays", 7);

        var token = new RefreshToken
        {
            Token = _tokenService.GenerateRefreshToken(),
            UserId = userId,
            ExpiresAt = DateTime.UtcNow.AddDays(expiryDays),
        };

        _context.RefreshTokens.Add(token);
        await _context.SaveChangesAsync();

        return token.Token;
    }
}
