using ECommerce.Domain.Contracts.Specifications;
using ECommerce.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Persistence
{
    public static class SpecificationsEvaluator
    {
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> baseQuery,ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey> 
        {
            var query = baseQuery;
            if (specifications.Criteria is not null)
                query = query.Where(specifications.Criteria);

            if (specifications.OrderBy is not null)
                query = query.OrderBy(specifications.OrderBy);

            if (specifications.OrderByDesc is not null)
                query = query.OrderByDescending(specifications.OrderByDesc);

            if (specifications.Includes is not null && specifications.Includes.Any())
                query = specifications.Includes.Aggregate(query, (CurrentQuery, Expression) => CurrentQuery.Include(Expression));

            return query;
        }

    }
}
