using Core.DTOs;
using Core.Entities;
using Core.Entities.Exceptions;
using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;

namespace Core.Services;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerService _logger;
    private readonly IMapperService _mapper;

    public EmployeeService(IRepositoryManager repository, ILoggerService logger, IMapperService mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }


    public async Task<IEnumerable<EmployeeDto>> GetEmployeesAsync(Guid companyId, bool trackChanges)
    {
        Company company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        IEnumerable<Employee> employees = await _repository.Employee.GetEmployeesAsync(companyId, trackChanges);

        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        return employeesDto;
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        Company company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        Employee employee = await _repository.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);

        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);

        var employeeDto = _mapper.Map<EmployeeDto>(employee);

        return employeeDto;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employee)
    {
        //check if not null
        if (employee is null)
            throw new NullParameterBadRequestException(nameof(employee));


        //check if the company exist or not befoe add employee to it
        Company company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        //map employeeForCreationDto to employee Entity
        Employee employeeEntity = _mapper.Map<Employee>(employee);

        //add employeeEntity to database
        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        await _repository.SaveAync();

        //map employeeEntity to employeeDto to return it
        EmployeeDto employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public async Task DeleteEmployeeFromCompanyAsync(Guid companyId, Guid employeeId)
    {
        // ensure that the company with this id is exist
        Company company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);

        if (company is null)
            throw new CompanyNotFoundException(companyId);


        // ensure that there is an employee with this id working in the company with that id
        Employee employee = await _repository.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges: false);

        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);

        // delete the employee
        _repository.Employee.DeleteEmployee(employee);
        await _repository.SaveAync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid employeeId, EmployeeForUpdateDto employeeForUpdate)
    {
        if (employeeForUpdate is null)
            throw new NullParameterBadRequestException(nameof(employeeForUpdate));

        // ensure that the company with this id is exist
        Company company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        // ensure that there is an employee with this id working in the company with that id
        Employee employee = await _repository.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges: true);

        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);

        // update the employee
        _mapper.Map(employeeForUpdate, employee);

        await _repository.SaveAync();
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid employeeId)
    {
        // ensure that the company with this id is exist
        Company company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        // ensure that there is an employee with this id working in the company with that id
        Employee employee = await _repository.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges: true);

        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);

        // Get EmployeeForUpdate Dto to applay patch to it
        EmployeeForUpdateDto employeeToPatch = _mapper.Map<EmployeeForUpdateDto>(employee);

        return (employeeToPatch, employee);
    }

    public async Task SaveEmployeeForPatchAsync(EmployeeForUpdateDto employeeToPatch, Employee employeeEnitiy)
    {
        _mapper.Map(employeeToPatch, employeeEnitiy);
        await _repository.SaveAync();
    }
}
