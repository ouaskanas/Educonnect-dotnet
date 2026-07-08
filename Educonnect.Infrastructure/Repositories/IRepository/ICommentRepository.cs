using Educonnect.Common.Pagination.Dto;
using Educonnect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Infrastructure.Repositories.IRepository
{
    public interface ICommentRepository : IRepository<Comment>
    {
        Task<PagedResponse<Comment>> GetCommentsAsync(PaginationParameters pagination);
        Task<PagedResponse<Comment>> GetCommentsByPostAsync(Guid postId, PaginationParameters? pagination);
        Task<PagedResponse<Comment>> GetCommentResponses(Guid commentId, Guid postId, PaginationParameters? pagination); 
    }
}
