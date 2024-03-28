using Domain.BaseDomain.User;
using NeApplication.Common;

namespace NeApplication.IBaseRepositories
{
    public interface IBackUpManager : IBaseRepository<IdentityUser>
    {
        Task<(bool isSuccess, string error)> GetBackup(string backUpPathh);
        Task<(bool isSuccess, string error)> Restore(string file);
    }
}
