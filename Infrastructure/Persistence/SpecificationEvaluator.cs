using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Contracts;
using Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace Persistence
{
    // Helper Class
    internal static class SpecificationEvaluator
    {
        // Create Query
        // dbContext.Products.where(P => P.id == id).Include(P => P.productBrand).Include(P => P.ProductType)
        public static IQueryable<TEntity> CreateQuery<TEntity, TKey>(IQueryable<TEntity> InputQuery, ISpecifications<TEntity, TKey> specifications) where TEntity : BaseEntity<TKey>
        {
            var Query = InputQuery;

            if(specifications.Criteria is not null)
            {
                Query = Query.Where(specifications.Criteria);
            }

            if(specifications.IncludeExpressions is not null && specifications.IncludeExpressions.Count > 0)
            {
                //foreach (var exp in specifications.IncludeExpressions)
                //{
                //    Query = Query.Include(exp);
                //}

                Query = specifications.IncludeExpressions.Aggregate(Query, (CurrentQuery, IncludeExp) => CurrentQuery.Include(IncludeExp));
            }



            return Query;
        }
    }
}
