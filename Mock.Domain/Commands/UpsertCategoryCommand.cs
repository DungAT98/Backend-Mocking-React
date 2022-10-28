namespace Mock.Domain.Commands;

public class UpsertCategoryCommand
{
    public Guid? Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }
}