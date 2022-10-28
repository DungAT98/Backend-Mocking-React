namespace Mock.Domain.Abstractions;

public abstract class BaseSearchCommand
{
    public string? SearchText { get; set; }

    public int PageNumber { get; set; } = 1;

    public int PageSize { get; set; } = 10;
}