using Educonnect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.CommentDto
{
    public class PostCommentDto
    {
        public Guid CommentId { get; set; }
        public string Content { get; set; } = string.Empty;
        public IEnumerable<Reaction> Reactions { get; set; } = Enumerable.Empty<Reaction>();
        public Guid AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
