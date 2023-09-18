using Core.Entities;

namespace Core.Interfaces.IRepositories;

public interface IEmployeeRepository
{
    IEnumerable<Employee> GetEmployees(Guid companyId, bool trackChanges);
    Employee GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
}
