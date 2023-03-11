using RentalCompany.Application.Dto;

namespace RentalCompany.Application.Interfaces;

public interface IRentalStoreService
{
    Task<IEnumerable<RentalStoreDto>> GetAllRentalStores();
}
