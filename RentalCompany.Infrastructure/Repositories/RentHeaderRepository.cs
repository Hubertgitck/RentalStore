using RentalCompany.Infrastructure.Data;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Infrastructure.Repositories;

public class RentHeaderRepository : Repository<RentHeader>, IRentHeaderRepository
{
    private readonly ApplicationDbContext _dbContext;
    public RentHeaderRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}