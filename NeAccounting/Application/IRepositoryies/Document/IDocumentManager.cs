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
            bool isInit,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<PagedResulViewModel<DetailRemittanceDto>> GetRemittancesByDate(DateTime StartTime,
             DateTime EndTime,
             Guid CusId,
             bool LeftOver,
             string Description,
             bool ignorePagination,
             bool isInit,
             int pageNum = 0,
             int pageCount = NeAccountingConstants.PageCount);

        Task<PagedResulViewModel<MaterialReportDto>> GetMaterialReport(int id,
            bool isBuy,
            bool isSell,
            DateTime startDate,
            DateTime endDate,
            bool ignorePagination,
            bool isInit,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<List<SummaryDoc>> GetSummaryDocs(Guid? CusId, DocumntType type);


        /// <summary>
        /// دریافت لیست بدهکاران
        /// </summary>
        Task<CreditorsOrDebtorsReport> GetDebtorsReport(DateTime startDate, DateTime endDate);

        /// <summary>
        /// دریافت لیست طلبکاران
        /// </summary>
        /// <returns></returns>
        Task<CreditorsOrDebtorsReport> GetcreditorsReport(DateTime startDate, DateTime endDate);
        #endregion

        #region Doc
        Task<(bool isSuccess, string errore)> DeleteDocument(Guid parameter);

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

        #region Cheque
        Task<PagedResulViewModel<ChequeListDtos>> GetChequeByDate(DateTime? startTime,
            DateTime? endTime,
            Guid? cusId,
            ChequeStatus status,
            bool isInit,
            int pageNum = 0,
            int pageCount = NeAccountingConstants.PageCount);

        Task<(bool isSuccess, UpdateChequeDto itm)> GetChequeById(Guid docId);

        Task<(string error, bool isSuccess)> CreateRecCheque(Guid customerId,
            SubmitChequeStatus submitStatus,
            string? descripion,
            DateTime submitDate,
            DateTime dueDate,
            long price,
            string cheque_Number,
            string accunt_Number,
            string bank_Name,
            string bank_Branch,
            string cheque_Owner);

        Task<(string error, bool isSuccess)> CreatePayCheque(Guid customerId,
            SubmitChequeStatus submitStatus,
            string? descripion,
            DateTime submitDate,
            DateTime dueDate,
            long price,
            string cheque_Number,
            string accunt_Number,
            string bank_Name,
            string bank_Branch,
            string cheque_Owner);

        Task<(string error, bool isSuccess)> CreateGarantyCheque(Guid customerId,
            SubmitChequeStatus submitStatus,
            string? descripion,
            DateTime submitDate,
            DateTime? dueDate,
            long price,
            string cheque_Number,
            string accunt_Number,
            string bank_Name,
            string bank_Branch,
            string cheque_Owner);

        Task<(string error, bool isSuccess)> UpdateCheque(
            Guid docId,
            Guid cusId,
            SubmitChequeStatus submitStatus,
            string? descripion,
            DateTime submitDate,
            DateTime? dueDate,
            long price,
            string cheque_Number,
            string accunt_Number,
            string bank_Name,
            string bank_Branch,
            string cheque_Owner);

        Task<(string error, bool isSuccess)> ConvertChequeToCash(Guid docId);

        Task<(string error, bool isSuccess)> ConvertChequeToReject(Guid docId);

        Task<(string error, bool isSuccess)> AssignCheque(Guid docId,
            Guid cusId,
            DateTime transferDate,
            string desc);

        Task<(string error, bool isSuccess)> RemoveCheque(Guid docId);
        #endregion
    }
}

