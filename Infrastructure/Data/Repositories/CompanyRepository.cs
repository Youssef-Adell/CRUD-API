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
}
