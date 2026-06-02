using Educonnect.Domain.Enums;
using Educonnect.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Domain.Entities
{
    public class Comment : IDeletable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Content { get; set; } = string.Empty;
        public IEnumerable<Reaction> Reactions { get; set; } = Enumerable.Empty<Reaction>();
        public virtual Profile Author { get; set; } = new Profile();
        public Guid AuthorId { get; set; }
        public virtual Post Post { get; set; } = new Post();
        public Guid PostId { get; set; }
        /// <summary>
        /// Deletable Attributes 
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }
    }
}
