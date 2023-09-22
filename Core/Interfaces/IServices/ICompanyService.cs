using Core.DTOs;

namespace Core.Interfaces.IServices;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
    CompanyDto GetCompany(Guid companyId, bool trackChanges);
    CompanyDto CreateCompany(CompanyForCreationDto company);
    (IEnumerable<CompanyDto> companies, string ids) CreateCompanyCollection(IEnumerable<CompanyForCreationDto> companyCollection);
    IEnumerable<CompanyDto> GetCompanyCollection(IEnumerable<Guid> ids, bool trackChanges = false);
}
