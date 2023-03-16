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

    public void UpdatePaymentID(int id, string sessionId, string paymentIntendId)
    {
        var rentHeaderFromDb = _dbContext.RentHeaders.FirstOrDefault(u => u.Id == id);

        rentHeaderFromDb.SessionId = sessionId;
        rentHeaderFromDb.PaymentIntendId = paymentIntendId;
        rentHeaderFromDb.PaymentDate = DateTime.Now;
    }
}