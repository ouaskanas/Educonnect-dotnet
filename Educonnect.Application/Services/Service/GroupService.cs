using Educonnect.Application.Dtos.GroupDto;
using Educonnect.Application.Services.IService;
using Educonnect.Common.Exceptions;
using Educonnect.Common.Pagination.Dto;
using Educonnect.Domain.Entities;
using Educonnect.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Services.Service
{
    public class GroupService : IGroupsService
    {
        private readonly IGroupRepository _groupRepository;
        private readonly IProfileRepository _profileRepository;
        public GroupService(IGroupRepository groupRepository, IProfileRepository profileRepository)
        {
            this._groupRepository = groupRepository;
            this._profileRepository = profileRepository;
        }
        public async Task<GetGroupDto> GetGroup(Guid groupId, Guid profilId)
        {
            var group = await this._groupRepository.GetById(groupId) ?? throw new EntityNotFoundException("Group Not Found");
            return new GetGroupDto
            {
                Id = groupId,
                Name = group.Name,
                Description = group.Description,
                AdminId = group.AdminId,
                AdminName = group.Admin.Username,
                PostCount = group.PostIds.Count(),
                MembreCount = group.MembersIds.Count(),
                IsMembre = group.MembersIds.Contains(profilId)
            };
        }

        public async Task<List<GetGroupDto>> GetGroups(Guid profilId, PaginationParameters pagination)
        {
            var groups = await this._groupRepository.GetGroupsAsync(pagination);
            return groups.Data.Select(g => new GetGroupDto
            {
                Id = g.Id,
                Name = g.Name,
                Description = g.Description,
                AdminId = g.AdminId,
                AdminName = g.Admin.Username,
                PostCount = g.PostIds.Count(),
                IsMembre = g.MembersIds.Contains(profilId)
            }).ToList();
        }

        public async Task<CreateGroupResponse> CreateGroup(Guid profilId, CreateGroupRequest createGroupRequest)
        {
            var profile = await this._profileRepository.GetById(profilId) ?? throw new EntityNotFoundException("User not found");
            var group = new Group
            {
                Name = createGroupRequest.Name,
                Description = createGroupRequest.Description,
                Admin = profile,
                AdminId = profilId,
            };
            await this._groupRepository.Add(group);
            return new CreateGroupResponse
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                AdminName = group.Admin.Username,
                CreatedBy = group.AdminId,
                CreatedDate = group.CreatedAt,
            };
        }

        public async Task<AddParticipantesResponse> AddParticipantesToGroup(Guid groupId, Guid adminProfileId, List<Guid> memberIds)
        {
            if (memberIds == null || !memberIds.Any())
            {
                return new AddParticipantesResponse { Id = groupId, Count = 0, Users = new List<Dictionary<string, Guid>>() };
            }

            var distinctMemberIds = memberIds.Distinct().ToList();

            var admin = await _profileRepository.GetById(adminProfileId)
                        ?? throw new EntityNotFoundException("Profile Not Found");

            if (admin.User?.Role != Domain.Enums.Role.Admin)
            {
                throw new UnauthorizedAccessException("You don't have access to add participants to this group");
            }

            var group = await _groupRepository.GetById(groupId)
                        ?? throw new EntityNotFoundException("Group Not Found");

            var profilesToAdd = await _profileRepository.GetRange(distinctMemberIds);

            var userList = new List<Dictionary<string, Guid>>();

            if (group.Membres is ICollection<Profile> membresCollection)
            {
                foreach (var profile in profilesToAdd)
                {
                    if (!membresCollection.Contains(profile))
                    {
                        membresCollection.Add(profile);
                        userList.Add(new Dictionary<string, Guid> { { profile.Username, profile.Id } });
                    }
                }
            }

            await _groupRepository.Update(group);

            return new AddParticipantesResponse
            {
                Id = group.Id,
                Count = userList.Count,
                Users = userList
            };
        }

        public async Task<UpdateGroupResponse> UpdateGroup(Guid profilId, Guid groupId, UpdateGroupRequest updateGroupRequest)
        {
            if (updateGroupRequest is null)
            {
                return new UpdateGroupResponse { Id = Guid.Empty, Description = string.Empty, Name = string.Empty, UpdatedAt = null };
            }

            var profil = await this._profileRepository.GetById(profilId) ?? throw new EntityNotFoundException("Profil Not Found");
            var group = await this._groupRepository.GetById(profilId) ?? throw new EntityNotFoundException("Group Not Found");

            if (profil.Id != group.AdminId)
            {
                throw new UnauthorizedAccessException("You Dont Have The Right To Update This Group Cordinates");
            }

            group.Modify(profil.UserId, updateGroupRequest.Name, updateGroupRequest.Description);
            await this._groupRepository.Update(group);
            return new UpdateGroupResponse
            {
                Id = group.Id,
                Name = group.Name,
                Description = group.Description,
                UpdatedAt = group.LastModifiedAt
            };
        }

        public async Task<DeleteGroupResponse> DeleteGroup(Guid profilId, Guid groupId)
        {
            var profil = await this._profileRepository.GetById(profilId) ?? throw new EntityNotFoundException("Profil Not Found");
            var group = await this._groupRepository.GetById(profilId) ?? throw new EntityNotFoundException("Group Not Found");
            if (profil.Id != group.AdminId)
            {
                throw new UnauthorizedAccessException("You Dont Have The Right To Delete This Group Cordinates");
            }

            group.Delete(profil.UserId);
            return new DeleteGroupResponse
            {
                Id = group.Id,
                Name = group.Name,
                DeletedAt = group.DeletedAt,
            };
        }
    }
}
