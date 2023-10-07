using Core.DTOs;
using Core.Entities;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class EmployeeRepository : RepositoryBase<Employee>, IEmployeeRepository
{
    public EmployeeRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<PagedList<Employee>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        IQueryable<Employee> query = FindByCondition((e) => e.CompanyId.Equals(companyId), trackChanges)
            .Filter(employeeParameters.MinAge, employeeParameters.MaxAge)
            .Search(employeeParameters.SearchTerm);

        List<Employee> employees = await query
            .Sort(employeeParameters.OrderBy)
            .Paging(employeeParameters.PageNumber, employeeParameters.PageSize)
            .ToListAsync();

        int totalEmployeesCount = await query.CountAsync();

        return new PagedList<Employee>(
            employees,
            employeeParameters.PageNumber,
            employeeParameters.PageSize,
            totalEmployeesCount
        );
    }

    public async Task<Employee> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges) =>
        await FindByCondition(e => e.Id.Equals(employeeId) && e.CompanyId.Equals(companyId), trackChanges)
        .SingleOrDefaultAsync();

    public void CreateEmployeeForCompany(Guid companyId, Employee employee)
    {
        employee.CompanyId = companyId;
        Create(employee);
    }

    public void DeleteEmployee(Employee employee)
    {
        Delete(employee);
    }

}
