using Core.Entities;
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

}
