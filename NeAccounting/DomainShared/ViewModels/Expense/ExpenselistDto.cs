namespace DomainShared.ViewModels.Expense
{
    public struct ExpenselistDto
    {
        public int Id { get; set; }
        public string Receiver { get; set; }
        public DateTime Date { get; set; }
        public string type { get; set; }
        public string Description { get; set; }



    }
}
