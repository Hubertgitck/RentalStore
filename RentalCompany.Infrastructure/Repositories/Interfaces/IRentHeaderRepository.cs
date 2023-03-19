namespace RentalCompany.Infrastructure.Repositories.Interfaces;
public interface IRentHeaderRepository : IRepository<RentHeader>
{
    void UpdatePaymentID(int id, string sessionId, string? paymentIntendId = null);
    void UpdateStatus(int id, string orderStatus, string? paymentStatus = null);
    void Update(RentHeader rentHeader);
}