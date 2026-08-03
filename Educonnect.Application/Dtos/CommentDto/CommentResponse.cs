using Educonnect.Application.Dtos.ReactionDto;
using Educonnect.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.CommentDto
{
    public class CommentResponse
    {
        public Guid Id;
        public string Comment { get; set; } = string.Empty;
        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }
        public List<PostReactionDto> reactions { get; set; } = null;
        public List<CommentResponseDto> CommentResponses { get; set; } = null;
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public Guid PostId { get; set; }
        public ReactionType MyReaction {  get; set; }

    }
}
