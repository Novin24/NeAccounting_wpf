namespace NeAccounting.Models
{
    class PrintInfo
    {
        public string Tabligh { get; set; }
        public string Company_Name { get; set; }
        public string Management { get; set; }
    }

    public class BackFilesDetails
    {
        public Guid Id { get; set; }
        public string? FileName { get; set; }
        public string? FulePath { get; set; }
        public string FullName { get; set; }
        public string? CreationDate { get; set; }    
        public string? CreationTime { get; set; }    
    }
}
