namespace Core.Entities.Exceptions;

public class InvalidIdsParameterBadRequestException : BadRequestException
{
    public InvalidIdsParameterBadRequestException()
    : base("There are one or more invalid id in parameter Ids.")
    {
    }

}
