using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.PostDto
{
    public class UpdatePostResponse
    {
        public Guid Id { get; set; }
        public string PostTitle { get; set; } = string.Empty; 
        public string PostBody { get; set; } = string.Empty;
        public DateTime? UpdatedAt { get; set; }
        public string UpdatedBy { get; set; } = string.Empty;
        public Guid? GroupId { get; set; }
        public Guid? AuthorId { get; set; }
    }
}
