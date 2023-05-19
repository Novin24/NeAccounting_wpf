using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public abstract class BaseEntity<TKey> : IEntities
    {
        [Key]
        public TKey Id { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifireId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? DeleterId { get; set; }
    }

    public abstract class BaseEntity : BaseEntity<int>
    {

    }
}
