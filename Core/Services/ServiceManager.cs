using Core.IRepositories;
using Core.IServices;

namespace Core.Services;

public class ServiceManager : IServiceManager
{
    private readonly Lazy<ICompanyService> _companyService;
    private readonly Lazy<IEmployeeService> _employeeService;

    public ServiceManager(IRepositoryManager repositoryManager, ILoggerService logger)
    {
        _companyService = new Lazy<ICompanyService>(() => new CompanyService(repositoryManager, logger));
        _employeeService = new Lazy<IEmployeeService>(() => new EmployeeService(repositoryManager, logger));
    }

    public ICompanyService CompanyService => _companyService.Value;

    public IEmployeeService EmployeeService => _employeeService.Value;

}
