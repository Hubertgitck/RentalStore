using Microsoft.Extensions.Options;
using RentalCompany.Application.Payments.Models;
using RentalCompany.Application.Payments.ServicesSettings;
using RentalCompany.Infrastructure.Repositories.Interfaces;
using Stripe;
using Stripe.Checkout;

namespace RentalCompany.Application.Payments.Services;

public class StripePaymentService : PaymentService<StripeModel>
{
    public IOptions<StripeSettings> _stripeSettings;
    public IUnitOfWork _unitOfWork;

    public StripePaymentService(IOptions<StripeSettings> stripeSettings, IUnitOfWork unitOfWork)
    {
        _stripeSettings = stripeSettings;
        _unitOfWork = unitOfWork;
    }

    protected override string MakePayment(StripeModel model)
    {
        var options = PrepareStripeOptions(model);

        var service = new SessionService();
        Session session = service.Create(options);

        _unitOfWork.RentHeader.UpdatePaymentID(model.RentHeaderId,
            session.Id, session.PaymentIntentId);
        _unitOfWork.Save();

        return session.Url;
    }

    protected override void MakeRefund(StripeModel model)
    {
        var options = new RefundCreateOptions
        {
            Reason = RefundReasons.RequestedByCustomer,
            PaymentIntent = model.PaymentIntentId
        };

        var service = new RefundService();
        service.Create(options);
    }

    protected override string GetPaymentStatus(StripeModel model)
    {
        var service = new SessionService();
        Session session = service.Get(model.SessionId);

        return session.PaymentStatus.ToLower();
    }

    private SessionCreateOptions PrepareStripeOptions(StripeModel model)
    {
        var options = new SessionCreateOptions

        {
            PaymentMethodTypes = new List<string>
                {
                "card",
                },
            LineItems = new List<SessionLineItemOptions>(),
            Mode = "payment",
            SuccessUrl = _stripeSettings.Value.Domain + $"customer/shop/OrderConfirmation?id={model.RentHeaderId}",
            CancelUrl = _stripeSettings.Value.Domain + $"customer/shop/index",
        };

        var sessionLineItem = new SessionLineItemOptions
        {
            PriceData = new SessionLineItemPriceDataOptions
            {
                UnitAmount = (long)(model.TotalCost! * 100),
                Currency = "usd",
                ProductData = new SessionLineItemPriceDataProductDataOptions
                {
                    Name = model.CarName,
                    Description = $"Pickup: {model.StartDate} + Return: {model.EndDate}"
                },
            },
            Quantity = 1,
        };
        options.LineItems.Add(sessionLineItem);

        return options;
    }
}
