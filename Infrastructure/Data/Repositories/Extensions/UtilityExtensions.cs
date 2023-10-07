using System.Reflection;
using System.Text;

namespace Infrastructure.Data.Repositories.Extensions;

public static class UtilityExtensions
{

    /*
        - takes orderyBy query string like that "name, age desc, lblb"
        - ensures that these properties are valid and exist
        - returns orderBy string that contains only valid properties like this "name asc, age desc"
    */
    public static string CreateSortingQuery<T>(string orderByQueryString)
    {
        // orderByQueryString is like this "name, age desc" and desc is optional
        string[] orderByParams = orderByQueryString.Trim().Split(',');

        // get properties of T type to use it for ensuring that the properties enterd in orderByQueryString are exist and valid
        PropertyInfo[] employeeProperties = typeof(T).GetProperties(BindingFlags.Instance | BindingFlags.Public);

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

        return query;
    }

}
