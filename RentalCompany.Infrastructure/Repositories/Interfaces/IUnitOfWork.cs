namespace RentalCompany.Infrastructure.Repositories.Interfaces;
public interface IUnitOfWork
{
    IApplicationUserRepository ApplicationUser { get; }
    IAvailableCarRepository AvailableCar { get; }
    ICarRepository Car { get; }
    IContactDataRepository ContactData { get; }
    IRentalStoreRepository RentalStore { get; }
    IRentHeaderRepository RentHeader { get; }

    void Save();
}