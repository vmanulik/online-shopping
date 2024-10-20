namespace OnlineShopping.Shared.Domain.Entities;

public class PaginatedList<T>
{
    public IReadOnlyCollection<T> Items { get; }
    public int PageNumber { get; }
    public int TotalPages { get; }
    public int TotalCount { get; }

    public PaginatedList(IReadOnlyCollection<T> items, int pageNumber, int pageSize)
    {
        PageNumber = pageNumber;
        TotalPages = (int) Math.Ceiling(items.Count / (double) pageSize);
        TotalCount = items.Count;
        Items = items;
    }
}
