using Educonnect.Common.Pagination.Dto;
using Educonnect.Domain.Entities;
using Educonnect.Infrastructure.Data;
using Educonnect.Infrastructure.Repositories.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Infrastructure.Repositories.Repository
{
    public class PostRepository : Repository<Post>, IPostRepository
    {
        private readonly ApplicationDbContext _context;
        public PostRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<PagedResponse<Post>> GetFeedAsync(PaginationParameters pagination, Guid userId)
        {
            var userGroupIds = await _context.Set<Group>()
                .Where(g => g.Memebres.Any(m => m.Id == userId) && !g.IsDeleted)
                .Select(g => g.Id)
                .ToListAsync();

            var now = DateTime.UtcNow;

            var query = _context.Set<Post>()
                .Where(p => !p.IsDeleted)
                .Select(p => new
                {
                    Post = p,
                    Author = p.Author,
                    CommentCount = p.Comments.Count(),
                    ReactionCount = p.Reactions.Count(),
                    CommunityBoost = (p.GroupId != null && userGroupIds.Contains(p.GroupId.Value)) ? 100 : 0,
                    AgeInHours = EF.Functions.DateDiffHour(p.CreatedAt, now),
                    TopComment = p.Comments
                        .OrderByDescending(c => c.Reactions.Count())
                        .FirstOrDefault()
                });

            var pagedResults = await query
                .Select(x => new
                {
                    x.Post,
                    x.Author,
                    x.TopComment,
                    Score = (double)(x.ReactionCount * 2 + x.CommentCount * 5 + x.CommunityBoost)
                            / Math.Pow((x.AgeInHours + 2), 1.5)
                })
                .OrderByDescending(x => x.Score)
                .Skip((pagination.PageNumber - 1) * pagination.PageSize)
                .Take(pagination.PageSize)
                .ToListAsync();

            var posts = pagedResults.Select(x =>
            {
                var post = x.Post;
                post.Author = x.Author;
                post.Comments = x.TopComment != null
                    ? new List<Comment> { x.TopComment }
                    : new List<Comment>();

                return post;
            }).ToList();

            var totalRecords = await _context.Set<Post>().CountAsync(p => !p.IsDeleted);
            return new PagedResponse<Post>(posts, pagination.PageNumber, pagination.PageSize, totalRecords);
        }


        public Task<Post?> GetPostById(Guid id)
        {
            return this._context.Posts
                .Include(p=>p.Comments)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<List<Post>> GetPostByName(string name)
        {
            return await this._context.Posts.Where(p=>p.Title.Contains(name) || p.Body.Contains(name)).ToListAsync();
        }

        public async Task<PagedResponse<Post>> GetPostsAsync(PaginationParameters pagination)
        {
            var query = _context.Posts
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking();
            int totalRecords = await query.CountAsync();

            var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
            return new PagedResponse<Post>(data, pagination.PageNumber, pagination.PageSize, totalRecords);
        }
    }
}
