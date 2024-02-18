using Domain.NovinEntity.Documents;
using DomainShared.ViewModels.Document;
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
        Task<(string error, bool isSuccess, string docSerial)> CreateDocument(Guid customerId,
            long price,
            DocumntType type,
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
        Task<(string error, bool isSuccess, string docSerial)> CreateSellDocument(Guid customerId,
                long price,
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
        Task<(string error, bool isSuccess, string docSerial)> CreateBuyDocument(Guid customerId,
                long price,
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
        Task<IEnumerable<InvoiceListDto>> GetInvoicesByDate(DateTime? StartTime, DateTime? EndTime, Guid? CusId, bool LeftOver);

        Task<IEnumerable<DetailRemittanceDto>> GetRemittancesByDate(DateTime StartTime, DateTime EndTime, Guid CusId, bool LeftOver, string Description);
        #endregion
    }
}

