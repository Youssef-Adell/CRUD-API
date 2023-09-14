using Core.IRepositories;
using Core.IServices;
using Core.Entities;

namespace Core.Services;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerService _logger;

    public CompanyService(IRepositoryManager repository, ILoggerService logger)
    {
        _repository = repository;
        _logger = logger;
    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var compaines = _repository.Company.GetAllCompanies(trackChanges);
            return compaines;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong in the{nameof(GetAllCompanies)} service method {ex}");
            throw;
        }
    }
}
