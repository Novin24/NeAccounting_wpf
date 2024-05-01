namespace Domain.Common
{
    public interface IBaseDomainEntities
    {
        public DateTime CreationTime { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifireId { get; set; }
    }
}
