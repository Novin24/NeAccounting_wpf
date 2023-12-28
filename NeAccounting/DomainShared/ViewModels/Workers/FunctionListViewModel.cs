namespace DomainShared.ViewModels.Workers
{
    public struct FunctionListViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        /// <summary>
        /// تعداد  کاری
        /// </summary>
        public byte AmountOf { get; set; }

        /// <summary>
        /// تعداد اضافه کاری
        /// </summary>
        public byte AmountOfOverTime { get; set; }
        public DateTime Date { get; set; }
    }
}
