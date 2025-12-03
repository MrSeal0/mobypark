public interface IPaymentLogic
{
    public List<PaymentModel> GetPaymentsByInitiator(string username);
    public void CreateNewPayment(PaymentRequest paymentData);
}

public class PaymentLogic : IPaymentLogic
{
    PaymentAcces _paymentAccess = new();
    RefundAcces _refundAccess = new();
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