using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FORUM_PROJECT.DAL
{
    public interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Get(
            Expression<Func<TEntity, bool>> filter,
            string includeProperties = ""
        );

        Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> filter,
            string includeProperties = ""
            );

        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");
        
        ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity);
    }
}
