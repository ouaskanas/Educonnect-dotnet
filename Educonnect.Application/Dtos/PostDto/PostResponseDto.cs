using Educonnect.Application.Dtos.CommentDto;
using Educonnect.Application.Dtos.ReactionDto;
using Educonnect.Domain.Entities;
using Educonnect.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.PostDto
{
    public class PostResponseDto
    {
        public Guid PostId { get; set; }
        public string PostTitle { get; set; } = string.Empty;
        public string PostBody { get; set; } = string.Empty;
        public IEnumerable<PostCommentDto> Comments { get; set; } = Enumerable.Empty<PostCommentDto>();
        public int reactionCount { get; set; }
        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }
        public DateTime PostDate { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public Guid AuthorId { get; set; }
        public Guid UserId { get; set; }
        public ReactionType MyReaction {  get; set; }

    }
}
