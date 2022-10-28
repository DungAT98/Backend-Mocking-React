namespace Mock.Domain.Abstractions;

public interface ISoftDelete
{
    public bool IsDeleted { get; set; }
}