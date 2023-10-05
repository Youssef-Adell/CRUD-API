using Core.DTOs;
using Core.Entities;
using Core.Interfaces.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Data.Repositories;

public class CompanyRepository : RepositoryBase<Company>, ICompanyRepository
{
    public CompanyRepository(AppDbContext context) : base(context)
    {

    }

    public async Task<PagedList<Company>> GetAllCompaniesAsync(CompanyParameters companyParameters, bool trackChanges)
    {
        var companies = await FindAll(trackChanges)
        .OrderBy(c => c.Name)
        .Skip((companyParameters.PageNumber - 1) * companyParameters.PageSize)
        .Take(companyParameters.PageSize)
        .ToListAsync();

        int totalComapniesCount = await FindAll(trackChanges).CountAsync();

        return new PagedList<Company>(
            companies,
            companyParameters.PageNumber,
            companyParameters.PageSize,
            totalComapniesCount
        );
    }
    public async Task<Company> GetCompanyAsync(Guid companyId, bool trackChanges) =>
        await FindByCondition((c) => c.Id == companyId, trackChanges)
        .SingleOrDefaultAsync();


    public void CreateCompany(Company company) => Create(company);

    public async Task<IEnumerable<Company>> GetCompanyCollectionAsync(IEnumerable<Guid> ids, bool trackChanges) =>
        await FindByCondition(c => ids.Contains(c.Id), trackChanges)
        .ToListAsync();

    public void DeleteCompany(Company company)
    {
        Delete(company);
    }
}
