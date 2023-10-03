namespace Core.DTOs;

public class PagedList<T> : List<T>
{
    public Metadata Metadata { get; set; }

    public PagedList(IEnumerable<T> items, int pageNumber, int pageSize, int totalItems)
    {
        Metadata = new Metadata
        {
            CurrentPage = pageNumber,
            PageSize = pageSize,
            TotalItems = totalItems,
            TotalPages = (int)Math.Ceiling(totalItems / (double)pageSize)
        };

        this.AddRange(items);
    }
    public PagedList(IEnumerable<T> items, Metadata metadata)
    {
        Metadata = metadata;
        this.AddRange(items);
    }
}

public class Metadata
{
    public int CurrentPage { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
    public int TotalPages { get; set; }

    public bool HasPrevious => CurrentPage > 1;
    public bool HasNext => CurrentPage < TotalPages;
}