using Educonnect.Common.Pagination.Dto;
using Educonnect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Infrastructure.Repositories.IRepository
{
    public interface IPostRepository : IRepository<Post>
    {
        Task<List<Post>> GetPostByName(string name);
        Task<List<Post>> GetPostById(Guid id);
        Task<PagedResponse<Post>> GetPostsAsync(PaginationParameters pagination);

        Task<PagedResponse<Post>> GetFeedAsync(PaginationParameters pagination, Guid userId);
    }
}
