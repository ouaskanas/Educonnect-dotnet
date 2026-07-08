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
        public int ReactionCount { get; set; } = 0;
        public IEnumerable<Reaction> Reactions { get; set; } = new List<Reaction>();
        public virtual Profile Author { get; set; }
        public Guid AuthorId { get; set; }
        public virtual Post? Post { get; set; }
        public Guid PostId { get; set; }
        public Guid FatherCommentId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt {  get; set; }
        public Guid? UpdateBy {  get; set; }
        /// <summary>
        /// Deletable Attributes 
        /// </summary>
        public DateTime? DeletedAt { get; set; }
        public bool IsDeleted { get; set; }
        public Guid? DeletedBy { get; set; }

        public void UpdateComment(string Comment, Guid userId)
        {
            UpdateBy = userId;
            UpdatedAt = DateTime.Now; 
            Content = Comment;
        }

        public void DeleteComment(Guid userId)
        {
            DeletedAt = DateTime.Now;
            IsDeleted = true; 
            DeletedBy = userId;
        }
    }
}
