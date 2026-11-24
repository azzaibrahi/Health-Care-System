namespace Health_Care_System.Results;

/// <summary>
/// One page of a larger result set.
/// </summary>
/// <typeparam name="T">Type of the items in the page.</typeparam>
public class PaginatedList<T>
{
    private const int MaxPageSize = 20;
    private int _pageSize = 10;

    public int PageNumber { get; set; } = 1;

    public int TotalRecords { get; set; }

    public int TotalPages { get; set; }

    public int PageSize
    {
        get => _pageSize;
        set => _pageSize = value > MaxPageSize ? MaxPageSize : value;
    }

    public IEnumerable<T>? Data { get; set; }
}
