using AutoMapper;
using Core.DTOs;
using Core.Entities;

namespace Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Company, CompanyDto>()
            .ForMember(dst => dst.FullAddress,
                        (opt) => opt.MapFrom(src => string.Join(" ", src.Address, src.Country)));

        CreateMap<Employee, EmployeeDto>();

        CreateMap<Company, CompanyForCreationDto>()
        .ReverseMap();

        CreateMap<Employee, EmployeeForCreationDto>()
        .ReverseMap();

        CreateMap<Employee, EmployeeForUpdateDto>()
        .ReverseMap();
    }
}
