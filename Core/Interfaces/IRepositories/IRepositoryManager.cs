namespace Core.Interfaces.IRepositories;

public interface IRepositoryManager
{
    ICompanyRepository Company { get; }
    IEmployeeRepository Employee { get; }

    Task SaveAync();
}
