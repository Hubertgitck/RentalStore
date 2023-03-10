using AutoMapper;
using RentalCompany.Application.Dto;
using RentalCompany.Infrastructure.Models;

namespace RentalCompany.Dto;

public class MappingProfile : Profile
{
    //  <source,destination>
    public MappingProfile()
    {
        CreateMap<Car, CarDto>();
        CreateMap<CarDto, Car>();
    }
}