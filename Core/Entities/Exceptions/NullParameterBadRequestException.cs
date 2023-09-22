namespace Core.Entities.Exceptions;

public class NullParameterBadRequestException : BadRequestException
{
    public NullParameterBadRequestException(string parameterName)
    : base($"Parameter {parameterName} is null.")
    {
    }

}
