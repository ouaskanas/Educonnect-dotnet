using Educonnect.Application.Dtos.CommentDto;
using Educonnect.Application.Dtos.PostDto;
using Educonnect.Application.Dtos.ReactionDto;
using Educonnect.Application.Services.IService;
using Educonnect.Common.Exceptions;
using Educonnect.Common.Pagination.Dto;
using Educonnect.Domain.Entities;
using Educonnect.Infrastructure.Repositories.IRepository;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Educonnect.Application.Services.Service;

public class PostService : IPostService
{
    private readonly IPostRepository _postRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly ICommentRepository _commentRepository;
    private readonly IReactionRepository _reactionRepository;
    private readonly IProfileRepository _profileRepository;

    public PostService(
        IPostRepository postRepository,
        IGroupRepository groupRepository,
        ICommentRepository commentRepository,
        IReactionRepository reactionRepository,
        IProfileRepository profileRepository)
    {
        _postRepository = postRepository;
        _groupRepository = groupRepository;
        _commentRepository = commentRepository;
        _reactionRepository = reactionRepository;
        _profileRepository = profileRepository;
    }

    public async Task<PostCreationResponse> CreatePost(PostCreationRequest postCreationRequest, Guid profileId)
    {
        var profile = await _profileRepository.GetById(profileId)
            ?? throw new EntityNotFoundException("Profile not found");

        var post = new Post
        {
            Title = postCreationRequest.Title,
            Body = postCreationRequest.Body,
            Author = profile,
            AuthorId = profileId
        };

        await _postRepository.Add(post);

        return new PostCreationResponse
        {
            Body = post.Body,
            Title = postCreationRequest.Title,
            CreatedAt = post.CreatedAt,
        };
    }

    public async Task<PostCreationResponse> CreatePostForGroup(PostCreationRequest postCreationRequest, Guid profileId, Guid groupId)
    {
        var profile = await _profileRepository.GetById(profileId)
            ?? throw new EntityNotFoundException("Profile not found");

        var group = await _groupRepository.GetById(groupId)
            ?? throw new EntityNotFoundException("Group not found");

        var post = new Post
        {
            Title = postCreationRequest.Title,
            Body = postCreationRequest.Body,
            Author = profile,
            AuthorId = profileId,
            GroupId = groupId,
            Group = group
        };
        profile.PostCount++;
        await _profileRepository.Update(profile);
        await _postRepository.Add(post);

        return new PostCreationResponse
        {
            Body = post.Body,
            Title = postCreationRequest.Title,
            CreatedAt = post.CreatedAt,
            GroupId = groupId
        };
    }

    public async Task<List<PostResponseDto>> GetPostByKey(string key)
    {
        var posts = await this._postRepository.GetPostByName(key);
        return posts.Select(post =>new PostResponseDto
        {
            PostId = post.Id,
            PostTitle = post.Title,
            PostBody = post.Body,
            PostDate = post.CreatedAt,
            AuthorId = post.Author.Id,
            AuthorName = post.Author.Username,
            reactionCount = post.Reactions.Count(),
            LikeCount = post.Reactions.Count(r => r.ReactionType == Domain.Enums.ReactionType.Like),
            DisLikeCount = post.Reactions.Count(r => r.ReactionType == Domain.Enums.ReactionType.Dislike),
            Comments = post.Comments.Select(c => new PostCommentDto
            {
                CommentId = c.Id,
                Content = c.Content,
                CreatedAt = c.CreatedAt,
                AuthorId = c.AuthorId,
                AuthorName = c.Author?.Username ?? "Anonyme"
            }).ToList(),
            Reactions = post.Reactions.Select(r => new PostReactionDto
            {
                ReactionId = r.Id,
                ReactionType = r.ReactionType,
                CreatedAt = r.CreatedAt,
                ProfileId = r.ProfileId,
                ProfileName = r.Profile.Username,
            }).ToList(),
            UserId = post.Author.Id
        }).ToList();
    }

    public async Task<PostResponseDto> GetPost(Guid postId)
    {
        var post = await _postRepository.GetById(postId) ?? throw new EntityNotFoundException("Post Not found");
        var comments = await _commentRepository.GetCommentsByPostAsync(postId, null);
        var reactions = await _reactionRepository.GetReactionsByPostAsync(postId, null);

        return new PostResponseDto
        {
            PostId = post.Id,
            PostTitle = post.Title,
            PostBody = post.Body,
            PostDate = post.CreatedAt,
            AuthorId = post.Author.Id,
            AuthorName = post.Author.Username,
            reactionCount = post.Reactions.Count(),
            LikeCount = post.Reactions.Count(r => r.ReactionType == Domain.Enums.ReactionType.Like),
            DisLikeCount = post.Reactions.Count(r => r.ReactionType == Domain.Enums.ReactionType.Dislike),
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
        _ = await _profileRepository.GetById(profileId) ?? throw new EntityNotFoundException("User Not Found");
        var posts = await _postRepository.GetFeedAsync(pagination, profileId);

        return posts.Data.Select(p => new PostResponseDto
        {
            PostId = p.Id,
            PostTitle = p.Title,
            PostBody = p.Body,
            PostDate = p.CreatedAt,
            AuthorId = p.Author.Id,
            AuthorName = p.Author.Username,
            reactionCount = p.Reactions.Count(),
            LikeCount = p.Reactions.Count(r => r.ReactionType == Domain.Enums.ReactionType.Like),
            DisLikeCount = p.Reactions.Count(r => r.ReactionType == Domain.Enums.ReactionType.Dislike),
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

    public async Task<bool> SoftdeletePost(Guid postId, Guid profileId)
    {
        var post = await _postRepository.GetById(postId) ?? throw new EntityNotFoundException("Post not found");

        // Simple ownership verification directly inside business logic
        if (post.AuthorId != profileId)
        {
            throw new UnauthorizedException("You dont have access to this ressource");
        }

        post.DeletePost(profileId);
        await _postRepository.Update(post);
        return true;
    }

    public async Task<UpdatePostResponse> UpdatePost(UpdatePostRequest updatePostRequest, Guid postId, Guid profileId)
    {
        var post = await _postRepository.GetById(postId) ?? throw new EntityNotFoundException("Post not found");

        if (post.AuthorId != profileId)
        {
            throw new UnauthorizedException("You dont have access to this ressource");
        }

        post.UpdatePost(updatePostRequest.PostTitle, updatePostRequest.PostBody, profileId);
        await _postRepository.Update(post);

        return new UpdatePostResponse
        {
            Id = postId,
            PostTitle = post.Title,
            PostBody = post.Body,
            UpdatedAt = post.UpdatedAt,
            UpdatedBy = post.Author.Username,
            GroupId = post.GroupId,
            AuthorId = profileId,
        };
    }
}