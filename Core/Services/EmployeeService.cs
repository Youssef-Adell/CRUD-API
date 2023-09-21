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

    public EmployeeDto CreateEmployeeForCompany(Guid companyId, EmployeeForCreationDto employeeToAdd, bool trackChanges = false)
    {

        //check if the company exist or not befoe add employee to it
        Company company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        //map employeeForCreationDto to employee Entity
        Employee employeeEntity = _mapper.Map<Employee>(employeeToAdd);

        //add employeeEntity to database
        _repository.Employee.CreateEmployeeForCompany(companyId, employeeEntity);
        _repository.Save();

        //map employeeEntity to employeeDto to return it
        EmployeeDto employeeToReturn = _mapper.Map<EmployeeDto>(employeeEntity);

        return employeeToReturn;
    }

}
