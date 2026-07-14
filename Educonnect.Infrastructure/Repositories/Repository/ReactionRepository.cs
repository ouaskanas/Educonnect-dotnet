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
    public class ReactionRepository : Repository<Reaction>, IReactionRepository
    {
        private readonly ApplicationDbContext _context;
        public ReactionRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<PagedResponse<Reaction>> GetReactionsAsync(PaginationParameters pagination)
        {
            var query = _context.Reactions
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking();
            int totalRecords = await query.CountAsync();

            var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
            return new PagedResponse<Reaction>(data, pagination.PageNumber, pagination.PageSize, totalRecords);
        }

        public async Task<PagedResponse<Reaction>> GetReactionsByPostAsync(Guid postId, PaginationParameters? pagination, Guid? profileId)
        {
            pagination ??= new PaginationParameters();

            var query = _context.Reactions
            .Where(c => c.PostId == postId && c.ProfileId != profileId.Value)
            .OrderByDescending(c => c.CreatedAt)
            .AsNoTracking();

            int totalRecords = await query.CountAsync();

            var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
            return new PagedResponse<Reaction>(data, pagination.PageNumber, pagination.PageSize, totalRecords);

        }

        public async Task<PagedResponse<Reaction>> GetReactionsByCommentAsync(Guid commentId, PaginationParameters? pagination, Guid? profileId)
        {
            pagination ??= new PaginationParameters();

            var query = _context.Reactions
            .Where(c => c.CommentId == commentId && c.ProfileId != profileId.Value)
            .OrderByDescending(c => c.CreatedAt)
            .AsNoTracking();

            int totalRecords = await query.CountAsync();

            var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
            return new PagedResponse<Reaction>(data, pagination.PageNumber, pagination.PageSize, totalRecords);
        }

        public async Task<Reaction?> GetReactionByPostAndProfileAsync(Guid postId, Guid profileId)
        {
            return await _context.Reactions
                .FirstOrDefaultAsync(r => r.PostId == postId && r.ProfileId == profileId);
        }

        public async Task<Reaction?> GetReactionByCommentAndProfileAsync(Guid commentId, Guid profileId)
        {
            return await _context.Reactions
                .FirstOrDefaultAsync(r => r.CommentId == commentId && r.ProfileId == profileId);
        }
    }
}
