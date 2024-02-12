using Domain.NovinEntity.Documents;
using DomainShared.ViewModels.Document;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IDocumentManager : IRepository<Document>
    {
        Task<string> GetLastDocumntNumber(DocumntType type);

        Task<(string error, bool isSuccess, string docSerial)> CreateDocument(Guid customerId,
            uint price,
            DocumntType type,
            string? descripion,
            DateTime submitDate,
            bool receivedOrPaid);

        Task<(string error, bool isSuccess , string docSerial)> CreateSellDocument(Guid customerId,
                uint price,
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
                uint price,
                string? descripion,
                DateTime submitDate,
                bool receivedOrPaid,
                List<RemittanceListViewModel> remittances);

    }
}
