using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.IServices;

public interface IEmployeeService
{
    Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges);
    Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges);
    Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employee);
    Task DeleteEmployeeFromCompanyAsync(Guid companyId, Guid employeeId);
    Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid employeeId, EmployeeForUpdateDto employeeForUpdate);
    Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid employeeId);
    Task SaveEmployeeForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEnitiy);
}