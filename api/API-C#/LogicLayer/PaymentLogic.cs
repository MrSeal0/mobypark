public class PaymentLogic
{
    PaymentAcces _paymentAccess = new();
    public PaymentModel GetPaymentByInitiator(string username)
    {
        return _paymentAccess.GetPaymentByInitiator(username);
    }

    public void CompletePayment(string username)
    {
        _paymentAccess.CompletePayment(username);
    }
}