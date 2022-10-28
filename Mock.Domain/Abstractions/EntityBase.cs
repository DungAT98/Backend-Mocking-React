namespace Mock.Domain.Abstractions;

public abstract class EntityBase : ISoftDelete
{
    public Guid Id { get; set; }

    public DateTime CreatedTime { get; set; }

    public DateTime ModifiedTime { get; set; }
    
    public bool IsDeleted { get; set; }
}