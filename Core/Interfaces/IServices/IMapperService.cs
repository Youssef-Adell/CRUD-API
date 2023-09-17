namespace Core.Interfaces.IServices;

public interface IMapperService
{
    TDestination Map<TDestination>(object source);
}
