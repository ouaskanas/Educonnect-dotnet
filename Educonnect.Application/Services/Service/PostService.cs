using Educonnect.Application.Dtos.CommentDto;
using Educonnect.Application.Dtos.PostDto;
using Educonnect.Application.Dtos.ReactionDto;
using Educonnect.Application.Services.IService;
using Educonnect.Common.Exceptions;
using Educonnect.Common.Pagination.Dto;
using Educonnect.Domain.Entities;
using Educonnect.Infrastructure.Repositories.IRepository;
using Educonnect.Infrastructure.Repositories.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Educonnect.Application.Services.Service
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly UserManager<User> _userManager;
        private readonly ICommentRepository _commentRepository;
        private readonly IReactionRepository _reactionRepository;
        private readonly IProfileRepository _profileRepository;

        public PostService(IPostRepository postRepository, UserManager<User> userManager, ICommentRepository commentRepository, IReactionRepository reactionRepository, IProfileRepository profileRepository)
        {
            _postRepository = postRepository;
            _userManager = userManager;
            _commentRepository = commentRepository;
            _reactionRepository = reactionRepository;
            _profileRepository = profileRepository;
        }

        public async Task<PostCreationResponse> CreatePost(PostCreationRequest postCreationRequest, string Id)
        {
            var user = await this._userManager.FindByIdAsync(Id) ?? throw new EntityNotFoundException("User not found");
            if (user.Profile == null)
            {
                throw new EntityNotFoundException("Profile not found");
            }
            var post = new Post
            {
                Title = postCreationRequest.Title,
                Body = postCreationRequest.Body,
                Author = user.Profile, 
                AuthorId = user.ProfilId,
            };
            await _postRepository.Add(post);
            return new PostCreationResponse
            {
                Body = post.Body,
                Title = postCreationRequest.Title,
                CreatedAt = post.CreatedAt,
            };
        }

        public async Task<PostResponseDto> GetPost(Guid postId)
        {
            var post = await this._postRepository.GetById(postId) ?? throw new EntityNotFoundException("Post Not found");
            var comments = await this._commentRepository.GetCommentsByPostAsync(postId, null);
            var reactions = await this._reactionRepository.GetReactionsByPostAsync(postId, null);

            return new PostResponseDto
            {
                PostId = post.Id,
                PostTitle = post.Title,
                PostBody = post.Body,
                PostDate = post.CreatedAt,
                AuthorId = post.Author.Id,
                AuthorName = post.Author.Username,
                reactionCount = post.Reactions.Count(),
                LikeCount = post.Reactions.Where(r => r.ReactionType == Domain.Enums.ReactionType.Like).Count(),
                DisLikeCount = post.Reactions.Where(r => r.ReactionType == Domain.Enums.ReactionType.Dislike).Count(),
                Comments = comments.Data.Select(c => new PostCommentDto
                {
                    CommentId = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    AuthorId = c.AuthorId,
                    AuthorName = c.Author?.Username ?? "Anonyme"
                }).ToList(),

                Reactions = reactions.Data.Select(r => new PostReactionDto
                {
                    ReactionId = r.Id,
                    ReactionType = r.ReactionType,
                    CreatedAt = r.CreatedAt,
                    ProfileId = r.ProfileId,
                    ProfileName = r.Profile.Username,
                }).ToList(),
                UserId = post.Author.Id
            };
        }

        public async Task<List<PostResponseDto>> GetPosts(PaginationParameters pagination, Guid profileId)
        {
            var user = await this._profileRepository.GetById(profileId) ?? throw new EntityNotFoundException("User Not Found");
            var posts = await this._postRepository.GetFeedAsync(pagination, profileId);
            return posts.Data.Select(p => new PostResponseDto
            {
                PostId = p.Id,
                PostTitle = p.Title,
                PostBody = p.Body,
                PostDate = p.CreatedAt,
                AuthorId = p.Author.Id,
                AuthorName = p.Author.Username,
                reactionCount = p.Reactions.Count(),
                LikeCount = p.Reactions.Where(r => r.ReactionType == Domain.Enums.ReactionType.Like).Count(),
                DisLikeCount = p.Reactions.Where(r => r.ReactionType == Domain.Enums.ReactionType.Dislike).Count(),
                Comments = p.Comments.Select(c => new PostCommentDto
                {
                    CommentId = c.Id,
                    Content = c.Content,
                    CreatedAt = c.CreatedAt,
                    AuthorId = c.AuthorId,
                    AuthorName = c.Author?.Username ?? "Anonyme"
                }).ToList(),
                Reactions = null,
                UserId = p.Author.Id
            }).ToList();
        }
    }
}
