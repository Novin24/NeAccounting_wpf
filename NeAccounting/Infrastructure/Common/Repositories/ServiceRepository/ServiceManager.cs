using Domain.NovinEntity.Services;
using DomainShared.ViewModels.Service;
using Infrastructure.EntityFramework;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class ServiceManager(NovinDbContext context) : Repository<Service>(context), IServiceManager
    {
        public Task<(string error, bool isSuccess)> ChangeStatus(int id, bool active)
        {
            throw new NotImplementedException();
        }

        public Task<(string error, bool isSuccess)> CreateService(string name, int price)
        {
            throw new NotImplementedException();
        }

        public Task<List<ServiceListDto>> GetServiceList()
        {
            throw new NotImplementedException();
        }

        public Task<(string error, bool isSuccess)> UpdateService(int id, string name, int price)
        {
            throw new NotImplementedException();
        }
    }
}


