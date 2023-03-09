using RentalCompany.Infrastructure.Data;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Infrastructure.Repositories;

public class RentalStoreRepository : Repository<RentalStore>, IRentalStoreRepository
{
    private readonly ApplicationDbContext _dbContext;
    public RentalStoreRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}