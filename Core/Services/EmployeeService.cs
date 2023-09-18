using Core.DTOs;
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
}
