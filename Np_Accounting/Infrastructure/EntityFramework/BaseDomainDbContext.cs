using Domain.Common;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.EntityFramework
{
    public class BaseDomainDbContext : DbContext
    {

        protected override void OnConfiguring(
                        DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=BaseDomain;Trusted_Connection=True;");

        }





        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IBaseDomainEntities).Assembly;
            modelBuilder.RegisterAllEntities<IBaseDomainEntities>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();
        }
    }


    public static class ServiceCollectionExtensions
    {
        //public static readonly ILoggerFactory MyLoggerFactory
        //    = LoggerFactory.Create(builder =>
        //    {
        //        builder.AddFilter((category, level) =>
        //                category == DbLoggerCategory.Database.Command.Name
        //                && level == LogLevel.Information)
        //            .AddConsole();
        //    });

        public static void AddRelationalDbContext(
            this IServiceCollection services,
            string connectionString)
        {
            if (string.IsNullOrWhiteSpace(connectionString))
            {
                throw new ArgumentException("Connection string is either null or empty.");
            }

            services.AddDbContext<BaseDomainDbContext>(options =>
            {
                //options.UseLoggerFactory(MyLoggerFactory);
                options.EnableSensitiveDataLogging(true);
                options.UseSqlServer(connectionString, builder =>
                {
                    ////builder.EnableRetryOnFailure(3, TimeSpan.FromSeconds(10), null);
                    builder.MigrationsAssembly("CleanHr.Persistence.RelationalDB");
                    builder.MigrationsHistoryTable("__EFCoreMigrationsHistory", schema: "_Migration");
                });
            });
        }
    }
}
