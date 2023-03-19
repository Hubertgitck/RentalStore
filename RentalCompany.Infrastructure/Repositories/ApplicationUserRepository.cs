using RentalCompany.Infrastructure.Data;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Infrastructure.Repositories;

public class ApplicationUserRepository : Repository<ApplicationUser>, IApplicationUserRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ApplicationUserRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}