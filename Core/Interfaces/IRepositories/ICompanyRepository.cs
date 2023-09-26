using Core.Entities;
namespace Core.Interfaces.IRepositories;

public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
    Company GetCompany(Guid companyId, bool trackChanges);
    void CreateCompany(Company company);
    IEnumerable<Company> GetCompanyCollection(IEnumerable<Guid> ids, bool trackChanges);
    void DeleteCompany(Company company);
}
