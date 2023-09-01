using Domain.Common;
using Infrastructure.Utilities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EntityFramework
{
    public class NovinDbContext :DbContext
    {

        public NovinDbContext(DbContextOptions<NovinDbContext> options ): base( options )
        {

        }
        protected override void OnConfiguring(
                        DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=NovinWpf;Trusted_Connection=True;");

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            var entitiesAssembly = typeof(IEntities).Assembly;
            modelBuilder.RegisterAllEntities<IEntities>(entitiesAssembly);
            modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
            modelBuilder.AddRestrictDeleteBehaviorConvention();
            modelBuilder.AddSequentialGuidForIdConvention();


            modelBuilder.ConfigureDbContext();
        }
    }
}
