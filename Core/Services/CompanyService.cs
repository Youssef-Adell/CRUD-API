using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;
using Core.DTOs;
using Core.Entities.Exceptions;

namespace Core.Services;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerService _logger;

    private readonly IMapperService _mapper;

    public CompanyService(IRepositoryManager repository, ILoggerService logger, IMapperService mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges)
    {
        var compaines = _repository.Company.GetAllCompanies(trackChanges);

        var compainesDto = _mapper.Map<IEnumerable<CompanyDto>>(compaines);

        return compainesDto;
    }

    public CompanyDto GetCompany(Guid companyId, bool trackChanges)
    {
        var company = _repository.Company.GetCompany(companyId, trackChanges);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        var companyDto = _mapper.Map<CompanyDto>(company);

        return companyDto;
    }
}
