using Educonnect.Application.Dtos.ProfileDto;
using Educonnect.Application.Services.IService;
using Educonnect.Common.Exceptions;
using Educonnect.Domain.Entities;
using Educonnect.Infrastructure.Repositories.IRepository;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Services.Service
{
    public class ProfileService : IProfileService
    {
        private readonly IProfileRepository _profileRepository;
        private readonly UserManager<User> _userManager;

        public ProfileService(IProfileRepository profileRepository, UserManager<User> userManager)
        {
            this._profileRepository = profileRepository;
            this._userManager = userManager;
        }
        public async Task<ProfileCreationResponse> CreateProfile(Guid userId)
        {
            var user = await this._userManager.FindByIdAsync(userId.ToString()) ?? throw new EntityNotFoundException("Entity Not Found Exception");
            var profile = new Profile { Username = user.UserName ?? $"username{DateTime.UtcNow:yyyyMMddHHmmssfff}" , UserId = userId, User = user };
            await this._profileRepository.Add(profile);
            return new ProfileCreationResponse { Username = profile.Username, Description = profile.Description };
        }

        public async Task<SuspendProfileDto> SuspendProfile(Guid profileId, DateTime? until, Guid AdminId)
        {
            var profile = await _profileRepository.GetById(profileId) ?? throw new EntityNotFoundException("Entity Not Found Exception");
            profile.SuspendUser(until, AdminId); 
            await _profileRepository.Update(profile);
            return new SuspendProfileDto
            {
                ProfileName = profile.Username, 
                SuspendedAt = profile.SuspendedAt,
                SuspendedBy = profile.SuspendedBy,
                SuspendedUntil = profile.SuspendedUntil,
            };
        }

        public async Task<ProfileCreationResponse> UpdateProfile(Guid profileId, UpdateProfileRequest updateProfileDto)
        {
            var profile = await this._profileRepository.GetById(profileId) ?? throw new EntityNotFoundException("Entity Not Found Exception");
            bool isUserCredUpdated = false;

            if (!string.IsNullOrWhiteSpace(updateProfileDto.username))
            {
                profile.Username = updateProfileDto.username;
                profile.User.UserName = updateProfileDto.username;
                profile.User.NormalizedUserName = updateProfileDto.username.ToUpperInvariant();

                isUserCredUpdated = true;
            }

            if (!string.IsNullOrWhiteSpace(updateProfileDto.email))
            {
                profile.User.Email = updateProfileDto.email;
                profile.User.NormalizedEmail = updateProfileDto.email.ToUpperInvariant();

                isUserCredUpdated = true;
            }

            profile.Description = updateProfileDto.description ?? profile.Description;
            profile.User.Name = updateProfileDto.name ?? profile.User.Name;
            profile.User.PhoneNumber = updateProfileDto.phonenumber ?? profile.User.PhoneNumber;

            if (isUserCredUpdated)
            {
                profile.User.SecurityStamp = Guid.NewGuid().ToString();
            }

            await this._profileRepository.Update(profile);
            return new ProfileCreationResponse
            {
                Username = profile.Username,
                Description = profile.Description,
            };
        }
    }
}
