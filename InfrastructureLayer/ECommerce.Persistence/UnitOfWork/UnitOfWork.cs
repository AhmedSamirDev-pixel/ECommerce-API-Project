using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Contracts.UnitOfWork;
using ECommerce.Domain.Models;
using ECommerce.Persistence.Contexts;
using ECommerce.Persistence.Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        // Cache for repositories
        private readonly Dictionary<string, object> _repositories = [];
        private readonly StoreDbContext _context;
        public UnitOfWork(StoreDbContext context)
        {
            _context = context;
        }

        // Get a generic repository for the specified entity type and key type
        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        {
            // Save the type name of the entity
            var typeName = typeof(TEntity).Name;

            // Check if the repository already exists in the cache
            if (_repositories.ContainsKey(typeName))

                // Return the existing repository
                return (IGenericRepository<TEntity, TKey>)_repositories[typeName];

            // Create a new repository instance
            else
            {
                // Create the repository
                var repositoryInstance = new GenericRepository<TEntity, TKey>(_context);

                // Add the new repository to the cache
                _repositories.Add(typeName, repositoryInstance);

                // Return the new repository
                return repositoryInstance;
            }
        }

        // Save changes asynchronously
        public async Task<int> SaveChangesAsync()
        {
             return await _context.SaveChangesAsync();
        }
    }
}
