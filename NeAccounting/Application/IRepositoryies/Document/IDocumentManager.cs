using Domain.NovinEntity.Documents;
using DomainShared.Constants;
using DomainShared.Enums;
using DomainShared.ViewModels.Document;
using DomainShared.ViewModels.PagedResul;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IDocumentManager : IRepository<Document>
    {
        #region Invoice
        Task<string> GetLastDocumntNumber(DocumntType type);

        /// <summary>
        /// received => true
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="price"></param>
        /// <param name="type"></param>
        /// <param name="descripion"></param>
        /// <param name="submitDate"></param>
        /// <param name="receivedOrPaid"></param>
        /// <returns></returns>
        Task<(string error, bool isSuccess)> CreateDocument(Guid customerId,
            long price,
            DocumntType type,
            PaymentType payType,
            string? descripion,
            DateTime submitDate,
            bool receivedOrPaid);

        /// <summary>
        /// received => true
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="price"></param>
        /// <param name="descripion"></param>
        /// <param name="submitDate"></param>
        /// <param name="receivedOrPaid"></param>
        /// <param name="remittances"></param>
        /// <returns></returns>
        Task<(string error, bool isSuccess)> CreateSellDocument(Guid customerId,
                long price,
                double? commission,
                string? descripion,
                DateTime submitDate,
                bool receivedOrPaid,
                List<RemittanceListViewModel> remittances);

        /// <summary>
        /// received => true
        /// </summary>
        /// <param name="customerId"></param>
        /// <param name="price"></param>
        /// <param name="descripion"></param>
        /// <param name="submitDate"></param>
        /// <param name="receivedOrPaid"></param>
        /// <param name="remittances"></param>
        /// <returns></returns>
        Task<(string error, bool isSuccess)> CreateBuyDocument(Guid customerId,
                long price,
                double? commission,
                string? descripion,
                DateTime submitDate,
                bool receivedOrPaid,
                List<RemittanceListViewModel> remittances);
        #endregion
        #region status

        /// <summary>
        /// مبلغ بدهی به ما
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<long> GetDebt(Guid customerId);

        /// <summary>
        /// مبلغ طلب از ما
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        Task<long> GetCredit(Guid customerId);

        Task<(long, string)> GetStatus(Guid customerId);
        #endregion

        #region report
        Task<PagedResulViewModel<InvoiceListDto>> GetInvoicesByDate(DateTime StartTime,
            DateTime EndTime,
            string desc,
            Guid CusId,
            bool LeftOver,
            bool ignorePagination,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<IEnumerable<DetailRemittanceDto>> GetRemittancesByDate(DateTime StartTime, DateTime EndTime, Guid CusId, bool LeftOver, string Description);
        #endregion
    }
}

