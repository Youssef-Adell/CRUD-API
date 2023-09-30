using Core.Entities;

namespace Core.Interfaces.IRepositories;

public interface IEmployeeRepository
{
    Task<IEnumerable<Employee>> GetEmployeesAsync(Guid companyId, bool trackChanges);
    Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    void CreateEmployeeForCompany(Guid companyId, Employee employee);
    void DeleteEmployee(Employee employee);
}
