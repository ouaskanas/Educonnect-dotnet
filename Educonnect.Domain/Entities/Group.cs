using Educonnect.Domain.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Domain.Entities
{
    public class Group : Auditable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public virtual IEnumerable<Profile> Membres { get; set; } = new List<Profile>();
        public IEnumerable<Guid> MembersIds { get; set; } = new List<Guid>();
        public virtual Profile Admin { get; set; } = new Profile();
        public Guid AdminId { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; } = new List<Post>();
        public List<Guid> PostIds { get; set; } = new List<Guid>();

        public void Modify(Guid userId, string Name, string Description)
        {
            base.Modify(userId);
            this.Name = Name;
            this.Description = Description;
        }

        public override void Delete(Guid userId)
        {
            base.Delete(userId);
        }
    }
}
