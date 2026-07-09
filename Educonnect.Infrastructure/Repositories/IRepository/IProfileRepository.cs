using Educonnect.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Infrastructure.Repositories.IRepository
{
    public interface IProfileRepository : IRepository<Profile>
    {
        Task<Profile?> GetByUsername(string username);
        Task<bool> ExistById(Guid id);
    }
}
