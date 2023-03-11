using RentalCompany.Infrastructure.Data;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Infrastructure.Repositories;

public class CarRepository : Repository<Car>, ICarRepository
{
    private readonly ApplicationDbContext _dbContext;
    public CarRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }

    public void Update(Car car)
    {
        _dbContext.Cars.Update(car);
    }
}
