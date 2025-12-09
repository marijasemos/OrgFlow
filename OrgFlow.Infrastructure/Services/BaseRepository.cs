using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OrgFlow.Infrastructure.Interfaces;

namespace OrgFlow.Infrastructure.Services
{
    public class BaseRepository<T> : IBaseRepository<T> where T : class
    {
        protected readonly OrgFlowDbContext _context;
        protected readonly DbSet<T> _dbSet;

        public BaseRepository(OrgFlowDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public virtual async Task<IReadOnlyList<T>> GetAllAsync(CancellationToken token = default)
        {
            return await _dbSet.AsNoTracking().ToListAsync(token);
        }

        public virtual async Task<T?> GetByIdAsync(int id, CancellationToken token = default)
        {
            // FindAsync koristi primarni ključ (pretpostavljamo da je Id : int)
            return await _dbSet.FindAsync(id, token);
        }

        public virtual async Task AddAsync(T entity, CancellationToken token = default)
        {
            await _dbSet.AddAsync(entity, token);
            await _context.SaveChangesAsync(token);
        }

        public virtual async Task UpdateAsync(T entity, CancellationToken token = default)
        {
            _dbSet.Update(entity);
            await _context.SaveChangesAsync(token);
        }

        public virtual async Task DeleteAsync(int id, CancellationToken token = default)
        {
            var entity = await this.GetByIdAsync(id, token);
            if (entity is null)
                throw new KeyNotFoundException($"Entity with id {id} does not exist!");

            _dbSet.Remove(entity);
            await _context.SaveChangesAsync(token);
        }
    }
}
