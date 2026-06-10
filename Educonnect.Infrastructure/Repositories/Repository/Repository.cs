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
    public class Repository<T> : IRepository<T> where T : class
    {
        private ApplicationDbContext _context;
        public Repository(ApplicationDbContext context)
        {
            this._context = context;
        }

        public async Task<T> Add(T entity)
        {
            await this._context.Set<T>().AddAsync(entity);
            await this._context.SaveChangesAsync();
            return entity;
        }

        public async Task Delete(Guid id)
        {
            var entity = await this._context.Set<T>().FindAsync(id);
            if (entity is null) return;
            this._context.Set<T>().Remove(entity);
            await this._context.SaveChangesAsync();
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            return await this._context.Set<T>().ToListAsync();
        }

        public async Task<T?> GetById(Guid id)
        {
            return await _context.Set<T>().FindAsync(id);
        }

        public async Task Update(T entity)
        {
            var entry = this._context.Entry(entity);
            entry.State = EntityState.Modified;
            await this._context.SaveChangesAsync();
        }
    }
}
