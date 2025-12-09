using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using System.Collections.Generic; // added
using System.Linq;    

namespace unit_testing;

public class PaymentTest
{
    [Theory]
    [InlineData("Joristest")]
    [InlineData("OokJoris")]

    public void PaymentGetTest(string user)
    {
        var paymentaccesMock = new Mock<IPaymentAcces>();
        paymentaccesMock
            .Setup(u => u.GetPaymentsByInitiator(user)) // checks for set input
            .Returns(new List<PaymentModel> // returns set value
            {
                new(){
                    ID = 1,
                    t_data_id = 1,
                    Amount = 12.01,
                    created_at = DateTime.Now,
                    completed_at = DateTime.Now,
                    Hash = "ujbifewuewib8iwn",
                    Initiator = user,
                    Transaction = "blabla"
                }
            });

        PaymentLogic _paymentlogic = new(paymentaccesMock.Object);

        PaymentRequest PReq = new()
        {
            Transaction = "blabla",
            Amount = 12.01,
            Initiator = user
        };

        _paymentlogic.CreateNewPayment(PReq);
        PaymentModel payment = _paymentlogic.GetPaymentsByInitiator(user)[-1];

        Assert.True(payment.Initiator == user);


    }
}