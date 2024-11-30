using Domain.NovinEntity.Materials;
using DomainShared.ViewModels;
using DomainShared.ViewModels.unit;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IUnitManager : IRepository<Units>
    {
        Task<List<SuggestBoxViewModel<Guid>>> GetUnits(bool IgnorArchive = false);

        /// <summary>
        /// لیست کاربردی
        /// </summary>
        /// <returns></returns>
        Task<List<UnitListDto>> GetUnitList();

        /// <summary>
        /// ثبت
        /// </summary>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<(string error, bool isSuccess)> CreateUnit(string name,
            string description);

        /// <summary>
        /// ویرایش
        /// </summary>
        /// <param name="id"></param>
        /// <param name="name"></param>
        /// <param name="description"></param>
        /// <returns></returns>
        Task<(string error, bool isSuccess)> UpdateUnit(
            Guid id,
            string name,
            string description);

        /// <summary>
        /// فعال یا غیر فعال کردن
        /// </summary>
        /// <param name="id"></param>
        /// <param name="active"></param>
        /// <returns></returns>
        Task<(string error, bool isSuccess)> ChangeStatus(Guid id, bool active);

		/// <summary>
		/// اضافه کردن همه به سال مالی جدید
		/// </summary>
		/// <param name="unitList"></param>
		/// <returns></returns>
		Task<(string error, bool isSuccess)> AddAllUnitsInNewYear(List<UnitListDto> unitList);

		/// <summary>
		/// تبدیل نام واحد به شناسه ی واحد
		/// </summary>
		/// <param name="UnitNumber"></param>
		/// <returns></returns>
		Task<(string error, Guid unitId)> GetUnitIdByUnitNumber(int UnitNumber);

	}
}
