using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Linq.Expressions;
using System.Threading.Tasks;
using FORUM_PROJECT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FORUM_PROJECT.DAL
{
    public class GenericRepository<TEntity> : IGenericRepository<TEntity> where TEntity : class
    {
        internal ForumContext context;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ForumContext context)
        {
            this.context = context;
            this.dbSet = context.Set<TEntity>();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            TEntity result = dbSet.FirstOrDefault(filter);
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                dbSet.Include(includeProperty);
            }

            return result;
        }

        public async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> filter, string includeProperties = "")
        {
            TEntity result = await dbSet.FirstOrDefaultAsync(filter);
            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                dbSet.Include(includeProperty);
            }

            return result;
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public async Task<IEnumerable<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> filter = null, Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null, string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return query.ToList();
            }
        }

        public EntityEntry<TEntity> Add(TEntity entity)
        {
            var savedEntry = dbSet.Add(entity);
            context.SaveChanges();

            return savedEntry;
        }

        public async ValueTask<EntityEntry<TEntity>> AddAsync(TEntity entity)
        {
            var addedEntry = await dbSet.AddAsync(entity);
            await context.SaveChangesAsync();
            return addedEntry;
        }

        public void AddRange(IEnumerable<TEntity> entities)
        {
            dbSet.AddRange(entities);
            context.SaveChanges();
        }

        public async Task AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await dbSet.AddRangeAsync();
            await context.SaveChangesAsync();
        }

        public EntityEntry<TEntity> Update(TEntity entity)
        {
            var entry = dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            context.SaveChanges();

            return entry;
        }

        public async Task<EntityEntry<TEntity>> UpdateAsync(TEntity entity)
        {
            var entry = dbSet.Attach(entity);
            context.Entry(entity).State = EntityState.Modified;
            await context.SaveChangesAsync();

            return entry;
        }

        public void UpdateRange(IEnumerable<TEntity> entities)
        {
            entities.ToList().ForEach(entity =>
            {
                dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            });

            context.SaveChanges();
        }
        public async Task UpdateRangeAsync(IEnumerable<TEntity> entities)
        {
            entities.ToList().ForEach(entity =>
            {
                dbSet.Attach(entity);
                context.Entry(entity).State = EntityState.Modified;
            });

            await context.SaveChangesAsync();
        }

        public EntityEntry<TEntity> Remove(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            var entry = dbSet.Remove(entity);
            context.SaveChanges();

            return entry;
        }
        public async Task<EntityEntry<TEntity>> RemoveAsync(TEntity entity)
        {
            if (context.Entry(entity).State == EntityState.Detached)
            {
                dbSet.Attach(entity);
            }

            var entry = dbSet.Remove(entity);
            await context.SaveChangesAsync();

            return entry;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            entities.ToList().ForEach(entity =>
            {
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }

                dbSet.Remove(entity);
            });

            context.SaveChanges();
        }

        public async Task RemoveRangeAsync(IEnumerable<TEntity> entities)
        {
            entities.ToList().ForEach(entity =>
            {
                if (context.Entry(entity).State == EntityState.Detached)
                {
                    dbSet.Attach(entity);
                }

                dbSet.Remove(entity);
            });

            await context.SaveChangesAsync();
        }
    }
}