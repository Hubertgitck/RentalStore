using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.Ocsp;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Infrastructure.Models;
using RentalCompany.Infrastructure.Repositories.Interfaces;
using RentalCompany.Utility;

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
            RentalStoreName = _unitOfWork.RentalStore.GetFirstOrDefault(u => u.Id == id).Name,
            RentalStoreId = id
        };

        return await Task.FromResult(shopIndexDto);
    }

    public async Task<BookViewDto> GetBookViewByCarIdAndStoreId(int carId, int storeId)
    {
        var bookViewDto = new BookViewDto()
        {
            CarDto = _mapper.Map<CarDto>(_unitOfWork.Car.GetFirstOrDefault(u => u.Id == carId)),
            RentHeaderDto = new RentHeaderDto()
            {
                PickupRentalStore = _mapper.Map<RentalStoreDto>(
                    _unitOfWork.RentalStore.GetFirstOrDefault(u => u.Id == storeId))
            },
            RentalStores = _unitOfWork.RentalStore.GetAll().Select(u => new SelectListItem
            {
                Text = u.Name,
                Value = u.Id.ToString()
            })
        };

        return await Task.FromResult(bookViewDto);     
    }

    public async Task<int> AddOrderHeader(BookViewDto bookViewDto, ClaimsPrincipal userClaims)
    {
        var userId = HelperMethods.GetApplicationUserIdFromClaimsPrincipal(userClaims);
        var rentHeader = new RentHeader()
        {
            CarId = bookViewDto.CarDto.Id,
            Car = _unitOfWork.Car.GetFirstOrDefault(u => u.Id == bookViewDto.CarDto.Id),
            TotalCost = bookViewDto.RentHeaderDto.TotalCost,
            RentStatus = Constants.StatusPending,
            RentPaymentStatus = Constants.PaymentStatusPending,
            ApplicationUserId = userId,
            StartDate = bookViewDto.RentHeaderDto.StartDate,
            EndDate = bookViewDto.RentHeaderDto.EndDate,
            PickupRentalStoreId = bookViewDto.RentHeaderDto.PickupRentalStore.Id,
            ReturnRentalStoreId = bookViewDto.RentHeaderDto.ReturnRentalStore.Id
        };
        _unitOfWork.RentHeader.Add(rentHeader);
        _unitOfWork.Save();

        return await Task.FromResult(rentHeader.Id);
    }


}

