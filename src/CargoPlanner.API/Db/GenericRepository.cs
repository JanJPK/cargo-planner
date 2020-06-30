using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CargoPlanner.API.Db
{
    public abstract class GenericRepository<TEntity, TKey> : IGenericRepository<TEntity, TKey> where TEntity : class
    {
        protected CargoPlannerContext Context;

        protected GenericRepository(CargoPlannerContext context)
        {
            Context = context;
        }

        public DbSet<TEntity> Set => Context.Set<TEntity>();

        public Task<int> Count()
        {
            return Set.CountAsync();
        }

        public IQueryable<TEntity> Get()
        {
            return Set.AsNoTracking();
        }

        public abstract Task<TEntity> GetById(TKey id);

        public virtual async Task<int> Insert(TEntity entity)
        {
            Set.Add(entity);
            return await Context.SaveChangesAsync();
        }

        public virtual async Task<int> Insert(IEnumerable<TEntity> entities)
        {
            Set.AddRange(entities);
            return await Context.SaveChangesAsync();
        }

        public virtual async Task Update(TEntity entity)
        {
            Set.Update(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task Update(IEnumerable<TEntity> entities)
        {
            await Set.AddRangeAsync(entities);
            await Context.SaveChangesAsync();
        }

        public virtual async Task Delete(TEntity entity)
        {
            Set.Remove(entity);
            await Context.SaveChangesAsync();
        }

        public virtual async Task Delete(TKey id)
        {
            var entity = await GetById(id);
            if (entity != null)
            {
                Set.Remove(entity);
                await Context.SaveChangesAsync();
            }
        }

        public abstract Task<bool> Exists(TKey id);
    }
}