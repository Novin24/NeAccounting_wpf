using System.ComponentModel.DataAnnotations;

namespace Domain.Common
{
    public abstract class LocalEntity<TKey> : IEntities , ISoftDeleted
    {
        [Key]
        public TKey Id { get; set; }
        public DateTime CreationTime { get; set; }
        public Guid CreatorId { get; set; }
        public DateTime? LastModificationTime { get; set; }
        public Guid? LastModifireId { get; set; }
        public DateTime? DeletionTime { get; set; }
        public Guid? DeleterId { get; set; }
        public bool IsDeleted { get; set; }
    }

    public abstract class LocalEntity : LocalEntity<int>
    {

    }
}
