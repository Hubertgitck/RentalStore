using System.Globalization;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
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
        var rentHeaderFromDb = _unitOfWork.RentHeader.GetFirstOrDefault(u => u.Id == id, includeProperties: "Car,PickupRentalStore,ReturnRentalStore,ApplicationUser");
        var rentHeaderDto = _mapper.Map<RentHeaderDto>(rentHeaderFromDb);

        return await Task.FromResult(rentHeaderDto);
    }

    public async Task<string> MakePayment(int id, ClaimsPrincipal userClaims)
    {
        var rentHeaderFromDb = _unitOfWork.RentHeader.GetFirstOrDefault(u => u.Id == id, includeProperties:"Car");

        var userId = HelperMethods.GetApplicationUserIdFromClaimsPrincipal(userClaims);

        if (!IsTotalCostCorrect(rentHeaderFromDb))
        {
            return await Task.FromResult("");
        }

        if (rentHeaderFromDb != null)
        {
            IPaymentModel stripeModel = new StripeModel()
            {
                RentHeaderId = rentHeaderFromDb.Id,
                StartDate = rentHeaderFromDb.StartDate.ToString("d", CultureInfo.CurrentCulture.DateTimeFormat),
                EndDate = rentHeaderFromDb.EndDate.ToString("d", CultureInfo.CurrentCulture.DateTimeFormat),
                TotalCost = rentHeaderFromDb.TotalCost,
                CarName = rentHeaderFromDb.Car.Name
            };

            var redirectUrl = _paymentStrategy.MakePayment(stripeModel);

            return await Task.FromResult(redirectUrl);
        }
        else
        {
            return await Task.FromResult("");
        }
    }

    private bool IsTotalCostCorrect(RentHeader rentHeader)
    {
        double totalCost = 0;
        
        for(var date = rentHeader.StartDate; date <= rentHeader.EndDate; date = date.AddDays(1))
        {
            totalCost += rentHeader.Car.DayRentalPrice;
        }
        return totalCost == rentHeader.TotalCost;
    }

    public async Task OrderConfirmation(int id)
    {
        RentHeader rentHeader = _unitOfWork.RentHeader
            .GetFirstOrDefault(u => u.Id == id, includeProperties: "ApplicationUser");
        var model = new StripeModel
		{
			SessionId = rentHeader.SessionId
		};

        var paymentStatus = _paymentStrategy.GetPaymentStatus(model);

        if (paymentStatus == "paid")
        {
            _unitOfWork.RentHeader.UpdatePaymentID(id, rentHeader.SessionId, _paymentStrategy.GetPaymentIntentId(model));
            _unitOfWork.RentHeader.UpdateStatus(id, Constants.StatusApproved, Constants.PaymentStatusApproved);
        }

        await _emailSender.SendEmailAsync(rentHeader.ApplicationUser.Email, "New Rent - Mallorca Rent-a-Car ", "<p>New Tesla booked!</p>");

        _unitOfWork.Save();

        return ;
    }

	public async Task<IEnumerable<string>> GetCarAvalabilityByIdAndStore(int carId, int storeId) 
    {
		var unavailableList = new List<string>();
		var bookingsByDate = new Dictionary<DateTime, int>();

		var startingDate = DateTime.Now;
        var endingDate = DateTime.Now.AddMonths(Constants.MonthsToCheckInAdvance);

        var rentHeaders = _unitOfWork.RentHeader.GetAll(u => u.PickupRentalStoreId == storeId && u.CarId == carId 
            && u.RentStatus != Constants.StatusCancelled && u.RentStatus != Constants.StatusRefunded);
        var carAvailability = _unitOfWork.AvailableCar.GetFirstOrDefault(u => u.RentalStoreId == storeId && u.CarId == carId);

        if (rentHeaders != null)
        {
			foreach (var rentHeader in rentHeaders)
			{
				var currentDate = rentHeader.StartDate;

				while (currentDate <= rentHeader.EndDate)
				{
					if (bookingsByDate.ContainsKey(currentDate))
					{
						bookingsByDate[currentDate]++;
					}
					else
					{
						bookingsByDate[currentDate] = 1;
					}
					currentDate = currentDate.AddDays(1);
				}
			}

			for (var date = startingDate.Date; date <= endingDate; date = date.AddDays(1))
			{
				if (bookingsByDate.ContainsKey(date) && bookingsByDate[date] >= carAvailability.CarsCount)
				{
					unavailableList.Add(FormatDateForDatepicker(date));
				}
			}
		}
		return await Task.FromResult(unavailableList);
    }
	private string FormatDateForDatepicker(DateTime date)
	{
		return DateTime.ParseExact(date.ToShortDateString(), 
            Constants.DotNetDateOriginalFormat, CultureInfo.InvariantCulture)
            .ToString(Constants.DatepickerDateFormat);
	}
}

