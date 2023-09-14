using Core.IRepositories;

namespace Infrastructure.Data.Repositories;
public class RepositoryManager : IRepositoryManager
{
    private readonly AppDbContext _context;
    private readonly Lazy<CompanyRepository> _companyRepository;
    private readonly Lazy<EmployeeRepository> _EmployeeRepository;

    public RepositoryManager(AppDbContext context)
    {
        _context = context;
        _companyRepository = new Lazy<CompanyRepository>(() => new CompanyRepository(context));
        _EmployeeRepository = new Lazy<EmployeeRepository>(() => new EmployeeRepository(context));
    }

    public ICompanyRepository Company => _companyRepository.Value;

    public IEmployeeRepository Employee => _EmployeeRepository.Value;

    public void Save()
    {
        _context.SaveChanges();
    }

}
