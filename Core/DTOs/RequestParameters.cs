namespace Core.DTOs;

public abstract class RequestParameters
{
    private const int maxPageSize = 50;
    private int _pageNumber = 1;
    private int _pageSize = 10;

    public int PageNumber
    {
        get
        {
            return _pageNumber;
        }
        set
        {
            if (value > 0)
                _pageNumber = value;
        }
    }

    public int PageSize
    {
        get
        {
            return _pageSize;
        }
        set
        {
            if (value > 0)
                _pageSize = (value > maxPageSize) ? maxPageSize : value;
        }
    }
}
