using Core.DTOs;
using Core.Entities;

namespace Core.Interfaces.IServices;

public interface IEmployeeService
{
    IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges);
    EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges);
    EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee);
    void DeleteEmployeeFromCompany(Guid companyId, Guid employeeId);
    void UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeForUpdateDto employeeForUpdate);
    (EmployeeForUpdateDto employeeToPatch, Employee employeeEntity) GetEmployeeForPatch(Guid companyId, Guid employeeId);
    void SaveEmployeeForPatch(EmployeeForUpdateDto employeeToPatch, Employee employeeEnitiy);
}