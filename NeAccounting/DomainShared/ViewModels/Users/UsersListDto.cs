namespace DomainShared.ViewModels.Users
{
    public class UsersListDto
    {
        public int Row { get; set; }
        public Guid Id { get; set; }
        public string DisplayName { get; set; }
        public string UserName { get; set; }
        public string Mobile { get; set; }
        public string ShamsiDate { get; set; }
        public DateTime? LastSeen{ get; set; }
        public bool IsActive { get; set; }

    }
}
