using System.ComponentModel.DataAnnotations;

namespace Core.DTOs;

public abstract record CompanyForManipulationDto
{
    [Required(ErrorMessage = "Company Name is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Name is 30 characters.")]
    public string Name { get; init; }

    [Required(ErrorMessage = "Address is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Address is 30 characters.")]
    public string Address { get; init; }

    [Required(ErrorMessage = " Country is a required field.")]
    [MaxLength(30, ErrorMessage = "Maximum length for the Country is 30 characters.")]
    public string Country { get; init; }

    public IEnumerable<EmployeeForCreationDto>? Employees { get; init; }
}
