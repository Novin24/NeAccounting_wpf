namespace Domain.Common
{
    public interface IEntities
    {
    }

    public interface ISoftDeleted
    {
        public bool IsDeleted { get; set; }
    }
}
