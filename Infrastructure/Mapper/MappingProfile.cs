using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForCtorParam("FullAddress",
                        (opt) => opt.MapFrom(src => string.Join(" ", src.Address, src.Country)));
    }
}
