using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.CommentDto
{
    public class UpdateCommentResponse
    {
        public Guid CommentId { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public Guid AuthorId {  get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public Guid PostId { get; set; }
    }
}
