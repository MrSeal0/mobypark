public class PaymentLogic
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
    public void CompletePayment(int pid)
    {
        _paymentAccess.CompletePayment(pid);
    }
}