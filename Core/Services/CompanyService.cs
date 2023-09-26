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

    public CompanyDto CreateCompany(CompanyForCreationDto company)
    {
        if (company is null)
            throw new NullParameterBadRequestException(nameof(company));

        Company companyEntity = _mapper.Map<Company>(company);

        _repository.Company.CreateCompany(companyEntity);
        _repository.Save();

        CompanyDto companyToReturn = _mapper.Map<CompanyDto>(companyEntity);

        return companyToReturn;
    }

    public (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection)
    {
        //check if CompanyCollection isnt null
        if (companyCollection is null)
            throw new NullParameterBadRequestException(nameof(companyCollection));

        //map DtoForCreation To entity
        IEnumerable<Company> companyCollectionEntitiies = _mapper.Map<IEnumerable<Company>>(companyCollection);

        //add entities to db
        foreach (Company company in companyCollectionEntitiies)
            _repository.Company.CreateCompany(company);

        _repository.Save();

        //map created entiies to Dto & concatenate all ids to return them
        IEnumerable<CompanyDto> companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyCollectionEntitiies);
        string ids = string.Join(",", companyCollectionEntitiies.Select(c => c.Id));

        return (companyCollectionToReturn, ids);
    }
    public IEnumerable<CompanyDto> GetCompanyCollection(IEnumerable<Guid> ids, bool trackChanges)
    {
        if (ids is null)
            throw new NullParameterBadRequestException(nameof(ids));

        IEnumerable<Company> companyCollection = _repository.Company.GetCompanyCollection(ids, trackChanges);

        if (companyCollection.Count() != ids.Count())
            throw new InvalidIdsParameterBadRequestException();

        IEnumerable<CompanyDto> companyCollectionToReturn = _mapper.Map<IEnumerable<CompanyDto>>(companyCollection);

        return companyCollectionToReturn;
    }

    public void DeleteCompany(Guid companyId)
    {
        Company company = _repository.Company.GetCompany(companyId, trackChanges: false);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        _repository.Company.DeleteCompany(company);
        _repository.Save();
    }

    public void UpdateCompany(Guid companyId, CompanyForUpdatenDto companyForUpdate)
    {
        if (companyForUpdate is null)
            throw new NullParameterBadRequestException(nameof(companyForUpdate));

        Company company = _repository.Company.GetCompany(companyId, trackChanges: true);

        if (company is null)
            throw new CompanyNotFoundException(companyId);

        // update the company
        _mapper.Map(companyForUpdate, company);

        _repository.Save();
    }
}
