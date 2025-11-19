using ECommerce.Domain.Contracts.Repos;
using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts.UnitOfWork
{
    public interface IUnitOfWork
    {
        // Get a generic repository for the specified entity type and key type
        IGenericRepository<TEntity,TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>;

        // Save changes asynchronously
        Task<int> SaveChangesAsync();
    }
}
