using RentalCompany.Infrastructure.Data;
using RentalCompany.Infrastructure.Repositories.Interfaces;

namespace RentalCompany.Infrastructure.Repositories;

public class ContactDataRepository : Repository<ContactData>, IContactDataRepository
{
    private readonly ApplicationDbContext _dbContext;
    public ContactDataRepository(ApplicationDbContext dbContext) : base(dbContext)
    {
        _dbContext = dbContext;
    }
}