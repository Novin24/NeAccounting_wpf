using Domain.NovinEntity.Workers;
using DomainShared.Enums;
using DomainShared.ViewModels.Workers;
using Infrastructure.EntityFramework;
using Microsoft.EntityFrameworkCore;
using NeApplication.IRepositoryies;

namespace Infrastructure.Repositories
{
    public class WorkerManager : Repository<Worker>, IWorkerManager
    {
        public WorkerManager(NovinDbContext context) : base(context) { }

        public Task<List<WorkerVewiModel>> GetWorkers(string fullName, string jobTitle, string nationalCode, Status status)
        {
            return TableNoTracking
                .Where(t => !string.IsNullOrEmpty(fullName) && t.FullName.Contains(fullName))
                .Where(t => !string.IsNullOrEmpty(jobTitle) && t.JobTitle.Contains(jobTitle))
                .Where(t => !string.IsNullOrEmpty(nationalCode) && t.JobTitle.Contains(nationalCode))
                .Where(t => status != Status.All && t.Status == status)
                .Select(t => new WorkerVewiModel
                {
                    Id = t.Id,
                    PersonelId = t.PersonnelId,
                    JobTitle = t.JobTitle,
                    status = t.Status,
                    FullName = t.FullName,
                    NationalCode = t.NationalCode,
                }).ToListAsync();
        }
    }
}
