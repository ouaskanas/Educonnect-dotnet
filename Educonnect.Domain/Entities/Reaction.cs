using Educonnect.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Domain.Entities
{
    public class Reaction
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public ReactionType ReactionType { get; set; } = ReactionType.None;
        public virtual Profile Profile { get; set; }
        public Guid ProfileId { get; set; }
        public virtual Post? Post { get; set; }
        public Guid? PostId { get; set; }
        public virtual Comment? Comment { get; set; }
        public Guid? CommentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } 

    }
}
