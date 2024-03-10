using Domain.NovinEntity.Services;
using DomainShared.ViewModels.Service;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IServiceManager : IRepository<Service>
    {
        Task<List<ServiceListDto>> GetServiceList();
        Task<(string error, bool isSuccess)> CreateService(string name,
            int price);
        Task<(string error, bool isSuccess)> UpdateService(
            int id,
            string name,
            int price);
        Task<(string error, bool isSuccess)> ChangeStatus(int id, bool active);


    }
}
