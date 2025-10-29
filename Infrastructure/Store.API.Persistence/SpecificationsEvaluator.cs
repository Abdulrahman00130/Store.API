using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Store.API.Domain.Contracts;
using Store.API.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Store.API.Persistence
{
    public static class SpecificationsEvaluator
    {
        // Generate Dynamic query
        public static IQueryable<TEntity> GetQuery<TKey, TEntity>(IQueryable<TEntity> inputQuery, ISpecifications<TKey, TEntity> spec) where TEntity : BaseEntity<TKey>
        {
            var query = inputQuery;     // _context.Products

            // filtering
            if (spec.Criteria is not null)
            {
                query = query.Where(spec.Criteria); // _context.Products.Where(criteria)
            }

            // sorting
            if(spec.OrderBy is not null)
            {
                query = query.OrderBy(spec.OrderBy);
            }
            else if (spec.OrderByDescending is not null)
            {
                query = query.OrderByDescending(spec.OrderByDescending);
            }

            // pagination
            if(spec.IsPagination)
            {
                query = query.Skip(spec.Skip).Take(spec.Take);
            }

            // loading (eager loading)
            query = spec.Includes.Aggregate(query, (query, includeExpression) => query.Include(includeExpression));
            // _context.Products.Where(criteria).Include(Includes[0])
            // _context.Products.Where(criteria).Include(Includes[0]).Include(Includes[1])

            return query;
        }
    }
}
