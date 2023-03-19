using System.Security.Claims;
using AutoMapper;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Application.Middleware.CustomExceptions;
using RentalCompany.Application.Payments.Interfaces;
using RentalCompany.Application.Payments.Models;
using RentalCompany.Infrastructure.Models;
using RentalCompany.Infrastructure.Repositories.Interfaces;
using RentalCompany.Utility;

namespace RentalCompany.Application.Services;

public class OrderService : IOrderService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;
	private readonly IPaymentStrategy _paymentStrategy;

	public OrderService(IUnitOfWork unitOfWork, IMapper mapper, IPaymentStrategy paymentStrategy)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
		_paymentStrategy = paymentStrategy;
	}

    public async Task<RentHeaderDto> GetRentHeaderById(int id)
    {
        var rentHeaderFromDb =_unitOfWork.RentHeader.GetFirstOrDefault(u => u.Id == id, includeProperties:"ApplicationUser,Car,PickupRentalStore,ReturnRentalStore");
        var result = _mapper.Map<RentHeaderDto>(rentHeaderFromDb);

        return await Task.FromResult(result);
    }

    public Task CancelOrder(int id)
    {
        var rentHeader = _unitOfWork.RentHeader.GetFirstOrDefault(u => u.Id == id, tracked: false);

		if (rentHeader == null)
		{
			throw new NotFoundException($"Rent Header with ID: {id} was not found in database");
		}

		if (rentHeader.RentPaymentStatus == Constants.PaymentStatusApproved)
		{
			_paymentStrategy.MakeRefund(new StripeModel { PaymentIntent = rentHeader.PaymentIntendId });
			_unitOfWork.RentHeader.UpdateStatus(rentHeader.Id, Constants.StatusCancelled, Constants.StatusRefunded);
		}
		else
		{
			_unitOfWork.RentHeader.UpdateStatus(rentHeader.Id, Constants.StatusCancelled, Constants.StatusCancelled);
		}

		_unitOfWork.Save();

		return Task.CompletedTask;
	}

	public Task<int> UpdateRentHeader(RentHeaderDto rentHeaderDto)
	{
		var rentHeaderFromDb = _unitOfWork.RentHeader.GetFirstOrDefault(
			u => u.Id == rentHeaderDto.Id,includeProperties:"ApplicationUser", tracked: false);

		if (rentHeaderFromDb == null)
		{
			throw new NotFoundException($"Rent with ID: {rentHeaderDto.Id} was not found in database");
		}

		rentHeaderFromDb.ApplicationUser.Name = rentHeaderDto.ApplicationUserDto.Name;
		rentHeaderFromDb.ApplicationUser.PhoneNumber = rentHeaderDto.ApplicationUserDto.PhoneNumber;
		rentHeaderFromDb.ApplicationUser.StreetAddress = rentHeaderDto.ApplicationUserDto.StreetAddress;
		rentHeaderFromDb.ApplicationUser.City = rentHeaderDto.ApplicationUserDto.City;
		rentHeaderFromDb.ApplicationUser.State = rentHeaderDto.ApplicationUserDto.State;
		rentHeaderFromDb.ApplicationUser.PostalCode = rentHeaderDto.ApplicationUserDto.PostalCode;

		_unitOfWork.RentHeader.Update(rentHeaderFromDb);
		_unitOfWork.Save();

		return Task.FromResult(rentHeaderFromDb.Id);
	}

	public async Task<IEnumerable<RentHeaderDto>> GetAllOrders(ClaimsPrincipal user, string status)
    {
        IEnumerable<RentHeader> rentHeadersFromDb;
        if (user.IsInRole(Constants.RoleAdmin) || user.IsInRole(Constants.RoleEmployee))
        {
            rentHeadersFromDb = _unitOfWork.RentHeader.GetAll(includeProperties: "ApplicationUser,Car");
        }
        else
        {
            var userId = HelperMethods.GetApplicationUserIdFromClaimsPrincipal(user);

            rentHeadersFromDb = _unitOfWork.RentHeader.GetAll(u => u.ApplicationUserId == userId, includeProperties: "ApplicationUser,Car");
        }

        switch (status)
        {
            case "refunded":
                rentHeadersFromDb = rentHeadersFromDb.Where(u => u.RentStatus == Constants.StatusRefunded);
                break;
            case "cancelled":
                rentHeadersFromDb = rentHeadersFromDb.Where(u => u.RentStatus == Constants.StatusCancelled);
                break;
            case "approved":
                rentHeadersFromDb = rentHeadersFromDb.Where(u => u.RentStatus == Constants.StatusApproved);
                break;
            case "pending":
                rentHeadersFromDb = rentHeadersFromDb.Where(u => u.RentStatus == Constants.StatusPending);
                break;
            default:
                break;
        }
        var result = _mapper.Map<IEnumerable<RentHeaderDto>>(rentHeadersFromDb);

        return await Task.FromResult(result);
	}
}

