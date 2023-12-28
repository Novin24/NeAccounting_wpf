namespace DomainShared.ViewModels.Workers
{
    public struct AidViewModel
    {
        public string Name { get; set; }
        public uint Price { get; set; }
        public DateTime Date { get; set; }
        public AidDetails Details { get; set; }
    }

    public struct AidDetails
    {
        public int Id { get; set; }
        public int SalaryId { get; set; }
        public int WorkerId { get; set; }
    }
}
