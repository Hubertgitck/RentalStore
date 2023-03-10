using AutoMapper;
using RentalCompany.Infrastructure.Models;

namespace RentalCompany.Application.Dto;

public class MappingProfile : Profile
{
    //  <source,destination>
    public MappingProfile()
    {
        CreateMap<Car, CarDto>();
    }
}