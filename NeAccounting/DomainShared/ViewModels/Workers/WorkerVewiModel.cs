using DomainShared.Enums;

namespace DomainShared.ViewModels.Workers
{
    public struct WorkerVewiModel
    {
        public int Id { get; set; }
        public string PersonelId { get; set; }
        public string FullName { get; set; }
        public string JobTitle { get; set; }
        public string NationalCode { get; set; }
        public Status Status { get; set; }
    }
}
