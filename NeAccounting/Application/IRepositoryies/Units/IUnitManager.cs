using Domain.NovinEntity.Materials;
using DomainShared.ViewModels;
using DomainShared.ViewModels.unit;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IUnitManager : IRepository<Unit>
    {
        Task<List<SuggestBoxViewModel<Guid>>> GetUnits(bool IgnorArchive = false);

        Task<List<UnitListDto>> GetUnitList();

        Task<(string error, bool isSuccess)> CreateUnit(string name,
            string description);

        Task<(string error, bool isSuccess)> UpdateUnit(
            Guid id,
            string name,
            string description);

        Task<(string error, bool isSuccess)> ChangeStatus(Guid id, bool active);
    }
}
