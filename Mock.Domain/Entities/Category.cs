using Mock.Domain.Abstractions;

namespace Mock.Domain.Entities;

public class Category : EntityBase
{
    public string Name { get; set; } = default!;

    public string? Description { get; set; }
}