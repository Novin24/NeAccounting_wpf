using Domain.NovinEntity.Workers;
using DomainShared.Enums;
using DomainShared.ViewModels.Workers;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IWorkerManager : IRepository<Worker>
    {
        Task<List<WorkerVewiModel>> GetWorkers(string fullName,
            string jobTitle,
            string nationalCode,
            Status status);
    }
}
