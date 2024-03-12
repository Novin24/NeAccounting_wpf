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

        /// <summary>
        /// دریافت جزییات فاکتور فروش
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        Task<(bool isSuccess, InvoiceDetailUpdateDto itm)> GetSellInvoiceDetail(Guid invoiceId);

        /// <summary>
        /// دریافت جزییات فاکتور خرید
        /// </summary>
        /// <param name="invoiceId"></param>
        /// <returns></returns>
        Task<(bool isSuccess, InvoiceDetailUpdateDto itm)> GetBuyInvoiceDetail(Guid invoiceId);

        /// <summary>
        /// دریافت اخرین شماره فاکتور
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
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
                List<RemittanceListViewModel> remittances);

        Task<(string error, bool isSuccess)> UpdateSellDocument(Guid docId,
            long price,
            double? commission,
            string? descripion,
            DateTime submitDate,
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
                List<RemittanceListViewModel> remittances);

        Task<(string error, bool isSuccess)> UpdateBuyDocument(Guid docId,
            long price,
            double? commission,
            string? descripion,
            DateTime submitDate,
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

        Task<UserDebtStatus> GetStatus(Guid customerId);
        #endregion

        #region report
        Task<PagedResulViewModel<InvoiceListDtos>> GetInvoicesByDate(DateTime StartTime,
            DateTime EndTime,
            string desc,
            Guid CusId,
            bool LeftOver,
            bool ignorePagination,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<IEnumerable<DetailRemittanceDto>> GetRemittancesByDate(DateTime StartTime, DateTime EndTime, Guid CusId, bool LeftOver, string Description);

        Task<List<SummaryDoc>> GetSummaryDocs(Guid? CusId, DocumntType type);
        #endregion

        #region Doc
        Task<(string error, bool isSuccess)> CreateRecDocument(Guid customerId,
            PaymentType paymentType,
            long price,
            long? discount,
            string? descripion,
            DateTime submitDate);

        Task<(string error, bool isSuccess)> CreatePayDocument(Guid customerId,
            PaymentType paymentType,
            long price,
            long? discount,
            string? descripion,
            DateTime submitDate);

        Task<(string error, bool isSuccess)> UpdatePayOrRecDocument(Guid docId,
            PaymentType paymentType,
            long price,
            long? discount,
            string? descripion,
            DateTime submitDate);


        Task<(bool isSuccess, DocUpdateDto itm)> GetDocumentById(Guid docId);

        Task<PagedResulViewModel<DalyBookDto>> GetDalyBook(int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);
        #endregion
    }
}

