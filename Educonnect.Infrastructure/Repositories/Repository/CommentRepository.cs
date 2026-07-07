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

        public async Task<PagedResponse<Comment>> GetCommentsByPostAsync(Guid postId, PaginationParameters? pagination)
        {
            pagination ??= new PaginationParameters();
            var query = _context.Comments
            .Where(c => c.PostId == postId)
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
    }
}
