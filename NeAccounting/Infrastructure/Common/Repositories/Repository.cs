using NeApplication.Common;
using Domain.Common;
using DomainShared.Utilities;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Repositories
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class, IEntities
    {
        protected readonly NovinDbContext DbContext;
        public DbSet<TEntity> Entities { get; }
        public virtual IQueryable<TEntity> Table => Entities;
        public virtual IQueryable<TEntity> TableNoTracking => Entities.AsNoTracking();

        public Repository(NovinDbContext dbContext)
        {
            DbContext = dbContext;
            Entities = DbContext.Set<TEntity>(); // City => Cities
        }

        #region Async Method
        public async virtual Task<TEntity> GetByIdAsync(  params object[] ids)
        {
            return await Entities.FindAsync(ids);
        }

        public async Task<TEntity> FindEntity(  Expression<Func<TEntity, bool>> expression)
        {
            return await TableNoTracking.FirstOrDefaultAsync(expression);
        }

        public virtual async Task<bool> AddAsync(TEntity entity,   bool saveNow = true)
        {
            try
            {
                Assert.NotNull(entity, nameof(entity));
                await Entities.AddAsync(entity).ConfigureAwait(false);
                if (saveNow)
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public virtual async Task<bool> AddRangeAsync(IEnumerable<TEntity> entities,   bool saveNow = true)
        {
            try
            {
                Assert.NotNull(entities, nameof(entities));
                await Entities.AddRangeAsync(entities).ConfigureAwait(false);
                if (saveNow)
                    await DbContext.SaveChangesAsync().ConfigureAwait(false);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task<bool> UpdateAsync(TEntity entity,   bool saveNow = true)
        {
            try
            {
                Assert.NotNull(entity, nameof(entity));
                Entities.Update(entity);
                if (saveNow)
                    await DbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public virtual async Task<(bool isSuccess, string ErrorMessage)> UpdateRangeAsync(IEnumerable<TEntity> entities,   bool saveNow = true)
        {
            try
            {
                Assert.NotNull(entities, nameof(entities));
                Entities.UpdateRange(entities);
                int count = 0;
                if (saveNow)
                    count = await DbContext.SaveChangesAsync();
                return (count >= 0, null);
            }
            catch (Exception e)
            {
                return (false, e.Message);
            }

        }

        public virtual async Task<bool> DeleteAsync(TEntity entity,   bool saveNow = true)
        {
            try
            {
                Assert.NotNull(entity, nameof(entity));
                Entities.Remove(entity);
                if (saveNow)
                    await DbContext.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int entityId,   bool saveNow = true)
        {
            try
            {

                await DeleteAsync(await GetByIdAsync(entityId));
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public virtual async Task DeleteRangeAsync(IEnumerable<TEntity> entities,   bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.RemoveRange(entities);
            if (saveNow)
                await DbContext.SaveChangesAsync();
        }


        #endregion

        #region Sync Methods
        public virtual TEntity GetById(params object[] ids)
        {
            return Entities.Find(ids);
        }

        public virtual void Add(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Add(entity);
            if (saveNow)
                DbContext.SaveChanges();
        }

        public virtual void AddRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.AddRange(entities);
            if (saveNow)
                DbContext.SaveChanges();
        }

        public virtual void Update(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Update(entity);
            DbContext.SaveChanges();
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.UpdateRange(entities);
            if (saveNow)
                DbContext.SaveChanges();
        }

        public virtual void Delete(TEntity entity, bool saveNow = true)
        {
            Assert.NotNull(entity, nameof(entity));
            Entities.Remove(entity);
            if (saveNow)
                DbContext.SaveChanges();
        }

        public virtual void DeleteRange(IEnumerable<TEntity> entities, bool saveNow = true)
        {
            Assert.NotNull(entities, nameof(entities));
            Entities.RemoveRange(entities);
            if (saveNow)
                DbContext.SaveChanges();
        }
        #endregion

        #region Attach & Detach
        public virtual void Detach(TEntity entity)
        {
            Assert.NotNull(entity, nameof(entity));
            var entry = DbContext.Entry(entity);
            if (entry != null)
                entry.State = EntityState.Detached;
        }

        public virtual void Attach(TEntity entity)
        {
            Assert.NotNull(entity, nameof(entity));
            if (DbContext.Entry(entity).State == EntityState.Detached)
                Entities.Attach(entity);
        }
        #endregion

        #region Explicit Loading
        public virtual async Task LoadCollectionAsync<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty, CancellationToken cancellationToken)
            where TProperty : class
        {
            Attach(entity);

            var collection = DbContext.Entry(entity).Collection(collectionProperty);
            if (!collection.IsLoaded)
                await collection.LoadAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual void LoadCollection<TProperty>(TEntity entity, Expression<Func<TEntity, IEnumerable<TProperty>>> collectionProperty)
            where TProperty : class
        {
            Attach(entity);
            var collection = DbContext.Entry(entity).Collection(collectionProperty);
            if (!collection.IsLoaded)
                collection.Load();
        }

        public virtual async Task LoadReferenceAsync<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty, CancellationToken cancellationToken)
            where TProperty : class
        {
            Attach(entity);
            var reference = DbContext.Entry(entity).Reference(referenceProperty);
            if (!reference.IsLoaded)
                await reference.LoadAsync(cancellationToken).ConfigureAwait(false);
        }

        public virtual void LoadReference<TProperty>(TEntity entity, Expression<Func<TEntity, TProperty>> referenceProperty)
            where TProperty : class
        {
            Attach(entity);
            var reference = DbContext.Entry(entity).Reference(referenceProperty);
            if (!reference.IsLoaded)
                reference.Load();
        }

        public List<TEntity> Tolist(Expression<Func<TEntity, bool>> expression)
        {
            if (expression != null)
                return Entities.Where(expression).ToList();
            return Entities.ToList();
        }

        public virtual async Task<List<TEntity>> TolistAsync(  Expression<Func<TEntity, bool>> expression = null )
        {
            if (expression != null)
                return await TableNoTracking.Where(expression).ToListAsync();
            return await TableNoTracking.ToListAsync();
        }
        public virtual async Task<List<TEntity>> TolistAsync<TProperty>(  Expression<Func<TEntity, TProperty>> IncludeExpression, Expression<Func<TEntity, bool>> expression = null)
        {
            if (expression != null && IncludeExpression != null)
                return await TableNoTracking.Where(expression).Include(IncludeExpression).ToListAsync();
            if (IncludeExpression != null)
                return await TableNoTracking.Include(IncludeExpression).ToListAsync();
            if (expression != null)
                return await TableNoTracking.Where(expression).ToListAsync();
            return await TableNoTracking.ToListAsync();
        }
        #endregion
    }
}
