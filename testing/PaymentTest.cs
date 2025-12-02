using System.Transactions;
using Dapper;
using System.Reflection;
using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using API_C_.Controllers;
using System.Xml.Serialization;
using Microsoft.AspNetCore.Mvc;



namespace testing;
[TestClass]
public class PaymentTest
{
    PaymentLogic _paymentLogic = new();

    [TestMethod]
    [DataRow("JorisTest", 10)]
    [DataRow("OokJoris", 12)]
    public void PaymentGetTest(string user, int am)
    {
        PaymentRequest PReq = new()
        {
            Transaction = "blabla",
            Amount = am,
            Initiator = user
        };

        _paymentLogic.CreateNewPayment(PReq);

        ActionResult<List<PaymentModel>> payments = _paymentLogic.GetPaymentsByInitiator(user);
        Assert.IsTrue(payments.Result is OkObjectResult);

        PaymentModel lastpayment = payments.Value[-1];
        Assert.IsTrue(lastpayment.Amount == am && lastpayment.Transaction == "blabla");
    }
}