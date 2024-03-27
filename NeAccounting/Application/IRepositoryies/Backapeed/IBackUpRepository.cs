using Domain.NovinEntity.Customers;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IBackUpRepository : IRepository<Customer>
    {
        (bool isSuccess, string error) GetBackup(string localPath, string ex_path);
    }
}
