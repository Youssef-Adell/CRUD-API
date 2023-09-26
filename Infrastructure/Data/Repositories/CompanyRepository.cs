using Core.Entities;
using Core.Interfaces.IRepositories;

namespace Infrastructure.Data.Repositories;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(AppDbContext context) : base(context)
    {

    }

    public IEnumerable<Company> GetAllCompanies(bool trackChanges) =>
        FindAll(trackChanges)
        .OrderBy(c => c.Name)
        .ToList();

    public Company GetCompany(Guid companyId, bool trackChanges) =>
        FindByCondition((c) => c.Id == companyId, trackChanges)
        .SingleOrDefault();


    public void CreateCompany(Company company) => Create(company);

    public IEnumerable<Company> GetCompanyCollection(IEnumerable<Guid> ids, bool trackChanges) =>
        FindByCondition(c => ids.Contains(c.Id), trackChanges)
        .ToList();

    public void DeleteCompany(Company company)
    {
        Delete(company);
    }
}
