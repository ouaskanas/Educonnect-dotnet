using Educonnect.Common.Pagination.Dto;
using Educonnect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Infrastructure.Repositories.IRepository
{
    public interface IReactionRepository : IRepository<Reaction>
    {
        Task<PagedResponse<Reaction>> GetReactionsAsync(PaginationParameters pagination);
        Task<PagedResponse<Reaction>> GetReactionsByPostAsync(Guid PostId, PaginationParameters? pagination, Guid? profileId);
        Task<PagedResponse<Reaction>> GetReactionsByCommentAsync(Guid postId, PaginationParameters? pagination, Guid? profileId);
    }
}
