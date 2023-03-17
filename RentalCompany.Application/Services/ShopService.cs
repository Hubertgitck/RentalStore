using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using Org.BouncyCastle.Asn1.Ocsp;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Application.Payments.Interfaces;
using RentalCompany.Application.Payments.Models;
using RentalCompany.Infrastructure.Models;
using RentalCompany.Infrastructure.Repositories.Interfaces;
using RentalCompany.Utility;

namespace RentalCompany.Application.Services;

public class ShopService : IShopService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
    private readonly IPaymentStrategy _paymentStrategy;
    private readonly IEmailSender _emailSender;

    public ShopService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentStrategy paymentStrategy,
        IEmailSender emailSender)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
        _paymentStrategy = paymentStrategy;
        _emailSender = emailSender;
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

    public async Task<RentHeaderDto> GetRentHeaderById(int id)
    {
        var rentHeader = _unitOfWork.RentHeader.GetFirstOrDefault(u => u.Id == id, includeProperties: "Car,PickupRentalStore,ReturnRentalStore,ApplicationUser");
        var rentHeaderDto = _mapper.Map<RentHeaderDto>(rentHeader);

        return await Task.FromResult(rentHeaderDto);
    }

    public async Task<string> MakePayment(int id, ClaimsPrincipal userClaims)
    {
        var rentHeader = _unitOfWork.RentHeader.GetFirstOrDefault(u => u.Id == id, includeProperties:"Car");

        var userId = HelperMethods.GetApplicationUserIdFromClaimsPrincipal(userClaims);

        if (rentHeader != null)
        {
            IPaymentModel stripeModel = new StripeModel()
            {
                RentHeaderId = rentHeader.Id,
                StartDate = rentHeader.StartDate.ToString("d", CultureInfo.CurrentCulture.DateTimeFormat),
                EndDate = rentHeader.EndDate.ToString("d", CultureInfo.CurrentCulture.DateTimeFormat),
                TotalCost = rentHeader.TotalCost,
                CarName = rentHeader.Car.Name
                
            };

            var redirectUrl = _paymentStrategy.MakePayment(stripeModel);

            return await Task.FromResult(redirectUrl);
        }
        else
        {
            return await Task.FromResult("");
        }
    }

    public async Task OrderConfirmation(int id)
    {
        RentHeader rentHeader = _unitOfWork.RentHeader
            .GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser");

        var paymentStatus = _paymentStrategy.GetPaymentStatus(new StripeModel
        {
            SessionId = rentHeader.SessionId
        });

        if (paymentStatus == "paid")
        {
            _unitOfWork.RentHeader.UpdatePaymentID(id, rentHeader.SessionId, rentHeader.PaymentIntendId);
            _unitOfWork.RentHeader.UpdateStatus(id, Constants.StatusApproved, Constants.PaymentStatusApproved);
        }

        await _emailSender.SendEmailAsync(rentHeader.ApplicationUser.Email, "New Rent - Rental Store", "<p>New Tesla booked!</p>");

        _unitOfWork.Save();

        return ;
    }
}

