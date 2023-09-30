using Core.DTOs;

namespace Core.Interfaces.IServices;

public interface ICompanyService
{
    Task<IEnumerable<CompanyDto>> GetAllCompaniesAsync(bool trackChanges);
    Task<CompanyDto> GetCompanyAsync(Guid companyId, bool trackChanges);
    Task<CompanyDto> CreateCompanyAsync(CompanyForCreationDto company);
    Task<(IEnumerable<CompanyDto> companies, string ids)> CreateCompanyCollectionAsync(IEnumerable<CompanyForCreationDto> companyCollection);
    Task<IEnumerable<CompanyDto>> GetCompanyCollectionAsync(IEnumerable<Guid> ids, bool trackChanges);
    Task DeleteCompanyAsync(Guid companyId);
    Task UpdateCompanyAsync(Guid companyId, CompanyForUpdatenDto companyForUpdate);
}
