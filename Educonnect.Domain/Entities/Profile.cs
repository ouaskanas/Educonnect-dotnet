using Educonnect.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Domain.Entities
{
    public class Profile : IDeletable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 
        public int ReactionCount { get; set; } = 0;
        public int PostCount { get; set; } = 0;
        public User User { get; set; } = new User();
        public Guid UserId { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; } = Enumerable.Empty<Post>();
        public virtual IEnumerable<Comment> Comments { get; set; } = Enumerable.Empty<Comment>();
        public virtual IEnumerable<Group> Groups { get; set; } = Enumerable.Empty<Group>();

        /// <summary>
        /// Deletable Attributes 
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }

    }
}
