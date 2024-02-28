using Domain.NovinEntity.Materials;
using DomainShared.ViewModels;
using DomainShared.ViewModels.unit;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IUnitManager : IRepository<Unit>
    {
        Task<List<SuggestBoxViewModel<int>>> GetUnits();

        Task<List<UnitListDto>> GetUnitList();

        Task<(string error, bool isSuccess)> CreateUnit(string name,
            string description);

        Task<(string error, bool isSuccess)> UpdateUnit(
            int id,
            string name,
            string description);

        Task<(string error, bool isSuccess)> ChangeStatus(int id);
    }
}
