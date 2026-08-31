using Educonnect.Domain.Common;
using Educonnect.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Domain.Entities
{
    public class Post : Auditable
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Title { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
        public virtual IEnumerable<Comment> Comments { get; set; } = new List<Comment>();
        public IEnumerable<Reaction> Reactions { get; set; } = new List<Reaction>();
        public Profile Author { get; set; } = new Profile();
        public Guid AuthorId { get; set; }
        public virtual Group? Group { get; set; } = null;
        public Guid? GroupId { get; set; } = null;

        public void Update(string title, string body, Guid userId)
        {
            Title = title;
            Body = body;
            Modify(userId);
        }
        public override void Delete(Guid userId)
        {
            base.Delete(userId);
        }

        public ReactionType HasReacted(Guid profileId)
        {
            return this.Reactions
                .FirstOrDefault(r=> r.ProfileId == profileId)?
                .ReactionType ?? ReactionType.None;
        }

        public Reaction LikePost(Profile profile)
        {
            var reaction = new Reaction
            {
                ReactionType = ReactionType.Like,
                Profile = profile,
                ProfileId = profile.Id,
                Post = this,
                PostId = this.Id,
                Comment = null,
                CommentId = null,
                CreatedAt = DateTime.Now,
            };
            if (this.Reactions is List<Reaction> localList)
            {
                localList.Add(reaction);
            }

            return reaction;
        }
        public Reaction DisLikePost(Profile profile)
        {
            var reaction = new Reaction
            {
                ReactionType = ReactionType.Dislike,
                Profile = profile,
                ProfileId = profile.Id,
                Post = this,
                PostId = this.Id,
                Comment = null,
                CommentId = null,
                CreatedAt = DateTime.Now,
            };
            if (this.Reactions is List<Reaction> localList)
            {
                localList.Add(reaction);
            }

            return reaction;
        }
        // todo : fix the filter
        public bool HasLiked(Guid profileId)
        {
            return this.Reactions.Any(r => r.ProfileId == profileId && r.ReactionType == ReactionType.Like);
        }

        public bool HasDisLiked(Guid profileId)
        {
            return this.Reactions.Any(r => r.ProfileId == profileId && r.ReactionType == ReactionType.Dislike);
        }
    }
}
