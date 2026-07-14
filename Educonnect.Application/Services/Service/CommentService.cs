using Educonnect.Application.Dtos.CommentDto;
using Educonnect.Application.Services.IService;
using Educonnect.Common.Exceptions;
using Educonnect.Domain.Entities;
using Educonnect.Infrastructure.Repositories.IRepository;

namespace Educonnect.Application.Services.Service
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IPostRepository _postRepository;
        private readonly IProfileRepository _profileRepository;

        public CommentService(ICommentRepository commentRepository, IPostRepository postRepository, IProfileRepository profileRepository)
        {
            _commentRepository = commentRepository;
            _postRepository = postRepository;
            _profileRepository = profileRepository;
        }

        public async Task<CreateCommentResponse> CreateCommentForPost(Guid profilId, Guid postId, string content)
        {
            var profile = await _profileRepository.GetById(profilId) ?? throw new EntityNotFoundException("Profile not found");
            var post = await _postRepository.GetById(postId) ?? throw new EntityNotFoundException("Post not found");
            var comment = new Comment
            {
                AuthorId = profilId,
                Author = profile,
                Content = content,
                PostId = postId,
                Post = post,
                CreatedAt = DateTime.UtcNow,
                FatherCommentId = null,
            }; 
            await this._commentRepository.Add(comment);
            return new CreateCommentResponse
            {
                Id = comment.Id,
                Content = content,
                PostId = postId,
                AuthorId = profilId,
            };
        }

        public async Task<CommentResponse> GetComment(Guid commentId, Guid? profileId)
        {
            var comment = await _commentRepository.GetById(commentId)
                ?? throw new EntityNotFoundException("Comment not found");

            var repliesPaged = await _commentRepository.GetCommentResponses(commentId, comment.PostId, null);

            var repliesDto = repliesPaged?.Data?.Select(r => new CommentResponseDto
            {
                Id = r.Id,
                Content = r.Content,
                CommentId = comment.Id,
                AuthorId = r.AuthorId,
                LikeCount = r.Reactions?.Count(x => x.ReactionType == Domain.Enums.ReactionType.Like) ?? 0,
                DisLikeCount = r.Reactions?.Count(x => x.ReactionType == Domain.Enums.ReactionType.Dislike) ?? 0,
                MyReaction = r.HasReacted(profileId ?? Guid.Empty)
            }).ToList() ?? new List<CommentResponseDto>();

            return new CommentResponse
            {
                Id = comment.Id,
                Comment = comment.Content,
                LikeCount = comment.Reactions?.Count(c => c.ReactionType == Domain.Enums.ReactionType.Like) ?? 0,
                DisLikeCount = comment.Reactions?.Count(c => c.ReactionType == Domain.Enums.ReactionType.Dislike) ?? 0,
                AuthorId = comment.AuthorId,
                AuthorName = comment.Author?.Username ?? "Unknown",
                PostId = comment.PostId,
                CommentResponses = repliesDto,
                MyReaction = comment.HasReacted(profileId ?? Guid.Empty)
            };
        }

        public async Task<List<CommentResponse>> GetCommentFromPost(Guid postId, Guid? profileId)
        {
            var postExists = await _postRepository.ExistById(postId);
            if (!postExists)
            {
                throw new EntityNotFoundException("Post not found");
            }

            var commentsResult = await _commentRepository.GetCommentsByPostAsync(postId, null);
            var commentsList = commentsResult.Data;

            var commentResponses = new List<CommentResponse>();

            foreach (var c in commentsList)
            {
                var repliesPaged = await _commentRepository.GetCommentResponses(c.Id, postId, null);

                var repliesDto = repliesPaged?.Data?.Select(r => new CommentResponseDto
                {
                    Id = r.Id,
                    Content = r.Content,
                    CommentId = c.Id,
                    AuthorId = r.AuthorId,
                    LikeCount = r.Reactions?.Count(x => x.ReactionType == Domain.Enums.ReactionType.Like) ?? 0,
                    DisLikeCount = r.Reactions?.Count(x => x.ReactionType == Domain.Enums.ReactionType.Dislike) ?? 0,
                    MyReaction = r.HasReacted(profileId ?? Guid.Empty)
                }).ToList() ?? new List<CommentResponseDto>();

                commentResponses.Add(new CommentResponse
                {
                    Id = c.Id,
                    Comment = c.Content,
                    LikeCount = c.Reactions?.Count(r => r.ReactionType == Domain.Enums.ReactionType.Like) ?? 0,
                    DisLikeCount = c.Reactions?.Count(r => r.ReactionType == Domain.Enums.ReactionType.Dislike) ?? 0,
                    AuthorId = c.AuthorId,
                    AuthorName = c.Author?.Username ?? "Unknown",
                    PostId = c.PostId,
                    CommentResponses = repliesDto,
                    MyReaction = c.HasReacted(profileId ?? Guid.Empty)
                });
            }

            return commentResponses;
        }

        public async Task<UpdateCommentResponse> UpdateComment(Guid profilId,Guid commentId, string Content)
        {
            var profil = await _profileRepository.GetById(profilId) ?? throw new EntityNotFoundException("Profil not found");
            var comment = await _commentRepository.GetById(commentId) ?? throw new EntityNotFoundException("Comment not found");
            comment.UpdateComment(Content, profil.UserId);
            await _commentRepository.Update(comment);
            return new UpdateCommentResponse
            {
                CommentId = commentId,
                CommentText = Content,
                AuthorId = profil.UserId,
                AuthorName = profil.Username,
                PostId = comment.Id,
            };
        }

        public async Task<bool> SoftDeleteComment(Guid commentId, Guid profilId)
        {
            var profil = await _profileRepository.GetById(profilId) ?? throw new EntityNotFoundException("Profil not found");
            var comment = await _commentRepository.GetById(commentId) ?? throw new EntityNotFoundException("Comment not found");
            comment.DeleteComment(profil.UserId);
            await _commentRepository.Update(comment);
            return true;
        }

        public async Task<CommentResponseDto> AnswerToComment(Guid commentId, Guid profilId, string Content)
        {
            var profil = await _profileRepository.GetById(profilId) ?? throw new EntityNotFoundException("Profil not found");
            var comment = await _commentRepository.GetById(commentId) ?? throw new EntityNotFoundException("Comment not found");
            var commentResponse = new Comment
            {
                Content = Content,
                Author = profil,
                AuthorId = profil.Id,
                PostId = comment.PostId,
                FatherCommentId = comment.Id,
                CreatedAt = DateTime.UtcNow,
            };
            await this._commentRepository.Add(commentResponse);
            return new CommentResponseDto
            {
                Id = commentResponse.Id,
                Content = Content,
                AuthorId = commentResponse.AuthorId,
                CommentId = comment.Id,
            };
        }
    }
}
