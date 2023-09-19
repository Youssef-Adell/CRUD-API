namespace Core.DTOs;

[Serializable]
public record CompanyDto(Guid Id, string Name, string FullAddress);
