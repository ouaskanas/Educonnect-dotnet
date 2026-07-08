using Educonnect.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Dtos.ReactionDto
{
    public class PostReactionDto
    {
        public Guid ReactionId { get; set; }
        public ReactionType ReactionType { get; set; }
        public Guid ProfileId { get; set; }
        public string ProfileName { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
    }
}
