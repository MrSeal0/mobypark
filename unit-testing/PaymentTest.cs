using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic; // added
using System.Linq;    

namespace unit_testing;

public class PaymentTest
{
    [Theory]
    [InlineData("Joristest", 10)]
    [InlineData("OokJoris", 12)]

    public void PaymentGetTest(string user, int am)
    {
        var paymentLogicMock = new Mock<IPaymentLogic>();
        PaymentRequest PReq = new()
        {
            Transaction = "blabla",
            Amount = am,
            Initiator = user
        };
        
        paymentLogicMock.Setup(p => p.CreateNewPayment(It.IsAny<PaymentRequest>())).Verifiable();

        var list = new List<PaymentModel>
        {
            new PaymentModel { Transaction = "blabla", Amount = am, Initiator = user }
        };

        paymentLogicMock.Setup(p => p.GetPaymentsByInitiator(user)).Returns(new List<PaymentModel>(list));

        paymentLogicMock.Object.CreateNewPayment(PReq);
        List<PaymentModel> payments = paymentLogicMock.Object.GetPaymentsByInitiator(user);

        PaymentModel lastpayment = payments.Last();
        Assert.True(lastpayment.Amount == am && lastpayment.Transaction == "blabla");
    }
}