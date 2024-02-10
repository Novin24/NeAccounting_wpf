using Domain.NovinEntity.Documents;
using NeApplication.Common;

namespace NeApplication.IRepositoryies
{
    public interface IDocumentManager : IRepository<Document>
    {
        Task<string> GetLastDocumntNumber(DocumntType type);

        Task<(string error, bool isSuccess)> CreateDocument(Guid customerId,
            uint price,
            string descripion,
            DateTime submitDate,
            bool receivedOrPaid);

        Task<(string error, bool isSuccess)> CreateSellDocument(Guid customerId,
                uint price,
                string descripion,
                DateTime submitDate,
                bool receivedOrPaid);

    }
}
