using Educonnect.Application.Dtos.ReactionDto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.CommentDto
{
    public class CommentResponseDto
    {
        public Guid Id { get; set; }
        public string Content { get; set; } = string.Empty;
        public int LikeCount { get; set; }
        public int DisLikeCount { get; set; }
        public Guid AuthorId { get; set; }
        public Guid CommentId { get; set; }
    }
}
