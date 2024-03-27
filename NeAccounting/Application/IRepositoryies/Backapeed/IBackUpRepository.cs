using Domain.BaseDomain.User;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IBackUpManager: IBaseRepository<IdentityUser>
    {
        (bool isSuccess, string error) GetBackup(string localPath, string ex_path);
    }
}
