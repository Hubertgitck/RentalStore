using RentalCompany.Application.Dto;

namespace RentalCompany.Application.Interfaces;

public interface IShopService
{
    Task<ShopIndexDto> GetStockByRentalStoreId(int id);
    Task<IEnumerable<RentalStoreDto>> GetAllStores();
}
