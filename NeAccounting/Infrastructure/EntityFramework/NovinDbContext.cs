using Domain.Common;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using System;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Security.Principal;
using System.Linq.Expressions;
using System.Threading;
using DomainShared.Constants;

namespace Infrastructure.EntityFramework
{
    public class NovinDbContext : DbContext
    {

        protected override void OnConfiguring(
                        DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=NovinWpf;Trusted_Connection=True;");

        }

        private static readonly MethodInfo ConfigureBasePropertiesMethodInfo = typeof(NovinDbContext)
                .GetMethod(nameof(ConfigureBaseProperties),
                BindingFlags.Instance | BindingFlags.NonPublic)!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntities).Assembly;
            modelBuilder.RegisterAllEntities<IEntities>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureBasePropertiesMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
            modelBuilder.ConfigureDbContext();
        }

        protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
                    where TEntity : class
        {
            if (mutableEntityType.IsOwned())
            {
                return;
            }

            if (!typeof(IEntities).IsAssignableFrom(typeof(TEntity)))
            {
                return;
            }

            ConfigureGlobalFilters<TEntity>(modelBuilder, mutableEntityType);
        }

        protected virtual void ConfigureGlobalFilters<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
     where TEntity : class
        {
            if (mutableEntityType.BaseType == null && ShouldFilterEntity<TEntity>(mutableEntityType))
            {
                var filterExpression = CreateFilterExpression<TEntity>();
                if (filterExpression != null)
                {
                    modelBuilder.Entity<TEntity>().HasQueryFilter(filterExpression);
                }
            }
        }

        protected virtual bool ShouldFilterEntity<TEntity>(IMutableEntityType entityType) where TEntity : class
        {
            if (typeof(ISoftDeleted).IsAssignableFrom(typeof(TEntity)))
            {
                return true;
            }

            return false;
        }

        protected virtual Expression<Func<TEntity, bool>> CreateFilterExpression<TEntity>()
            where TEntity : class
        {
            Expression<Func<TEntity, bool>> expression = null;

            if (typeof(ISoftDeleted).IsAssignableFrom(typeof(TEntity)))
            {
                expression = e => !EF.Property<bool>(e, "IsDeleted");
            }

            return expression;
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleSoftDelete();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void HandleSoftDelete()
        {
            var entities = ChangeTracker.Entries()
                                .Where(e => e.State == EntityState.Deleted || e.State == EntityState.Modified || e.State == EntityState.Added);
            foreach (var entity in entities)
            {
                if (entity.State == EntityState.Deleted && entity.Entity is ISoftDeleted)
                {
                    entity.State = EntityState.Modified;
                    var book = entity.Entity as ISoftDeleted;
                    book.DeletionTime = DateTime.Now;
                    book.DeleterId = CurrentUser.CurrentUserId;
                    book.IsDeleted = true;
                }

                if (entity.State == EntityState.Modified && entity.Entity is IEntities)
                {
                    var book = entity.Entity as IEntities;
                    book.LastModifireId = CurrentUser.CurrentUserId;
                    book.LastModificationTime = DateTime.Now;
                }
                
                if (entity.State == EntityState.Added && entity.Entity is IEntities)
                {
                    var book = entity.Entity as IEntities;
                    book.CreatorId = CurrentUser.CurrentUserId;
                    book.CreationTime = DateTime.Now;
                }
            }
        }
    }
}
