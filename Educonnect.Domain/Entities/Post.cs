using Educonnect.Domain.Enums;
using Educonnect.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Domain.Entities
{
    public class Post : IDeletable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public virtual IEnumerable<Comment> Comments { get; set; } = Enumerable.Empty<Comment>();
        public IEnumerable<Reaction> Reactions { get; set; } = Enumerable.Empty<Reaction>();
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } = null;
        public Guid? UpdatedBy { get; set; } = null;
        public Profile Author { get; set; } = new Profile();
        public Guid AuthorId { get; set; }
        public virtual Group? Group { get; set; } = null;
        public Guid? GroupId { get; set; } = null;

        /// <summary>
        /// Deletable Attributes 
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }

        public void UpdatePost(string Title, string Body, Guid userId)
        {
            this.Title = Title;
            this.Body = Body;
            this.UpdatedAt = DateTime.Now;
            this.UpdatedBy = userId;
        }

        public void DeletePost(Guid userId)
        {
            DeletedAt = DateTime.Now;
            IsDeleted = true;
            DeletedBy = userId;
            // comment & reaction cascade soft delete
        }
    }
}
