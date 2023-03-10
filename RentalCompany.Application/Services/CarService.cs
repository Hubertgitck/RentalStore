using AutoMapper;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Application.Services;

public class CarService : ICarService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public CarService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<IEnumerable<CarDto>> GetAllCars()
    {
        var allCarsCollection = _unitOfWork.Car.GetAll();
        var allCarsCollectionDto = _mapper.Map<IEnumerable<CarDto>>(allCarsCollection);

        return Task.FromResult(allCarsCollectionDto);
    }
}
