namespace RentalCompany.Application.Payments.Interfaces;

public interface IPaymentStrategy
{
    string MakePayment<T>(T model) where T : IPaymentModel;
    void MakeRefund<T>(T model) where T : IPaymentModel;
    string GetPaymentStatus<T>(T model) where T : IPaymentModel;
    string GetPaymentIntentId<T>(T model) where T : IPaymentModel;
}
