using Core.Entities;

namespace Core.IServices;

public interface ICompanyService
{
    IEnumerable<Company> GetAllCompanies(bool trackChanges);
}
