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

        if (rentHeaderFromDb != null)
        {
            rentHeaderFromDb.SessionId = sessionId;
            rentHeaderFromDb.PaymentIntendId = paymentIntendId;
            rentHeaderFromDb.PaymentDate = DateTime.Now;
        }
    }

    public void UpdateStatus(int id, string orderStatus, string? paymentStatus = null)
    {
        var rentHeaderFromDb = _dbContext.RentHeaders.FirstOrDefault(u => u.Id == id);
        if (rentHeaderFromDb != null)
        {
            rentHeaderFromDb.RentStatus = orderStatus;
            if (paymentStatus != null)
            {
                rentHeaderFromDb.RentPaymentStatus = paymentStatus;
            }
        }
    }
}