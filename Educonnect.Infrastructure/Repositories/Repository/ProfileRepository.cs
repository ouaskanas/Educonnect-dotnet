using Educonnect.Domain.Entities;
using Educonnect.Infrastructure.Data;
using System;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Educonnect.Infrastructure.Repositories.IRepository;

namespace Educonnect.Infrastructure.Repositories.Repository
{
    public class ProfileRepository :  Repository<Profile>, IProfileRepository
    {
        private readonly ApplicationDbContext _context;
        public ProfileRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<bool> ExistById(Guid id)
        {
            return await _context.Profiles.AnyAsync(p => p.Id == id);
        }

        public async Task<Profile?> GetByUsername(string username)
        {
            return await _context.Profiles.FirstOrDefaultAsync(p => p.Username == username);
        }
    }
}
