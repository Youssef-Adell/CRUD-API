using Core.IRepositories;
using Core.IServices;
using Core.DTOs;

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

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        try
        {
            var compaines = _repository.Company.GetAllCompanies(trackChanges);

            var compainesDto = compaines.Select(c =>
                                new CompanyDto(c.Id, c.Name, string.Join(' ', c.Address, c.Country)));

            return compainesDto;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Something went wrong in the{nameof(GetAllCompanies)} service method {ex}");
            throw;
        }
    }
}
