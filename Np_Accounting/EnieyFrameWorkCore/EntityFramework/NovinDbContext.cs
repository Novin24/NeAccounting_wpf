using Microsoft.EntityFrameworkCore;

namespace EnieyFrameworkCore.EntityFramework
{
    public class NovinDbContext :DbContext
    {
        public NovinDbContext(DbContextOptions<DbContext> options)
        : base(options)
        {
        }
    }
}
