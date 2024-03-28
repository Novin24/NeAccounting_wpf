namespace NeAccounting.Models
{
    public class BackFilesDetails
    {
        public Guid Id { get; set; }
        public string FileName { get; set; }
        public string? FulePath { get; set; }
        public string FullName { get; set; }
        public string? CreationDate { get; set; }    
        public string? CreationTime { get; set; }    
    }
}
