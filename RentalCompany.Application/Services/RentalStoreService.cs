using System.Linq;
using AutoMapper;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Application.Middleware.CustomExceptions;
using RentalCompany.Infrastructure.Models;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Application.Services;

public class RentalStoreService : IRentalStoreService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public RentalStoreService(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task<IEnumerable<RentalStoreDto>> GetAllRentalStores()
    {
        var allStoresCollection = _unitOfWork.RentalStore.GetAll(includeProperties: "ContactData");
        var allStoresCollectionDto = _mapper.Map<IEnumerable<RentalStoreDto>>(allStoresCollection);

        return Task.FromResult(allStoresCollectionDto);
    }

    public Task AddRentalStore(RentalStoreDto rentalStoreDto)
    {
        var rentalStoreToAddToDb = _mapper.Map<RentalStore>(rentalStoreDto);
        _unitOfWork.RentalStore.Add(rentalStoreToAddToDb);
        _unitOfWork.Save();

        return Task.CompletedTask;
    }

    public Task<RentalStoreDto> GetRentalStoreById(int? id)
    {
        if (id.GetValueOrDefault() == 0)
        {
            throw new ArgumentException("Invalid id");
        }

        var rentalStoreFromDb = _unitOfWork.RentalStore
            .GetFirstOrDefault(c => c.Id == id, includeProperties: "ContactData");

        if (rentalStoreFromDb == null)
        {
            throw new NotFoundException($"Rental Store with ID: {id} was not found in database");
        }

        var rentalStoreDto = _mapper.Map<RentalStoreDto>(rentalStoreFromDb);

        return Task.FromResult(rentalStoreDto);
    }

    public Task EditRentalStore(RentalStoreDto rentalStoreDto)
    {
        var rentalStoreToAddToDb = _mapper.Map<RentalStore>(rentalStoreDto);

        _unitOfWork.RentalStore.Update(rentalStoreToAddToDb);
        _unitOfWork.Save();

        return Task.CompletedTask;
    }

    public Task DeleteRentalStoreById(int? id)
    {
        var rentalStoreToDeleteFromDb = _unitOfWork.RentalStore.GetFirstOrDefault(u => u.Id == id);

        if (rentalStoreToDeleteFromDb == null)
        {
            throw new NotFoundException($"Rental store with ID: {id} was not found in database");
        }
        _unitOfWork.RentalStore.Remove(rentalStoreToDeleteFromDb);
        _unitOfWork.Save();

        return Task.CompletedTask;
    }

    public Task<RentalStoreSelectDto> GetSelectCarsView(int? id)
    {
        RentalStoreSelectDto rentalStoreSelectDto = new()
        {
            RentalStoreDto = GetRentalStoreById(id).Result,
            NumberOfAvailableCars = GetAvailableCars(id),
            CarsList = _mapper.Map<ICollection<CarDto>>(_unitOfWork.Car.GetAll().ToList())
        };

        return Task.FromResult(rentalStoreSelectDto);
    }

    private Dictionary<int, int> GetAvailableCars(int? id)
    {
        var result = new Dictionary<int, int>();
        var listFromDb = _unitOfWork.AvailableCar.GetAll(u => u.RentalStoreId == id);

        foreach (var element in listFromDb)
        {
            result.Add(element.CarId, element.CarsCount);
        }
        return result;
    }

    public Task AddAvailableCars(RentalStoreSelectDto rentalStoreSelectDto)
    {
        var rentalStoreId = rentalStoreSelectDto.RentalStoreDto.Id;
        var rentalStoreFromDb = _unitOfWork.RentalStore
            .GetFirstOrDefault(u => u.Id == rentalStoreId, includeProperties: "AvailableCars");
        
        rentalStoreFromDb.AvailableCars.Clear();

        if (rentalStoreSelectDto.NumberOfAvailableCars == null)
        {
            _unitOfWork.Save();
            return Task.CompletedTask;
        }

        rentalStoreFromDb.AvailableCars = new List<AvailableCar>();

        foreach (KeyValuePair<int, int> entry in rentalStoreSelectDto.NumberOfAvailableCars)
        {
            var car = new AvailableCar()
            {
                CarId = entry.Key,
                CarsCount = entry.Value,
                RentalStoreId = rentalStoreId
            };
            rentalStoreFromDb.AvailableCars.Add(car);  
        }
        _unitOfWork.Save();
        
        return Task.CompletedTask;
    }
}
