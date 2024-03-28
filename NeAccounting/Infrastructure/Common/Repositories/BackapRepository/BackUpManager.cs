using Domain.BaseDomain.User;
using DomainShared.Constants;
using Infrastructure.Common.BaseRepositories;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class BackUpManager(BaseDomainDbContext context) : BaseRepository<IdentityUser>(context), IBackUpManager
    {
        public (bool isSuccess, string error) GetBackup(string localPath, string ex_path)
        {
            try
            {
                string command = $@"Backup DataBase [{NeAccountingConstants.NvoinDbConnectionStrint}] To Disk='" + ex_path + "' WITH INIT";
                DbContext.Database.ExecuteSqlRaw(command);

                command = $@"Backup DataBase [{NeAccountingConstants.NvoinDbConnectionStrint}] To Disk='" + localPath + "' WITH INIT";
                DbContext.Database.ExecuteSqlRaw(command);

                return new(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new(false, ex.Message);
            }
        }

        public (bool isSuccess, string error) Restore(string file)
        {
            try
            {
                string command = $"ALTER DATABASE [{NeAccountingConstants.NvoinDbConnectionStrint}] SET OFFLINE WITH ROLLBACK IMMEDIATE " +
                                 $" RESTORE DATABASE [{NeAccountingConstants.NvoinDbConnectionStrint}] FROM DISK='" + file + "'WITH REPLACE " +
                                 $"ALTER DATABASE [{NeAccountingConstants.NvoinDbConnectionStrint}] SET ONLINE";
                DbContext.Database.ExecuteSqlRaw(command);

                return new(true, string.Empty);
            }
            catch (Exception ex)
            {
                return new(false, ex.Message);
            }
        }
    }
}
