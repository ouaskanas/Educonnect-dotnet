using Educonnect.Api.Controllers;
using Educonnect.Application.Services.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace Educonnect.Controllers
{
    [Authorize(Roles = "User")]
    public class CommentController : ApiControllerBase
    {
        private readonly ICommentService commentService;
        public CommentController(ICommentService commentService)
        {
            this.commentService = commentService;
        }

        [HttpPost("create/{postId:guid}")]
        public async Task<IActionResult> CreateComment([FromBody] string comment,Guid postId)
        {
            var response =  await this.commentService.CreateCommentForPost(CurrentProfileId, postId, comment);
            return Ok(response);
        }

        [HttpGet("getfrompost/{postId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCommentsFromPost(Guid postId)
        {
            var response =  await this.commentService.GetCommentFromPost(postId, CurrentProfileId);
            return Ok(response);
        }

        [HttpGet("get/{commentId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetComment(Guid commentId)
        {
            var response = await this.commentService.GetComment(commentId, CurrentProfileId);
            return Ok(response);
        }

        [HttpPost("answer/{commentId:guid}")]
        public async Task<IActionResult> AnswerComment([FromBody] string comment, Guid commentId)
        {
            var response = await this.commentService.AnswerToComment(commentId, CurrentProfileId, comment);
            return Ok(response);
        }

        [HttpPut("update/{commentId:guid}")]
        public async Task<IActionResult> UpdateComment([FromBody] string comment,Guid commentId)
        {
            var response = await this.commentService.UpdateComment(CurrentProfileId, commentId, comment);
            return Ok(response);
        }

        [HttpDelete("delete/{commentId:guid}")]
        public async Task<IActionResult> DeleteComment(Guid commentId)
        {
            var response = await this.commentService.SoftDeleteComment(commentId,CurrentProfileId);
            return Ok(response);
        }
    }
}
