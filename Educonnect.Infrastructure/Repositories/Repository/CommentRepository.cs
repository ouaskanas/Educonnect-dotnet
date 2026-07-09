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
    public class CommentRepository : Repository<Comment>, ICommentRepository
    {
        private readonly ApplicationDbContext _context;
        public CommentRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        override
        public async Task<Comment?> GetById(Guid id)
        {
            return await this._context.Comments
                .Include(c=>c.Author)
                .FirstOrDefaultAsync(c => c.Id == id && c.FatherCommentId == null);
        }

        public async Task<PagedResponse<Comment>> GetCommentsByPostAsync(Guid postId, PaginationParameters? pagination)
        {
            pagination ??= new PaginationParameters();
            var query = _context.Comments
           .Include(c => c.Author)
           .Where(c => c.PostId == postId && c.FatherCommentId == null)
           .OrderByDescending(p => p.CreatedAt)
           .AsNoTracking();
            int totalRecords = await query.CountAsync();

            var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
            return new PagedResponse<Comment>(data, pagination.PageNumber, pagination.PageSize, totalRecords);
        }

        public async Task<PagedResponse<Comment>> GetCommentsAsync(PaginationParameters pagination)
        {
            pagination ??= new PaginationParameters();
            var query = _context.Comments
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking();
            int totalRecords = await query.CountAsync();

            var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
            return new PagedResponse<Comment>(data, pagination.PageNumber, pagination.PageSize, totalRecords);
        }

        public async Task<PagedResponse<Comment>> GetCommentResponses(Guid commentId, Guid postId, PaginationParameters? pagination)
        {
            pagination ??= new PaginationParameters();
            var query = _context.Comments
                .Where(c => c.FatherCommentId == commentId && c.PostId == postId)
                .Include(c=>c.Author)
                .OrderByDescending(p => p.CreatedAt)
                .AsNoTracking();
            int totalRecords = await query.CountAsync();

            var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
            return new PagedResponse<Comment>(data, pagination.PageNumber, pagination.PageSize, totalRecords);
        }
    }
}
