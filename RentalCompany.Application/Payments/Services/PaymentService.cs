﻿using RentalCompany.Application.Payments.Interfaces;

namespace RentalCompany.Application.Payments.Services;
public abstract class PaymentService<TModel> : IPaymentService
    where TModel : IPaymentModel
{
    public virtual bool AppliesTo(Type provider)
    {
        return typeof(TModel).Equals(provider);
    }

    public string MakePayment<T>(T model) where T : IPaymentModel
    {
        return MakePayment((TModel)(object)model);
    }    
    public void MakeRefund<T>(T model) where T : IPaymentModel
    {
        MakeRefund((TModel)(object)model);
    }


    public string GetPaymentStatus<T>(T model) where T : IPaymentModel
    {
        return GetPaymentStatus((TModel)(object)model); 
    }

    protected abstract string MakePayment(TModel model);
    protected abstract void MakeRefund(TModel model);
    protected abstract string GetPaymentStatus(TModel model);
}
