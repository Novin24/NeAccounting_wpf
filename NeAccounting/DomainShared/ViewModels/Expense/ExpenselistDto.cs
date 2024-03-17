namespace DomainShared.ViewModels.Expense
{
    public struct ExpenselistDto
    {
        public Guid Id { get; set; }
        public string Receiver { get; set; }
        public string Date { get; set; }
        public string type { get; set; }
        public string Description { get; set; }



    }
}
