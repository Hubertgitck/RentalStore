using AutoMapper;
using RentalCompany.Application.Dto;
using RentalCompany.Infrastructure.Models;

namespace RentalCompany.Automapper;

public class MappingProfile : Profile
{
    //  <source,destination>
    public MappingProfile()
    {
        CreateMap<Car, CarDto>();
        CreateMap<CarDto, Car>();

        CreateMap<ContactData, ContactDataDto>();
        CreateMap<ContactDataDto, ContactData>();

        CreateMap<RentalStore, RentalStoreDto>()
            .ForMember(dest => dest.ContactDataDto, opt => opt.MapFrom(src => src.ContactData));
        CreateMap<RentalStoreDto, RentalStore>()
            .ForMember(dest => dest.ContactData, opt => opt.MapFrom(src => src.ContactDataDto))
            .ForMember(dest => dest.PickupPlaces, opt => opt.Ignore())
            .ForMember(dest => dest.ReturnPlaces, opt => opt.Ignore());


    }


}