namespace Core.DTOs;

public class EmployeeParameters : RequestParameters
{
    public int MinAge { get; set; } = int.MinValue;
    public int MaxAge { get; set; } = int.MaxValue;
}
