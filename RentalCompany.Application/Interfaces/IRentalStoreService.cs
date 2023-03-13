using RentalCompany.Application.Dto;

namespace RentalCompany.Application.Interfaces;

public interface IRentalStoreService
{
    Task<IEnumerable<RentalStoreDto>> GetAllRentalStores();
    Task AddRentalStore(RentalStoreDto rentalStoreDto);
    Task<RentalStoreDto> GetRentalStoreById(int? id);
    Task EditRentalStore(RentalStoreDto rentalStoreDto);
    Task DeleteRentalStoreById(int? id);
    Task<RentalStoreSelectDto> GetSelectCarsView(int? id);
    Task AddAvailableCars(RentalStoreSelectDto rentalStoreSelectDto);
}
