using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FORUM_PROJECT.DAL
{
    interface IGenericRepository<TEntity> where TEntity : class
    {
        TEntity Get(
            Expression<Func<TEntity, bool>> filter,
            string includeProperties = ""
        );

        Task<TEntity> GetAsync(
            Expression<Func<TEntity, bool>> filter,
            string includeProperties = ""
            );

        IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        Task<IEnumerable<TEntity>> GetAllAsync(
            Expression<Func<TEntity, bool>>? filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "");

        EntityEntry<TEntity> Add(TEntity entity);

        ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity);

        void AddRange(IEnumerable<TEntity> entities);

        Task AddRangeAsync(IEnumerable<TEntity> entities);

        EntityEntry<TEntity> Update(TEntity entity);

        Task<EntityEntry<TEntity>> UpdateAsync(TEntity entity);

        void UpdateRange(IEnumerable<TEntity> entities);

        Task UpdateRangeAsync(IEnumerable<TEntity> entities);

        EntityEntry<TEntity> Remove(TEntity entity);

        Task<EntityEntry<TEntity>> RemoveAsync(TEntity entity);

        void RemoveRange(IEnumerable<TEntity> entities);

        Task RemoveRangeAsync(IEnumerable<TEntity> entities);

    }
}
