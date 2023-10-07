using Core.Entities;
using Infrastructure.Data.Repositories.Extensions;
using System.Linq.Dynamic.Core;
using System.Reflection;
using System.Text;
namespace Infrastructure.Data.Repositories;

public static class EmployeeRepositoryExtensions
{
    public static IQueryable<Employee> Filter(this IQueryable<Employee> employees, uint minAge, uint maxAge)
    {
        return employees.Where(e => e.Age >= minAge && e.Age <= maxAge);
    }

    public static IQueryable<Employee> Paging(this IQueryable<Employee> employees, int pageNumber, int pageSize)
    {
        return employees.Skip((pageNumber - 1) * pageSize)
                .Take(pageSize);
    }

    public static IQueryable<Employee> Search(this IQueryable<Employee> employees, string? searchTerm)
    {
        if (searchTerm is null)
            return employees;

        searchTerm = searchTerm.Trim().ToLower();

        return employees.Where(e => e.Name.ToLower().Contains(searchTerm));
    }

    public static IQueryable<Employee> Sort(this IQueryable<Employee> employees, string orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return employees.OrderBy(e => e.Name);

        string sortingQuery = UtilityExtensions.CreateSortingQuery<Employee>(orderByQueryString);

        if (String.IsNullOrWhiteSpace(sortingQuery))
            return employees.OrderBy(e => e.Name);

        // this OrderBy is from Linq.Dynamic nuget package which takes string like this "age asc, name desc" to order by it
        return employees.OrderBy(sortingQuery);
    }
}
