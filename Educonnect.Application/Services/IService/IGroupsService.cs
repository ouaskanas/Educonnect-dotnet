using Educonnect.Application.Dtos.GroupDto;
using Educonnect.Common.Pagination.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Services.IService
{
    public interface IGroupsService
    {
        Task<GetGroupDto> GetGroup(Guid groupId, Guid profilId);
        Task<List<GetGroupDto>> GetGroups(Guid profilId, PaginationParameters pagination);
        Task<CreateGroupResponse> CreateGroup(Guid profilId, CreateGroupRequest createGroupRequest);
        Task<AddParticipantesResponse> AddParticipantesToGroup(Guid groupId, Guid adminProfileId, List<Guid> memberIds);
        Task<UpdateGroupResponse> UpdateGroup(Guid profilId, Guid groupId, UpdateGroupRequest updateGroupRequest);
        Task<DeleteGroupResponse> DeleteGroup(Guid profilId, Guid groupId);
    }
}
