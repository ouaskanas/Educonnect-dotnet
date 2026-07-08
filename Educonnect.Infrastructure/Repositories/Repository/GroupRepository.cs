using Educonnect.Domain.Entities;
using Educonnect.Infrastructure.Data;
using Educonnect.Infrastructure.Repositories.IRepository;
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
    }
}
