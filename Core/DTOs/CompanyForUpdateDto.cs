namespace Core.DTOs;

public record CompanyForUpdatenDto
{
    public string Name { get; init; }
    public string Address { get; init; }
    public string Country { get; init; }
    public IEnumerable<EmployeeForCreationDto> Employees { get; init; }
}
