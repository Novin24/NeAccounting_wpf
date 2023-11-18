using Domain.NovinEntity.Materials;
using DomainShared.ViewModels;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IUnitManager : IRepository<Unit>
    {
        Task<List<SuggestBoxViewModel<int>>> GetUnits();
    }
}
