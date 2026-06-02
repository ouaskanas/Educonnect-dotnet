using Educonnect.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Domain.Entities
{
    public class Group : IDeletable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        // uniq
        public string GroupId { get; set; } = string.Empty;
        public virtual IEnumerable<Profile> Memebres { get; set; } = Enumerable.Empty<Profile>();
        public virtual Profile Admin { get; set; } = new Profile();
        public Guid AdminId { get; set; }
        public virtual IEnumerable<Post> Posts { get; set; } = Enumerable.Empty<Post>(); 
        public Guid PostId { get; set; }

        /// <summary>
        /// Deletable Attributes 
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
