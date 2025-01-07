namespace Mongo.Common
{
    public interface IBaseEntity
    {
        public Guid Id { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime UpdatedOn { get; set; }
        public bool IsDeprecated { get; set; }
    }
}
