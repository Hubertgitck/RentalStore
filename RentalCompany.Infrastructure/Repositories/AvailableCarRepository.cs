using RentalCompany.Infrastructure.Data;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Infrastructure.Repositories;

public class AvailableCarRepository : Repository<AvailableCar>, IAvailableCarRepository
{
    private readonly ApplicationDbContext _dbContext;
    public AvailableCarRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}