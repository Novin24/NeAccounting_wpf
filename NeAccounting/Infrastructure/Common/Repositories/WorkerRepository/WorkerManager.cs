using Domain.NovinEntity.Materials;
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
                    Status = t.Status,
                    FullName = t.FullName,
                    NationalCode = t.NationalCode,
                }).ToListAsync();
        }

        public async Task<(string error, bool isSuccess)> Create(
            string fullName,
            string natinalCode,
            string mobile,
            string address,
            DateOnly startDate,
            string personalId,
            string accountNumber,
            string description,
            string jobTitle,
            Shift shift)
        {
            var worker = await TableNoTracking.FirstOrDefaultAsync(t => t.NationalCode == natinalCode || t.PersonnelId == personalId);

            if (worker != null)
            {
                if (worker.NationalCode == natinalCode)
                    return new($"کاربر گرامی کارگر {worker.FullName} با این کد ملی در پایگاه داده موجود می‌باشد!!!", false);

                if (worker.PersonnelId == personalId)
                    return new($"کاربر گرامی کارگر {worker.FullName} با این کد پرسنلی در پایگاه داده موجود می‌باشد!!!", false);
            }

            try
            {

                var t = await Entities.AddAsync(new Worker(
                    fullName,
                    natinalCode,
                    mobile,
                    address,
                    startDate,
                    personalId,
                    accountNumber,
                    description,
                    jobTitle,
                    shift));
            }
            catch (Exception ex)
            {
                return new("خطا دراتصال به پایگاه داده!!!", false);
            }
            return new(string.Empty, true);
        }
    }
}
