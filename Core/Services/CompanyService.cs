using Core.IRepositories;
using Core.IServices;

namespace Core.Services;

public class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerService _logger;

    public CompanyService(IRepositoryManager repository, ILoggerService logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
