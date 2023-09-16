namespace Core.IServices;

public interface IMapperService
{
    TDestination Map<TDestination>(object source);
}
