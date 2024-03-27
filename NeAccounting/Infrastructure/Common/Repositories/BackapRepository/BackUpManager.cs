using Domain.NovinEntity.Customers;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class BackUpManager(NovinDbContext context) : Repository<Customer>(context), IBackUpRepository
    {
        public (bool isSuccess, string error) GetBackup(string localPath, string ex_path)
        {
            try
            {
                string command = @"Backup DataBase [Novin_DB] To Disk='" + localPath + "' WITH INIT";
                command = @"Backup DataBase [Novin_DB] To Disk='" + ex_path + "' WITH INIT";
                DbContext.Database.ExecuteSqlRaw(command);
                return new (true,string.Empty);
            }
            catch (Exception ex)
            {
                return new (false ,ex.Message );
            }
        }
    }
}
