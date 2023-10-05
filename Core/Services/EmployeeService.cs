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

    private async Task CheckIfCompanyExists(Guid companyId)
    {
        Company company = await _repository.Company.GetCompanyAsync(companyId, trackChanges: false);

        if (company is null)
            throw new CompanyNotFoundException(companyId);
    }

    private async Task<Employee> GetEmployeeAndCheckIfItExists(Guid companyId, Guid employeeId, bool trackChanges)
    {
        Employee employee = await _repository.Employee.GetEmployeeAsync(companyId, employeeId, trackChanges);

        if (employee is null)
            throw new EmployeeNotFoundException(employeeId);

        return employee;
    }

    public EmployeeService(IRepositoryManager repository, ILoggerService logger, IMapperService mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<PagedList<EmployeeDto>> GetEmployeesAsync(Guid companyId, EmployeeParameters employeeParameters, bool trackChanges)
    {
        if (!employeeParameters.ValidAgeRange)
            throw new MaxAgeRangeBadRequestException();

        await CheckIfCompanyExists(companyId);

        PagedList<Employee> employeesPagedList = await _repository.Employee.GetEmployeesAsync(companyId, employeeParameters, trackChanges);

        IEnumerable<EmployeeDto> employeesDto = _mapper.Map<IEnumerable<EmployeeDto>>(employeesPagedList);

        return new PagedList<EmployeeDto>(employeesDto, employeesPagedList.Metadata);
    }

    public async Task<EmployeeDto> GetEmployeeAsync(Guid companyId, Guid employeeId, bool trackChanges)
    {
        await CheckIfCompanyExists(companyId);

        Employee employee = await GetEmployeeAndCheckIfItExists(companyId, employeeId, trackChanges);

        var employeeDto = _mapper.Map<EmployeeDto>(employee);

        return employeeDto;
    }

    public async Task<EmployeeDto> CreateEmployeeForCompanyAsync(Guid companyId, EmployeeForCreationDto employee)
    {
        //check if not null
        if (employee is null)
            throw new NullParameterBadRequestException(nameof(employee));

        //check if the company exist or not befoe add employee to it
        await CheckIfCompanyExists(companyId);

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
        await CheckIfCompanyExists(companyId);

        // ensure that there is an employee with this id working in the company with that id
        Employee employee = await GetEmployeeAndCheckIfItExists(companyId, employeeId, trackChanges: false);

        // delete the employee
        _repository.Employee.DeleteEmployee(employee);
        await _repository.SaveAync();
    }

    public async Task UpdateEmployeeForCompanyAsync(Guid companyId, Guid employeeId, EmployeeForUpdateDto employeeForUpdate)
    {
        if (employeeForUpdate is null)
            throw new NullParameterBadRequestException(nameof(employeeForUpdate));

        // ensure that the company with this id is exist
        await CheckIfCompanyExists(companyId);

        // ensure that there is an employee with this id working in the company with that id
        Employee employee = await GetEmployeeAndCheckIfItExists(companyId, employeeId, trackChanges: true);

        // update the employee
        _mapper.Map(employeeForUpdate, employee);

        await _repository.SaveAync();
    }

    public async Task<(EmployeeForUpdateDto employeeToPatch, Employee employeeEntity)> GetEmployeeForPatchAsync(Guid companyId, Guid employeeId)
    {
        // ensure that the company with this id is exist
        await CheckIfCompanyExists(companyId);

        // ensure that there is an employee with this id working in the company with that id
        Employee employee = await GetEmployeeAndCheckIfItExists(companyId, employeeId, trackChanges: true);

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
