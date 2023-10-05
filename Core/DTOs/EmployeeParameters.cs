namespace Core.DTOs;

public class EmployeeParameters : RequestParameters
{
    // make them unsigned integer to avoid negative numbers
    // if consumer entered negative number in the query string the value will be 0 instead
    public uint MinAge { get; set; } = 0;
    public uint MaxAge { get; set; } = int.MaxValue;

    public bool ValidAgeRange => MaxAge > MinAge;
}
