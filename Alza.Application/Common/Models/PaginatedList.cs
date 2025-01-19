namespace Application.Common.Models;

public class PaginatedList<T>
{
    public List<T> Items { get; }
    public int PageNumber { get; }
    public int PageSize { get; }

    public PaginatedList(List<T> items, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        Items = items;
        PageSize = pageSize;
    }
}