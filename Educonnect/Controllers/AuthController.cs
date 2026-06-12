using Educonnect.Application.Dtos.AuthDto;
using Educonnect.Application.Services.IService.IAuthService;
using Microsoft.AspNetCore.Mvc;

namespace Educonnect.Controllers
{
    [Route("api/auth")]
    public class AuthController : Controller
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            this._authService = authService;
        }

        [HttpPost("signup")]
        public async Task<IActionResult> signup([FromBody] SignUpDto signUpDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                await _authService.SignUpAsync(signUpDto);
                return Ok(new { message = "User Signed In" });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }

        [HttpPost("signin")]
        public async Task<IActionResult> signin([FromBody] SignInDto signInDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            try
            {
                var response = await _authService.SignInAsync(signInDto);
                return Ok(new { message = response });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] string request)
        {
            if (string.IsNullOrWhiteSpace(request))
                return BadRequest(new { error = "Refresh token is required." });
            try
            {
                var response = await _authService.RefreshAsync(request);
                return Ok(response);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new { error = ex.Message });
            }
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody] string request)
        {
            if (string.IsNullOrWhiteSpace(request))
                return BadRequest(new { error = "Refresh token is required." });

            try
            {
                await _authService.SingOutAsync(request);
                return Ok(new { message = "Signed out successfully." });
            }
            catch (Exception ex)
            {
                return BadRequest(new { error = "Invalid refresh token." });
            }
        }

    }
}
