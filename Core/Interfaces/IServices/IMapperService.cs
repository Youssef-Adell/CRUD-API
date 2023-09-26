namespace Core.Interfaces.IServices;

public interface IMapperService
{
    TDestination Map<TDestination>(object source);
    TDestination Map<TSource, TDestination>(TSource source, TDestination destination);
}
