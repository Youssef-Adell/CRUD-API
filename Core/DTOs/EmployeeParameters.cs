using System.ComponentModel.DataAnnotations;

namespace Core.DTOs;

public class EmployeeParameters : RequestParameters, IValidatableObject
{

    public EmployeeParameters() => OrderBy = "Name";


    // make them unsigned integer to avoid negative numbers if consumer entered negative number in the query string the value will be 0 instead
    public uint MinAge { get; set; } = 0;
    public uint MaxAge { get; set; } = int.MaxValue;
    // public bool ValidAgeRange => MaxAge > MinAge;
    public string? SearchTerm { get; set; }

    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (MaxAge <= MinAge)
            yield return new ValidationResult("Max age should be greater than min age", new[] { nameof(MaxAge) });
    }
}
