using Educonnect.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Domain.Entities
{
    public class Profile : Auditable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty; 
        public int ReactionCount { get; set; } = 0;
        public int PostCount { get; set; } = 0;
        public User User { get; set; }
        public Guid UserId { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public virtual IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public virtual IEnumerable<Group> Groups { get; set; } = new List<Group>();
        public bool IsActive { get; set; } = true;
        public DateTime? SuspendedAt { get; set; } = null;
        public DateTime? SuspendedUntil { get;set; } = null;
        public Guid? SuspendedBy { get; set; } = null;

        public void Update(string username, string description, Guid userId)
        {
            Username = username;
            Description = description;
            Modify(userId);
        }
        public override void Delete(Guid userId)
        {
            base.Delete(userId);
        }

        public void SuspendUser(DateTime? Until, Guid AdminId)
        {
            this.IsActive = false; 
            this.SuspendedAt = DateTime.Now;
            this.SuspendedUntil = Until ?? null;
            this.SuspendedBy = AdminId;
            this.User.LockoutEnabled = true;
            this.User.LockoutEnd = Until ?? null;
        }
    }
}
