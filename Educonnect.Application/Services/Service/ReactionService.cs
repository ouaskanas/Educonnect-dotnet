using Educonnect.Application.Dtos.ReactionDto;
using Educonnect.Application.Services.IService;
using Educonnect.Common.Exceptions;
using Educonnect.Common.Pagination.Dto;
using Educonnect.Domain.Entities;
using Educonnect.Domain.Enums;
using Educonnect.Infrastructure.Repositories.IRepository;
using Educonnect.Infrastructure.Repositories.Repository;
using Microsoft.AspNetCore.Http.HttpResults;
using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Application.Services.Service
{
    public class ReactionService : IReactionService
    {
        private readonly IReactionRepository reactionRepository;
        private readonly IProfileRepository profileRepository;
        private readonly ICommentRepository commentRepository;
        private readonly IPostRepository postRepository;

        public ReactionService(IReactionRepository reactionRepository, IProfileRepository profileRepository, ICommentRepository commentRepository, IPostRepository postRepository)
        {
            this.reactionRepository = reactionRepository;
            this.profileRepository = profileRepository;
            this.commentRepository = commentRepository;
            this.postRepository = postRepository;
        }
        public async Task<CommentReactionDto> DisLikeComment(Guid profileId, Guid commentId)
        {
            var profile = await profileRepository.GetById(profileId) ?? throw new EntityNotFoundException("Profile Not Found");
            var comment = await commentRepository.GetById(commentId) ?? throw new EntityNotFoundException("Comment Not Found");

            var existing = await reactionRepository.GetReactionByCommentAndProfileAsync(commentId, profileId);
            Reaction reaction;

            if (existing != null)
            {
                if (existing.ReactionType == ReactionType.Dislike)
                    throw new Exception("Comment Already DisLiked");

                existing.ReactionType = ReactionType.Dislike;
                existing.CreatedAt = DateTime.Now;
                await reactionRepository.Update(existing);
                reaction = existing;
            }
            else
            {
                reaction = comment.DislikeComment(profile);
                await reactionRepository.Add(reaction);
            }

            return new CommentReactionDto
            {
                ReactionId = reaction.Id,
                CommentId = comment.Id,
                ReactionType = reaction.ReactionType,
                ProfileId = profileId,
                ProfileName = profile.Username,
                CreatedAt = reaction.CreatedAt
            };
        }

        public async Task<PostReactionDto> DisLikePost(Guid profileId, Guid postId)
        {
            var profile = await profileRepository.GetById(profileId) ?? throw new EntityNotFoundException("Profile Not Found");
            var post = await postRepository.GetById(postId) ?? throw new EntityNotFoundException("Post Not Found");

            var existing = await reactionRepository.GetReactionByPostAndProfileAsync(postId, profileId);
            Reaction reaction;
            if (existing != null)
            {
                if (existing.ReactionType == ReactionType.Dislike)
                    throw new Exception("Post Already DisLiked");

                existing.ReactionType = ReactionType.Dislike;
                existing.CreatedAt = DateTime.Now;
                await reactionRepository.Update(existing);
                reaction = existing;
            }
            else
            {
                reaction = post.DisLikePost(profile);
                await reactionRepository.Add(reaction);
            }

            return new PostReactionDto
            {
                ReactionId = reaction.Id,
                PostId = post.Id,
                ReactionType = reaction.ReactionType,
                ProfileId = profileId,
                ProfileName = profile.Username,
                CreatedAt = reaction.CreatedAt
            };
        }

        public async Task<List<PostReactionDto>> GetPostReactions(Guid postId, PaginationParameters pagination, Guid? profileId)
        {
            _ = await postRepository.GetById(postId) ?? throw new EntityNotFoundException("Post Not Found");
            var reactionsPaged = await reactionRepository.GetReactionsByPostAsync(postId, pagination, profileId);
            return reactionsPaged.Data.Select(r => new PostReactionDto
            {
                ReactionId = r.Id,
                PostId = postId,
                ReactionType = r.ReactionType,
                ProfileId = r.ProfileId,
                ProfileName = r.Profile?.Username ?? "Anonyme",
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<List<CommentReactionDto>> GetCommentReactions(Guid commentId, PaginationParameters pagination, Guid? profileId)
        {
            _ = await commentRepository.GetById(commentId) ?? throw new EntityNotFoundException("Comment Not Found");
            var reactionsPaged = await reactionRepository.GetReactionsByCommentAsync(commentId, pagination, profileId);
            return reactionsPaged.Data.Select(r => new CommentReactionDto 
            {
                ReactionId = r.Id,
                CommentId = commentId,
                ReactionType = r.ReactionType,
                ProfileId = r.ProfileId,
                ProfileName = r.Profile?.Username ?? "Anonyme",
                CreatedAt = r.CreatedAt
            }).ToList();
        }

        public async Task<CommentReactionDto> LikeComment(Guid profileId, Guid commentId)
        {
            var profile = await profileRepository.GetById(profileId) ?? throw new EntityNotFoundException("Profile Not Found");
            var comment = await commentRepository.GetById(commentId) ?? throw new EntityNotFoundException("Comment Not Found");

            var existing = await reactionRepository.GetReactionByCommentAndProfileAsync(commentId, profileId);
            Reaction reaction;

            if (existing != null)
            {
                if (existing.ReactionType == ReactionType.Like)
                    throw new Exception("Comment Already Liked");

                existing.ReactionType = ReactionType.Like;
                existing.CreatedAt = DateTime.Now;
                await reactionRepository.Update(existing);
                reaction = existing;
            }
            else
            {
                reaction = comment.LikeComment(profile);
                await reactionRepository.Add(reaction);
            }

            return new CommentReactionDto
            {
                ReactionId = reaction.Id,
                CommentId = comment.Id,
                ReactionType = reaction.ReactionType,
                ProfileId = profileId,
                ProfileName = profile.Username,
                CreatedAt = reaction.CreatedAt
            };
        }

        public async Task<PostReactionDto> LikePost(Guid profileId, Guid postId)
        {
            var profile = await profileRepository.GetById(profileId) ?? throw new EntityNotFoundException("Profile Not Found");
            var post = await postRepository.GetById(postId) ?? throw new EntityNotFoundException("Post Not Found");

            var existing = await reactionRepository.GetReactionByPostAndProfileAsync(postId, profileId);
            Reaction reaction;
            if (existing != null)
            {
                if (existing.ReactionType == ReactionType.Like)
                    throw new Exception("Post Already Liked");

                existing.ReactionType = ReactionType.Like;
                existing.UpdatedAt = DateTime.Now;
                await reactionRepository.Update(existing);
                reaction = existing;
            }
            else
            {
                reaction = post.LikePost(profile);
                await reactionRepository.Add(reaction);
            }

            return new PostReactionDto
            {
                ReactionId = reaction.Id,
                PostId = post.Id,
                ReactionType = reaction.ReactionType,
                ProfileId = profileId,
                ProfileName = profile.Username,
                CreatedAt = reaction.CreatedAt
            };
        }
    }
}
