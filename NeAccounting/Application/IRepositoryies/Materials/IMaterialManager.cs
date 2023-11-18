using Domain.NovinEntity.Materials;
using DomainShared.Pun;
using DomainShared.ViewModels;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IMaterialManager : IRepository<Material>
    {
        Task<List<SuggestBoxViewModel<int>>> GetMaterails();
        Task<List<PunListDto>> GetMaterails(string name, string serial);
        Task<(string error, bool isSuccess)> CreateMaterial(string name,
            double entity,
            int unitId,
            string serial,
            string address);

        Task<(string error, bool isSuccess)> UpdateMaterial(
            int materialId,
            string name,
            double entity,
            int unitId,
            string serial,
            string address);
    }
}
