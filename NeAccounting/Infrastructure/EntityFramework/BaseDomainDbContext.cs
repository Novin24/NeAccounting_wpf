using Domain.Common;
using DomainShared.Constants;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.EntityFramework
{
    public class BaseDomainDbContext : DbContext
    {

        protected override void OnConfiguring(
                        DbContextOptionsBuilder optionsBuilder)
        {
            //علی اصغر
            //optionsBuilder.UseSqlServer("Server=.\\SQLEXPRESS;Database=BaseDomain;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;");

            //optionsBuilder.UseSqlServer("Server=ALI"\\SQLEXPRESS;Database=BaseDomain;Integrated Security=True;Trusted_Connection=True;TrustServerCertificate=True;");

            //سرور
            optionsBuilder.UseSqlServer("Server=192.168.10.242,1433;Database=BaseDomain;User Id=SA;Password=@#$%QWer;TrustServerCertificate=True;");

            // ابلفضل
            //optionsBuilder.UseSqlServer("Data Source=(localDb)"\\MssqlLocalDb;Database=BaseDomain;Integrated Security=True;User ID=DESKTOP-N0GO0QP\"\ALI;");


        }

        private static readonly MethodInfo ConfigureBasePropertiesMethodInfo = typeof(BaseDomainDbContext)
        .GetMethod(nameof(ConfigureBaseProperties),
        BindingFlags.Instance | BindingFlags.NonPublic)!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IBaseDomainEntities).Assembly;
            modelBuilder.RegisterAllEntities<IBaseDomainEntities>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                ConfigureBasePropertiesMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
            modelBuilder.ConfigureBaseDbContext();
        }



        protected virtual void ConfigureBaseProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
                    where TEntity : class
        {
            if (mutableEntityType.IsOwned())
            {
                return;
            }

            if (!typeof(IBaseDomainEntities).IsAssignableFrom(typeof(TEntity)))
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

                if (entity.State == EntityState.Modified && entity.Entity is IBaseDomainEntities)
                {
                    var book = entity.Entity as IBaseDomainEntities;
                    book.LastModifireId = CurrentUser.CurrentUserId;
                    book.LastModificationTime = DateTime.Now;
                }

                if (entity.State == EntityState.Deleted && entity.Entity is ISoftDeleted)
                {
                    entity.State = EntityState.Modified;
                    var book = entity.Entity as ISoftDeleted;
                    book.DeletionTime = DateTime.Now;
                    book.DeleterId = CurrentUser.CurrentUserId;
                    book.IsDeleted = true;
                }

                if (entity.State == EntityState.Added && entity.Entity is IBaseDomainEntities)
                {
                    var book = entity.Entity as IBaseDomainEntities;
                    book.CreatorId = CurrentUser.CurrentUserId;
                    book.CreationTime = DateTime.Now;
                }
            }
        }

    }
}
