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


    public IEnumerable<EmployeeDto> GetEmployees(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employees = _repository.Employee.GetEmployees(companyId, trackChanges);

        var employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employees);

        return employeesDto;
    }

    public EmployeeDto GetEmployee(Guid companyId, Guid employeeId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var employee = _repository.Employee.GetEmployee(companyId, employeeId, trackChanges);

        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);

        var employeeDto = _mapper.Map<EmployeeDto>(employee);

        return employeeDto;
    }

    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employee)
    {
        //check if not null
        if (employee is null)
            throw new NullParameterBadRequestException(nameof(employee));


        //check if the company exist or not befoe add employee to it
        Company company = _repository.Company.GetCompany(companyId, trackChanges: false);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        //map employeeForCreationDto to employee Entity
        Employee employeeEntity = _mapper.Map<Employee>(employee);

        //add employeeEntity to database
        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        _repository.Save();

        //map employeeEntity to employeeDto to return it
        EmployeeDto employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

    public void DeleteEmployeeFromCompany(Guid companyId, Guid employeeId)
    {
        // ensure that the company with this id is exist
        Company company = _repository.Company.GetCompany(companyId, trackChanges: false);

        if (company is null)
            throw new CompanyNotFoundException(companyId);


        // ensure that there is an employee with this id working in the company with that id
        Employee employee = _repository.Employee.GetEmployee(companyId, employeeId, trackChanges: false);

        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);

        // delete the employee
        _repository.Employee.DeleteEmployee(employee);
        _repository.Save();
    }

    public void UpdateEmployeeForCompany(Guid companyId, Guid employeeId, EmployeeForUpdateDto employeeForUpdate)
    {
        if (employeeForUpdate is null)
            throw new NullParameterBadRequestException(nameof(employeeForUpdate));

        // ensure that the company with this id is exist
        Company company = _repository.Company.GetCompany(companyId, trackChanges: false);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        // ensure that there is an employee with this id working in the company with that id
        Employee employee = _repository.Employee.GetEmployee(companyId, employeeId, trackChanges: true);

        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);

        // update the employee
        _mapper.Map(employeeForUpdate, employee);

        _repository.Save();
    }

}
