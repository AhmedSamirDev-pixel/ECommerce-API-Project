using ECommerce.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Domain.Contracts.Specifications
{
    
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {
        // Filter Criteria (WHERE)
        Expression<Func<TEntity, bool>> Criteria { get; }
        //Includes(to load navigation properties)
        List<Expression<Func<TEntity, object>>> Includes { get; }
        // Sorting Ascending
        Expression<Func<TEntity, object>> OrderBy { get; }
        // Sorting Descending
        Expression<Func<TEntity, object>> OrderByDesc { get; }

        // Adding Pagination
        int Take { get; }
        int Skip { get; }
        bool IsPaginated { get; set; }

    }
}
