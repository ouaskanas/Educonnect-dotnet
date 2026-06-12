using Educonnect.Application.Dtos.ProfileDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Services.IService
{
    public interface IProfileService
    {
        Task<ProfileCreationResponse> CreateProfile(Guid userId);
        Task<ProfileCreationResponse> UpdateProfile(Guid profileId, UpdateProfileRequest updateProfileDto);
        Task<SuspendProfileDto> SuspendProfile(Guid profileId, DateTime? until, Guid AdminId); 
    }
}
