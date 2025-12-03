public interface IPaymentLogic
{
    public List<PaymentModel> GetPaymentsByInitiator(string username);
    public void CreateNewPayment(PaymentRequest paymentData);
}

public class PaymentLogic : IPaymentLogic
{
    // je moet constructors in je logic zetten zodat je de juiste dependencies kan injecten omdat je die nooit aanroept, dus de mock classes die je in je test hebt gemaakt worden nooit gebruikt
    IPaymentAcces _paymentAccess;
    IRefundAcces _refundAccess;
    public PaymentLogic(IPaymentAcces paymentacces = null, IRefundAcces refundacces = null)
    {
        _paymentAccess = paymentacces ?? new PaymentAcces();
        _refundAccess = refundacces ?? new RefundAcces();
    }

    public List<PaymentModel> GetPaymentsByInitiator(string username)
    {
        return _paymentAccess.GetPaymentsByInitiator(username);
    }

    public void CreateNewPayment(PaymentRequest paymentData)
    {
        _paymentAccess.CreateNewPayment(paymentData);
    }

    public void CreateNewRefund(RefundRequest refundData)
    {
        _refundAccess.CreateNewRefund(refundData);
    }
    public PaymentModel CompletePayment(int pid)
    {
        _paymentAccess.CompletePayment(pid);
        return _paymentAccess.GetInfoForTData(pid);
    }

    public void CompleteTData(int pid, TDataRequest tdata)
    {
        _paymentAccess.CompleteTData(pid, tdata);
    }


    public void RefundPayment(int pid, AccountModel admin)
    {
        PaymentModel pmodel = _paymentAccess.GetPaymentByID(pid);
        RefundRequest RR = new()
        {
            Amount = pmodel.Amount,
            Coupled_To = pmodel.Initiator,
            Processed_By = admin.username
        };
        _refundAccess.RefundPayment(pid, RR);

    }
}