using Domain.NovinEntity.Materials;
using DomainShared.ViewModels.Customer;
using DomainShared.ViewModels.Pun;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IMaterialManager : IRepository<Pun>
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

		/// <summary>
		/// لیست برای خروجی گرفتن از اجناس
		/// </summary>
		/// <param name="IsArchive"></param>
		/// <returns></returns>
		Task<List<ExporteMaterialListDto>> GetExporteMaterialList(bool IsArchive);

		Task<(string error, PunListDto pun)> GetMaterailById(Guid Id);

        Task<(string error, bool isSuccess)> CreateMaterial(string name,
            Guid unitId,
            bool isService,
            long lastPrice,
            string serial,
            string address,
            bool isManufacturedGoods);

        Task<(string error, bool isSuccess)> UpdateMaterial(
            Guid materialId,
            string name,
            Guid unitId,
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
        Task<(string errore, bool isSuccess)> UpdateMaterialEntity(Guid materialId,
            double entity,
            bool DecreaseOrIncrease,
            long? lastPrice = null);
        Task<(string error, bool isSuccess)> ChangeStatus(
           Guid id, bool active);

        Task<(string error, bool isSuccess)> AddAllMaterialsInNewYear(List<PunListDto> matList);
    }
}
