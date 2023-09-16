using AutoMapper;
using Core.IServices;

namespace Infrastructure.Mapper;

public class AutoMapperService : IMapperService
{
    private readonly IMapper _mapper;

    public AutoMapperService(IMapper mapper)
    {
        _mapper = mapper;
    }

    public TDestination Map<TDestination>(object source)
    {
        return _mapper.Map<TDestination>(source);
    }
}
