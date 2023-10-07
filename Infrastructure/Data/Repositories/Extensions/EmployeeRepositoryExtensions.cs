using Core.Entities;
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

        // orderByQueryString consist of "Property dirction, Property direction" ex: "name, age desc" and direction is optional
        string[] orderByParams = orderByQueryString.Trim().Split(',');

        // get employee properties to use it for ensuring that the properties enterd in orderByQueryString are exist and valid
        PropertyInfo[] employeeProperties = typeof(Employee).GetProperties(BindingFlags.Instance | BindingFlags.Public);

        StringBuilder queryBuilder = new StringBuilder();

        // loop on all enterd properties and add the valid ones to queryBulder
        foreach (string param in orderByParams)
        {
            if (string.IsNullOrWhiteSpace(param))
                continue;

            // get property name from param which is like "age desc"
            string enterdPropertyName = param.Trim().Split(" ")[0];

            bool employeeHasTheEnterdProperty = employeeProperties.Any(
                p => p.Name.Equals(enterdPropertyName, StringComparison.InvariantCultureIgnoreCase)
            );

            if (!employeeHasTheEnterdProperty)
                continue;

            string sortingDirection = param.EndsWith(" desc") ? "descending" : "ascending";

            queryBuilder.Append($"{enterdPropertyName} {sortingDirection},");
        }

        string query = queryBuilder.ToString().TrimEnd(',', ' ');

        if (String.IsNullOrWhiteSpace(query))
            return employees.OrderBy(e => e.Name);

        // this OrderBy is from Linq.Dynamic nuget package which takes string like this "age asc, name desc" to order by it
        return employees.OrderBy(query);
    }
}
