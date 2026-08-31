using Educonnect.Api.Controllers;
using Educonnect.Application.Dtos.GroupDto;
using Educonnect.Application.Services.IService;
using Educonnect.Common.Pagination.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Educonnect.Controllers
{
    [Authorize(Roles = "User")]
    public class GroupController : ApiControllerBase
    {
        private readonly IGroupsService _groupsService;

        public GroupController(IGroupsService groupsService)
        {
            _groupsService = groupsService;
        }

        [HttpGet("get/{groupId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGroup(Guid groupId)
        {
            var response = await _groupsService.GetGroup(groupId, CurrentProfileId);
            return Ok(response);
        }

        [HttpGet("getall")]
        [AllowAnonymous]
        public async Task<IActionResult> GetGroups([FromQuery] PaginationParameters pagination)
        {
            var response = await _groupsService.GetGroups(CurrentProfileId, pagination);
            return Ok(response);
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGroup([FromBody] CreateGroupRequest createGroupRequest)
        {
            var response = await _groupsService.CreateGroup(CurrentProfileId, createGroupRequest);
            return Ok(response);
        }

        [HttpPost("add-participants/{groupId:guid}")]
        public async Task<IActionResult> AddParticipants(Guid groupId, [FromBody] List<Guid> memberIds)
        {
            var response = await _groupsService.AddParticipantesToGroup(groupId, CurrentProfileId, memberIds);
            return Ok(response);
        }

        [HttpPut("update/{groupId:guid}")]
        public async Task<IActionResult> UpdateGroup(Guid groupId, [FromBody] UpdateGroupRequest updateGroupRequest)
        {
            var response = await _groupsService.UpdateGroup(CurrentProfileId, groupId, updateGroupRequest);
            return Ok(response);
        }

        [HttpDelete("delete/{groupId:guid}")]
        public async Task<IActionResult> DeleteGroup(Guid groupId)
        {
            var response = await _groupsService.DeleteGroup(CurrentProfileId, groupId);
            return Ok(response);
        }
    }
}