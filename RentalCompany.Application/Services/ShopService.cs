using AutoMapper;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Application.Services;

public class ShopService : IShopService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public ShopService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

    public async Task<IEnumerable<RentalStoreDto>> GetAllStores()
    {
        var rentalStoresFromDb = _unitOfWork.RentalStore.GetAll(includeProperties: "ContactData");
        var rentalStoresDtos = _mapper.Map<IEnumerable<RentalStoreDto>>(rentalStoresFromDb);
        return await Task.FromResult(rentalStoresDtos) ;
    }

    public async Task<ShopIndexDto> GetStockByRentalStoreId(int id)
    {
        List<CarDto> stockList = new();
        var availableCars = _unitOfWork.AvailableCar.GetAll(u => u.RentalStoreId == id && u.CarsCount > 0);

        foreach (var element in availableCars)
        {
            var carDto = _mapper.Map<CarDto>(_unitOfWork.Car.GetFirstOrDefault(u => u.Id == element.CarId));
            stockList.Add(carDto);
        }

        ShopIndexDto shopIndexDto = new()
        {
            CarsInStock = stockList,
            RentalStoreName = _unitOfWork.RentalStore.GetFirstOrDefault(u => u.Id == id).Name
        };

        return await Task.FromResult(shopIndexDto);
    }
}

