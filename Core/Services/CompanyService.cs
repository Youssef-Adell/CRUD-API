using Core.Interfaces.IRepositories;
using Core.Interfaces.IServices;
using Core.DTOs;
using Core.Entities.Exceptions;
using Core.Entities;

namespace Core.Services;

internal sealed class CompanyService : ICompanyService
{
    private readonly IRepositoryManager _repository;
    private readonly ILoggerService _logger;
    private readonly IMapperService _mapper;

    private async Task<Company> GetCompanyAndCheckIfItExists(Guid companyId, bool trackChanges)
    {
        Company company = await _repository.Company.GetCompanyAsync(companyId, trackChanges);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        return company;
    }

    public CompanyService(IRepositoryManager repository, ILoggerService logger, IMapperService mapper)
    {
        _repository = repository;
        _logger = logger;
        _mapper = mapper;
    }

    public async Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges)
    {
        IEnumerable<Company> compaines = await _repository.Company.GetAllCompaniesAsync(trackChanges);

        IEnumerable<CompanyDto> compainesDto = _mapper.Map<IEnumerable<CompanyDto>>(compaines);

        return compainesDto;
    }

    public async Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges)
    {
        Company company = await GetCompanyAndCheckIfItExists(companyId, trackChanges);

        CompanyDto companyDto = _mapper.Map<CompanyDto>(company);

        return companyDto;
    }

    public async Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto addedCompany)
    {
        if (addedCompany is null)
            throw new NullParameterBadRequestException(nameof(addedCompany));

        Company companyEntity = _mapper.Map<Company>(addedCompany);

        _repository.Company.CreateCompany(companyEntity);
        await _repository.SaveAync();

        CompanyDto companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

        return companyToReturn;
    }

    public async Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection)
    {
        //check if CompanyCollection isnt null
        if (companyCollection is null)
            throw new NullParameterBadRequestException(nameof(companyCollection));

        //map DtoForCreation To entity
        IEnumerable<Company> companyCollectionEntitiies = _mapper.Map<IEnumerable<Company>>(companyCollection);

        //add entities to db
        foreach (Company company in companyCollectionEntitiies)
            _repository.Company.CreateCompany(company);

        await _repository.SaveAync();

        //map created entiies to Dto & concatenate all ids to return them
        IEnumerable<CompanyDto> companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyCollectionEntitiies);
        string ids = string.Join(",", companyCollectionEntitiies.Select(c => c.Id));

        return (companyCollectionToReturn, ids);
    }
    public async Task<IEnumerable<CompanyDto>> GetCompanyCollectionAsync(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new NullParameterBadRequestException(nameof(ids));

        IEnumerable<Company> companyCollection = await _repository.Company.GetCompanyCollectionAsync(ids, trackChanges);

        if (companyCollection.Count() != ids.Count())
            throw new InvalidIdsParameterBadRequestException();

        IEnumerable<CompanyDto> companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyCollection);

        return companyCollectionToReturn;
    }

    public async Task DeleteCompanyAsync(Guid companyId)
    {
        Company company = await GetCompanyAndCheckIfItExists(companyId, trackChanges: false);

        _repository.Company.DeleteCompany(company);
        await _repository.SaveAync();
    }

    public async Task UpdateCompanyAsync(Guid companyId, CompanyForUpdatenDto companyForUpdate)
    {
        if (companyForUpdate is null)
            throw new NullParameterBadRequestException(nameof(companyForUpdate));

        Company company = await GetCompanyAndCheckIfItExists(companyId, trackChanges: true);

        // update the company
        _mapper.Map(companyForUpdate, company);

        await _repository.SaveAync();
    }
}
