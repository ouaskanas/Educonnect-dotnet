using Educonnect.Application.Dtos.ReactionDto;
using Educonnect.Common.Pagination.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Services.IService
{
    public interface IReactionService
    {
        Task<PostReactionDto> LikePost(Guid profileId, Guid postId);
        Task<PostReactionDto> DisLikePost(Guid profileId, Guid postId);
        Task<List<PostReactionDto>> GetPostReactions(Guid postId, PaginationParameters pagination, Guid? profileId);
        Task<List<CommentReactionDto>> GetCommentReactions(Guid postId, PaginationParameters pagination, Guid? profileId);
        Task<CommentReactionDto> LikeComment(Guid profileId, Guid commentId);
        Task<CommentReactionDto> DisLikeComment(Guid profileId, Guid commentId);
    }
}
