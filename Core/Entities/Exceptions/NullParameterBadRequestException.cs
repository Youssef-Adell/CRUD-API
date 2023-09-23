namespace Core.Entities.Exceptions;

public class NullParameterBadRequestException : BadRequestException
{
    public NullParameterBadRequestException(string parameterName)
    : base($"{char.ToUpper(parameterName[0]) + parameterName.Substring(1)} sent from the client is null.")
    {
    }

}
