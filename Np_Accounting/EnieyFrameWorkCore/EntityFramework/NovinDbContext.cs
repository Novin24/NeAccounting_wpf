using Domain.User;
using Microsoft.EntityFrameworkCore;

namespace EnieyFrameworkCore.EntityFramework
{
    public class NovinDbContext :DbContext
    {

        public NovinDbContext(DbContextOptions<NovinDbContext> options ): base( options )
        {

        }

        public DbSet<IdentityUser> Users => Set<IdentityUser>();


        protected override void OnConfiguring(
                        DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=(LocalDb)\\MSSQLLocalDB;Database=NovinWpf;Trusted_Connection=True;");

        }





        //protected override void OnModelCreating(ModelBuilder modelBuilder) 
        //{
        //    base.OnModelCreating(modelBuilder);

        //    var entitiesAssembly = typeof(IEntities).Assembly;
        //    modelBuilder.RegisterAllEntities<IEntities>(entitiesAssembly);
        //    modelBuilder.RegisterEntityTypeConfiguration(entitiesAssembly);
        //    modelBuilder.AddRestrictDeleteBehaviorConvention();
        //    modelBuilder.AddSequentialGuidForIdConvention();
        //}
    }
}
