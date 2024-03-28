using Domain.BaseDomain.User;
using DomainShared.Constants;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IBaseRepositories;

namespace Infrastructure.Common.BaseRepositories.BackapRepository
{
    public class BackUpManager(BaseDomainDbContext context) : BaseRepository<IdentityUser>(context), IBackUpManager
    {
        public async Task<(bool isSuccess, string error)> GetBackup(string backUpPathh)
        {
            try
            {
                string command = $@"Backup DataBase [{NeAccountingConstants.NvoinDbConnectionStrint}] To Disk='" + backUpPathh + "' WITH INIT";
                await DbContext.Database.ExecuteSqlRawAsync(command);

                return new(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new(false, ex.Message);
            }
        }

        public async Task<(bool isSuccess, string error)> Restore(string file)
        {
            try
            {
                string command = $"ALTER DATABASE [{NeAccountingConstants.NvoinDbConnectionStrint}] SET OFFLINE WITH ROLLBACK IMMEDIATE " +
                                 $" RESTORE DATABASE [{NeAccountingConstants.NvoinDbConnectionStrint}] FROM DISK='" + file + "'WITH REPLACE " +
                                 $"ALTER DATABASE [{NeAccountingConstants.NvoinDbConnectionStrint}] SET ONLINE";
                await DbContext.Database.ExecuteSqlRawAsync(command);

                return new(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new(false, ex.Message);
            }
        }
    }
}
