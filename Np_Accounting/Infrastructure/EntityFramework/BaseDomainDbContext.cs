using Domain.Common;
using DomainShared.Utilities;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework
{
    public class BaseDomainDbContext : DbContext
    {

        public BaseDomainDbContext(DbContextOptions<BaseDomainDbContext> options) : base(options)
        {

        }



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
}
