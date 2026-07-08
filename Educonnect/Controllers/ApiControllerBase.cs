using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;

namespace Educonnect.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public abstract class ApiControllerBase : ControllerBase
{
    /// <summary>
    /// Gets the Identity User ID string of the authenticated user.
    /// </summary>
    protected string CurrentUserId => User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new UnauthorizedAccessException("User identification claim is missing from the token.");

    /// <summary>
    /// Gets the social Profile ID linked to the regular user.
    /// Throws an exception if an Admin tries to access a profile-bound route.
    /// </summary>
    protected Guid CurrentProfileId
    {
        get
        {
            var profileClaim = User.FindFirstValue("profile_id");

            if (string.IsNullOrEmpty(profileClaim))
            {
                throw new UnauthorizedAccessException("Profile claim is missing. This action is restricted to standard users.");
            }

            return Guid.Parse(profileClaim);
        }
    }
}