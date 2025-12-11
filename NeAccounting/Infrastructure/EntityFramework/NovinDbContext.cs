using Domain.Common;
using DomainShared.Constants;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Linq.Expressions;
using System.Reflection;

namespace Infrastructure.EntityFramework
{
    public class NovinDbContext : DbContext
    {
        private static readonly MethodInfo ConfigurePropertiesMethodInfo = typeof(NovinDbContext)
                .GetMethod(nameof(ConfigureProperties),
                BindingFlags.Instance | BindingFlags.NonPublic)!;

        protected override void OnConfiguring(
                        DbContextOptionsBuilder optionsBuilder)
        {
            //علی اصغر
            //optionsBuilder.UseSqlServer($"Server=.\\SQLExpress;Database={NeAccountingConstants.NvoinDbConnectionStrint};Trusted_Connection=True;TrustServerCertificate=True;");
            //optionsBuilder.UseSqlServer($"Data Source=(localDb)\\MssqlLocalDb;Database={NeAccountingConstants.NvoinDbConnectionStrint};Trusted_Connection=True;TrustServerCertificate=True;");

            //optionsBuilder.UseSqlServer($"Server=ALI"\\SQLEXPRESS;Database={NeAccountingConstants.NvoinDbConnectionStrint};Trusted_Connection=True;TrustServerCertificate=True;");

            //سرور
            optionsBuilder.UseSqlServer($"Data Source=192.168.10.242,1433;Database={NeAccountingConstants.NvoinDbConnectionStrint};TrustServerCertificate=True;User Id=SA;Password=@#$%QWer;");

            //ابلفضل
            //optionsBuilder.UseSqlServer($"Data Source=(localDb)"\\MssqlLocalDb;Database={NeAccountingConstants.NvoinDbConnectionStrint};Integrated Security=True;User ID=DESKTOP-N0GO0QP\"\ALI;");

        }

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
                ConfigurePropertiesMethodInfo
                    .MakeGenericMethod(entityType.ClrType)
                    .Invoke(this, new object[] { modelBuilder, entityType });
            }
            modelBuilder.ConfigureNovinDbContext();
        }

        protected virtual void ConfigureProperties<TEntity>(ModelBuilder modelBuilder, IMutableEntityType mutableEntityType)
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
                if (entity.State == EntityState.Modified && entity.Entity is IEntities)
                {
                    var book = entity.Entity as IEntities;
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
