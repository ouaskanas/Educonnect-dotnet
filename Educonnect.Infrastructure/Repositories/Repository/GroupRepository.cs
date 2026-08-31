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
    public class GroupRepository : Repository<Group>, IGroupRepository
    {
        private readonly ApplicationDbContext _context;
        public GroupRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<PagedResponse<Group>> GetGroupsAsync(PaginationParameters pagination)
        {
            var query = _context.Groups
            .OrderByDescending(p => p.CreatedAt)
            .AsNoTracking();
            int totalRecords = await query.CountAsync();

            var data = await query
            .Skip((pagination.PageNumber - 1) * pagination.PageSize)
            .Take(pagination.PageSize)
            .ToListAsync();
            return new PagedResponse<Group>(data, pagination.PageNumber, pagination.PageSize, totalRecords);
        }
    }
}
