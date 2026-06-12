using Educonnect.Domain.Entities;
using Educonnect.Infrastructure.Data;
using Educonnect.Infrastructure.Repositories.Repository;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Educonnect.Infrastructure.Repositories.IRepository
{
    public class ProfileRepository :  Repository<Profile>, IProfileRepository
    {
        private readonly ApplicationDbContext _context;
        public ProfileRepository(ApplicationDbContext context) : base(context)
        {
            this._context = context;
        }

        public async Task<Profile?> GetByUsername(string username)
        {
            return await this._context.Profiles.FirstOrDefaultAsync(p => p.Username == username);
        }
    }
}
