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

        [HttpPost("{postId:guid}")]
        public async Task<IActionResult> CreateComment([FromBody] string comment,Guid postId)
        {
            var response =  await this.commentService.CreateCommentForPost(CurrentProfileId, postId, comment);
            return Ok(response);
        }

        [HttpGet("{postId:guid}")]
        public async Task<IActionResult> GetCommentsFromPost(Guid postId)
        {
            var response =  await this.commentService.GetCommentFromPost(postId);
            return Ok(response);
        }

        [HttpGet("{commentId:guid}")]
        public async Task<IActionResult> GetComment(Guid commentId)
        {
            var response =  await this.commentService.GetComment(commentId);
            return Ok(response);
        }






    }
}
