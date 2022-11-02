namespace Mock.Domain.Abstractions;

public class BasePaginatedResult<TModel>
{
    private int _pageNumber = 1;

    private int _pageSize = 10;

    public BasePaginatedResult()
    {
    }

    public BasePaginatedResult(IQueryable<TModel> query, BaseSearchCommand command)
    {
        GetData(query, command);
    }

    public int TotalItems { get; set; }

    public int TotalPage => (int)Math.Ceiling(TotalItems * 1.0 / PageSize);

    public int PageNumber
    {
        get => _pageNumber;
        set
        {
            if (value <= 0)
            {
                throw new Exception("Page number incorrect");
            }

            _pageNumber = value;
        }
    }

    public int PageSize
    {
        get => _pageSize;
        set
        {
            if (value <= 0)
            {
                throw new Exception("Page size incorrect");
            }

            _pageSize = value;
        }
    }

    public List<TModel> Data { get; set; } = new();

    public void GetData(IQueryable<TModel> query, BaseSearchCommand command)
    {
        var result = query.Skip((command.PageNumber - 1) * command.PageSize)
            .Take(command.PageSize)
            .ToList();

        Data = result;
        PageNumber = command.PageNumber;
        PageSize = command.PageSize;
        TotalItems = query.Count();
    }
}