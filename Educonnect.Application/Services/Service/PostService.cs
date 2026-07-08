using Educonnect.Application.Dtos.CommentDto;
using Educonnect.Application.Dtos.PostDto;
using Educonnect.Application.Dtos.ReactionDto;
using Educonnect.Application.Services.IService;
using Educonnect.Common.Exceptions;
using Educonnect.Common.Pagination.Dto;
using Educonnect.Domain.Entities;
using Educonnect.Infrastructure.Repositories.IRepository;
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
        private readonly IGroupRepository groupRepository;
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

        public async Task<PostCreationResponse> CreatePost(PostCreationRequest postCreationRequest, string userId)
        {
            var user = await this._userManager.FindByIdAsync(userId) ?? throw new EntityNotFoundException("User not found");
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

        public async Task<PostCreationResponse> CreatePostForGroup(PostCreationRequest postCreationRequest, string userId, Guid groupId)
        {
            var user = await this._userManager.FindByIdAsync(userId) ?? throw new EntityNotFoundException("User not found");
            var group = await this.groupRepository.GetById(groupId) ?? throw new EntityNotFoundException("Group not found");
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
                GroupId = groupId,
                Group = group
            };

            await _postRepository.Add(post);
            return new PostCreationResponse
            {
                Body = post.Body,
                Title = postCreationRequest.Title,
                CreatedAt = post.CreatedAt,
                GroupId = groupId
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

        public async Task<bool> SoftdeletePost(Guid postId, string userId)
        {
            var user = await this._userManager.FindByIdAsync(userId) ?? throw new EntityNotFoundException("User not found");
            var post = await this._postRepository.GetById(postId) ?? throw new EntityNotFoundException("Post not found");
            if (user.Profile == null || post.AuthorId != user.ProfilId)
            {
                throw new UnauthorizedException("You dont have access to this ressource");
            }

            post.DeletePost(Guid.Parse(userId)); 
            await this._postRepository.Update(post);
            return true;
        }

        public async Task<UpdatePostResponse> UpdatePost(UpdatePostRequest updatePostRequest, Guid postId, string userId)
        {
            var user = await this._userManager.FindByIdAsync(userId) ?? throw new EntityNotFoundException("User not found");
            var post = await this._postRepository.GetById(postId) ?? throw new EntityNotFoundException("Post not found");
            if (user.Profile ==  null || post.AuthorId != user.ProfilId)
            {
                throw new UnauthorizedException("You dont have access to this ressource");
            }
            post.UpdatePost(updatePostRequest.PostTitle, updatePostRequest.PostBody, Guid.Parse(userId));
            await this._postRepository.Update(post);
            return new UpdatePostResponse
            {
                Id = postId,
                PostTitle = post.Title,
                PostBody = post.Body,
                UpdatedAt = post.UpdatedAt,
                UpdatedBy = user.Profile.Username,
                GroupId = post.GroupId ?? null,
                AuthorId = user.ProfilId,
            };
        }
    }
}
