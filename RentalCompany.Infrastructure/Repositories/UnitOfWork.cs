using Microsoft.EntityFrameworkCore;
using RentalCompany.Infrastructure.Data;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    public readonly ApplicationDbContext _dbContext;

    public UnitOfWork(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
        ApplicationUser = new ApplicationUserRepository(_dbContext);
        AvailableCar = new AvailableCarRepository(_dbContext);
        Car = new CarRepository(_dbContext);
        ContactData = new ContactDataRepository(_dbContext);
        RentalStore = new RentalStoreRepository(_dbContext);
        RentHeader= new RentHeaderRepository(_dbContext);
    }

    public IApplicationUserRepository ApplicationUser { get; private set; }

    public IAvailableCarRepository AvailableCar { get; private set; }

    public ICarRepository Car { get; private set; }

    public IContactDataRepository ContactData { get; private set; }

    public IRentalStoreRepository RentalStore { get; private set; }

    public IRentHeaderRepository RentHeader { get; private set; }

    public void Save()
    {
        _dbContext.SaveChanges();
    }
}
