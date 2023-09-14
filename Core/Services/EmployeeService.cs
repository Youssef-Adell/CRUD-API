using Core.IRepositories;
using Core.IServices;

namespace Core.Services;

internal sealed class EmployeeService : IEmployeeService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerService _logger;

    public EmployeeService(IRepositoryManager repository, ILoggerService logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
