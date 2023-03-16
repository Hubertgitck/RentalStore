using System.Security.Claims;
using RentalCompany.Application.Dto;

namespace RentalCompany.Application.Interfaces;

public interface IShopService
{
    Task<ShopIndexDto> GetStockByRentalStoreId(int id);
    Task<IEnumerable<RentalStoreDto>> GetAllStores();
    Task<BookViewDto> GetBookViewByCarIdAndStoreId(int id, int storeId);
    Task<int> AddOrderHeader(BookViewDto bookViewDto, ClaimsPrincipal userClaims);
}
