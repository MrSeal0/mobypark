using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic; // added
using System.Linq;    

namespace unit_testing;

public class PaymentTest
{
    [Theory]
    [InlineData("Joristest", 3)]
    [InlineData("OokJoris", 12)]

    public void PaymentGetTest(string user, int am)
    {
        var paymentaccesMock = new Mock<IPaymentAcces>();
        paymentaccesMock
            .Setup(u => u.GetPaymentsByInitiator(user)) // checks for set input
            .Returns(new List<PaymentModel> // returns set value
            {
                new(){
                    ID = 1,
                    t_data_id = 1,
                    Amount = am,
                    created_at = DateTime.Now,
                    completed_at = DateTime.Now,
                    Hash = "ujbifewuewib8iwn",
                    Initiator = user,
                    Transaction = "blabla"
                }
            });

        PaymentLogic _paymentlogic = new(paymentaccesMock.Object);

        PaymentModel payment = _paymentlogic.GetPaymentsByInitiator(user)[-1];

        Assert.True(payment.Amount == am);


    }

    [Theory]
    [InlineData(1, 10)]
    [InlineData(2, 20)]
    public void RefundPaymentTest(int id, int am)
    {
        var refundAccesMock = new Mock<IRefundAcces>();
        refundAccesMock
            .Setup(u => u.GetRefund(id))
            .Returns(am);
        
        PaymentLogic _paymentlogic = new(null, refundAccesMock.Object);

        int amount = _paymentlogic.GetRefundById(id);
        Assert.True(amount == am);
    }
}