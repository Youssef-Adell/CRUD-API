using Core.Entities;
using Core.Interfaces.IRepositories;

namespace Infrastructure.Data.Repositories;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {

    }

    public IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges) =>
        FindByCondition((e) => e.CompanyId.Equals(companyId), trackChanges)
        .OrderBy(e => e.Name)
        .ToList();

}
