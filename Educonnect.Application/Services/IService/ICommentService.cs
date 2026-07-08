using Educonnect.Application.Dtos;
using Educonnect.Application.Dtos.CommentDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Services.IService
{
    public interface ICommentService
    {
        Task<CreateCommentResponse> CreateCommentForPost(Guid profilId, Guid postId, string content);
        Task<CommentResponse> GetComment(Guid commentId);
        Task<List<CommentResponse>> GetCommentFromPost(Guid postId);
        Task<UpdateCommentResponse> UpdateComment(Guid profilId, Guid commentId, string Content);
        Task<bool> SoftDeleteComment(Guid commentId, Guid profilId);
        Task<CommentResponseDto> AnswerToComment(Guid commentId, Guid profilId, string Content);
    }
}
