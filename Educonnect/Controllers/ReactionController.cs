using Educonnect.Api.Controllers;
using Educonnect.Application.Dtos.ReactionDto;
using Educonnect.Application.Services.IService;
using Educonnect.Common.Pagination.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Educonnect.Controllers
{
    [Authorize(Roles = "User")]
    public class ReactionController : ApiControllerBase
    {
        private readonly IReactionService _reactionService;

        public ReactionController(IReactionService reactionService)
        {
            _reactionService = reactionService;
        }

        [HttpPost("post/{postId:guid}/like")]
        public async Task<IActionResult> LikePost(Guid postId)
        {
            var result = await _reactionService.LikePost(CurrentProfileId, postId);
            return Ok(result);
        }

        [HttpPost("post/{postId:guid}/dislike")]
        public async Task<IActionResult> DisLikePost(Guid postId)
        {
            var result = await _reactionService.DisLikePost(CurrentProfileId, postId);
            return Ok(result);
        }

        [HttpPost("comment/{commentId:guid}/like")]
        public async Task<IActionResult> LikeComment(Guid commentId)
        {
            var result = await _reactionService.LikeComment(CurrentProfileId, commentId);
            return Ok(result);
        }

        [HttpPost("comment/{commentId:guid}/dislike")]
        public async Task<IActionResult> DisLikeComment(Guid commentId)
        {
            var result = await _reactionService.DisLikeComment(CurrentProfileId, commentId);
            return Ok(result);
        }

        [HttpGet("post/{postId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostReactions(Guid postId, [FromQuery] PaginationParameters pagination)
        {
            var reactions = await _reactionService.GetPostReactions(postId, pagination, CurrentProfileId);
            return Ok(reactions);
        }

        [HttpGet("comment/{commentId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentReactions(Guid commentId, [FromQuery] PaginationParameters pagination)
        {
            var reactions = await _reactionService.GetCommentReactions(commentId, pagination, CurrentProfileId);
            return Ok(reactions);
        }
    }
}