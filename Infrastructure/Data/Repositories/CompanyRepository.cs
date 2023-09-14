using Core.Entities;
using Core.IRepositories;

namespace Infrastructure.Data.Repositories;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(AppDbContext context) : base(context)
    {

    }
}
