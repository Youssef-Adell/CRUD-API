using Core.DTOs;

namespace Core.Interfaces.IServices;

public interface ICompanyService
{
    IEnumerable<CompanyDto> GetAllCompanies(bool trackChanges);
}
