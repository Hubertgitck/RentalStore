using RentalCompany.Application.Payments.Interfaces;

namespace RentalCompany.Application.Payments.Models;

public class StripeModel : IPaymentModel
{
    public int RentHeaderId { get; set; }
    public string? PaymentIntentId { get; set; }
    public string? SessionId { get; set; }
    public string? PaymentStatus { get; set; }
    public double? TotalCost { get; set; }
    public string? CarName { get; set; }
    public string StartDate { get; set; }
    public string EndDate { get; set; }
}   
