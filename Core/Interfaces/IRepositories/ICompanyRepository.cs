using Core.Entities;
namespace Core.Interfaces.IRepositories;

public interface ICompanyRepository
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
}
