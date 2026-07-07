using Educonnect.Application.Dtos.PostDto;
using Educonnect.Common.Pagination.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Services.IService
{
    public interface IPostService
    {
        Task<PostCreationResponse> CreatePost(PostCreationRequest postCreationRequest, string Id);
        Task<PostResponseDto> GetPost(Guid postId);
        Task<List<PostResponseDto>> GetPosts(PaginationParameters pagination, Guid profileId);
    }
}
