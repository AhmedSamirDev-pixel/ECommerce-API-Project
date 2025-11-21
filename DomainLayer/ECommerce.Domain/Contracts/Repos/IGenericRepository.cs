using ECommerce.Domain.Contracts.Specifications;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts.Repos
{
    // Generic Repository Interface
    public interface IGenericRepository<TEntity, TKey> where TEntity: BaseEntity<TKey>
    {
        // Get all entities
        Task<IEnumerable<TEntity>> GetAllAsync();

        // Get entity by id
        Task<TEntity?> GetByIdAsync(TKey id);

        // Add new entity
        void Add(TEntity entity);

        // Update existing entity
        void Update(TEntity entity);

        // Delete Entity
        void Delete(TEntity entity);

        // Get All Entities With it's own specifications
        Task<IEnumerable<TEntity>> GetAllWithSpecificationsAsync(ISpecifications<TEntity, TKey> specifications);

        // Get entity by Id With it's own specification
        Task<TEntity> GetByIdWithSpecificationsAsync(ISpecifications<TEntity, TKey> specifications);
    }
}
