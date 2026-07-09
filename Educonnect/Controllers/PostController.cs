using Educonnect.Api.Controllers;
using Educonnect.Application.Dtos.PostDto;
using Educonnect.Application.Services.IService;
using Educonnect.Common.Pagination.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Educonnect.Controllers
{
    [Authorize(Roles = "User")]
    public class PostController : ApiControllerBase
    {
        
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PostCreationRequest request)
        {
            var result = await _postService.CreatePost(request, CurrentProfileId);
            return Ok(result);
        }

        [HttpPost("group/{groupId:guid}")]
        public async Task<IActionResult> CreateForGroup([FromBody] PostCreationRequest request, Guid groupId)
        {
            var result = await _postService.CreatePostForGroup(request, CurrentProfileId, groupId);
            return Ok(result);
        }

        [HttpGet("{postId:guid}")]
        [AllowAnonymous]
        public async Task<IActionResult> Get(Guid postId)
        {
            var post = await _postService.GetPost(postId, CurrentProfileId);
            return Ok(post);
        }

        [HttpGet("feed")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeed([FromQuery] PaginationParameters pagination)
        {
            var posts = await _postService.GetPosts(pagination, CurrentProfileId);
            return Ok(posts);
        }

        [HttpPut("{postId:guid}")]
        public async Task<IActionResult> Update([FromBody] UpdatePostRequest request, Guid postId)
        {
            var result = await _postService.UpdatePost(request, postId, CurrentProfileId);
            return Ok(result);
        }

        [HttpDelete("{postId:guid}")]
        public async Task<IActionResult> SoftDelete(Guid postId)
        {
            await _postService.SoftdeletePost(postId, CurrentProfileId);
            return NoContent();
        }

        [HttpGet("{key}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetPostByKey(string key)
        {
            var posts = await _postService.GetPostByKey(key);
            return Ok(posts);
        }

    }
}
