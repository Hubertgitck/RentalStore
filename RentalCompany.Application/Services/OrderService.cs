using System.Security.Claims;
using AutoMapper;
using RentalCompany.Application.Dto;
using RentalCompany.Application.Interfaces;
using RentalCompany.Infrastructure.Models;
using RentalCompany.Infrastructure.Repositories;
using RentalCompany.Infrastructure.Repositories.Interfaces;
using RentalCompany.Utility;

namespace RentalCompany.Application.Services;

public class OrderService : IOrderService
{
	private readonly IUnitOfWork _unitOfWork;
	private readonly IMapper _mapper;

	public OrderService(IUnitOfWork unitOfWork, IMapper mapper)
	{
		_unitOfWork = unitOfWork;
		_mapper = mapper;
	}

    public async Task<RentHeaderDto> GetRentHeaderById(int id)
    {
        var rentHeaderFromDb =_unitOfWork.RentHeader.GetFirstOrDefault(u => u.Id == id, includeProperties:"ApplicationUser,Car,PickupRentalStore,ReturnRentalStore");
        var result = _mapper.Map<RentHeaderDto>(rentHeaderFromDb);

        return await Task.FromResult(result);
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

