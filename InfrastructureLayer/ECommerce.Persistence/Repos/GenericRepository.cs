using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Models;
using ECommerce.Persistence.Contexts;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.Repos
{
    public class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        private readonly StoreDbContext _context;
        public GenericRepository(StoreDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync()
            => await _context.Set<TEntity>().ToListAsync();

        public async Task<TEntity?> GetByIdAsync(TKey id) => await _context.Set<TEntity>().FindAsync(id);

        public void Add(TEntity entity) => _context.Set<TEntity>().Add(entity);

        public void Delete(TEntity entity) => _context.Set<TEntity>().Remove(entity);

        public void Update(TEntity entity) => _context.Set<TEntity>().Update(entity);
    }
}
