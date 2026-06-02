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
        public virtual Profile Profile { get; set; } = new Profile();
        public Guid ProfileId { get; set; }
        public virtual Post? Post { get; set; }
        public Guid? PostId { get; set; }
        public virtual Comment? Comment { get; set; }
        public Guid? CommentId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; } 

        public void LikePost(Post post, Profile profile)
        {
            this.ReactionType = ReactionType.Like; 
            this.Profile = profile;
            this.ProfileId = profile.Id;
            this.Post = post;
            this.PostId = post.Id;
        }

        public void DislikePost(Post post, Profile profile) 
        {
            this.ReactionType = ReactionType.Dislike;
            this.Profile = profile;
            this.ProfileId = profile.Id;
            this.Post = post;
            this.PostId = post.Id;
        }

        public void LikeComment(Comment comment, Profile profile)
        {
            this.ReactionType = ReactionType.Like;
            this.Profile = profile;
            this.ProfileId = profile.Id;
            this.Comment = comment;
            this.CommentId = comment.Id;
        }

        public void DislikeComment(Comment comment, Profile profile)
        {
            this.ReactionType = ReactionType.Dislike;
            this.Profile = profile;
            this.ProfileId = profile.Id;
            this.Comment = comment;
            this.CommentId = comment.Id;
        }

    }
}
