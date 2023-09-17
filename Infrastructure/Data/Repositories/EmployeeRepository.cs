using Core.Entities;
using Core.Interfaces.IRepositories;

namespace Infrastructure.Data.Repositories;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {

    }
}
