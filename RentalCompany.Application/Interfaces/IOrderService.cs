using System.Security.Claims;
using RentalCompany.Application.Dto;

namespace RentalCompany.Application.Interfaces;

public interface IOrderService
{
    Task<IEnumerable<RentHeaderDto>> GetAllOrders(ClaimsPrincipal user, string status);
	Task<RentHeaderDto> GetRentHeaderById(int id);
	Task CancelOrder(int id);
}
