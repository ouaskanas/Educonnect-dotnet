using Educonnect.Domain.Entities;
using Educonnect.Infrastructure.Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Infrastructure.Repositories.Repository
{
    public interface IProfileRepository : IRepository<Profile>
    {
       Task<Profile?> GetByUsername(string username);
    }
}
