using Domain.NovinEntity.Materials;
using DomainShared.ViewModels.Pun;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IMaterialManager : IRepository<Material>
    {
        /// <summary>
        /// فیلتر شده برای فاکتور ها
        /// </summary>
        /// <returns></returns>
        Task<List<MatListDto>> GetMaterails();
        /// <summary>
        /// فیلتر نشده برای لیست اجناس
        /// </summary>
        /// <param name="name"></param>
        /// <param name="serial"></param>
        /// <returns></returns>
        Task<List<PunListDto>> GetMaterails(string name, string serial);
        Task<(string error, PunListDto pun)> GetMaterailById(int Id);
        Task<(string error, bool isSuccess)> CreateMaterial(string name,
            int unitId,
            bool isService,
            long lastPrice,
            string serial,
            string address,
            bool isManufacturedGoods);

        Task<(string error, bool isSuccess)> UpdateMaterial(
            int materialId,
            string name,
            int unitId,
            string serial,
            string address,
            long lastPrice,
            bool isManufacturedGoods);

        /// <summary>
        /// Increase =
        /// buy => true
        /// </summary>
        /// <param name="materialId"></param>
        /// <param name="entity"></param>
        /// <param name="DecreaseOrIncrease"></param>
        /// <param name="lastPrice"></param>
        /// <returns></returns>
        Task<(string errore, bool isSuccess)> UpdateMaterialEntity(int materialId,
            double entity,
            bool DecreaseOrIncrease,
            long? lastPrice = null);
        Task<(string error, bool isSuccess)> ChangeStatus(
           int id, bool active);
    }
}
