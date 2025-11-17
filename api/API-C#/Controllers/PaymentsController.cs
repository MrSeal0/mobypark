using System.Net;
using System.Net.Cache;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

namespace API_C_.Controllers;

public class PaymentRequest
{

}

[ApiController]
[Route("[controller]")]

public class PaymentsController : ControllerBase
{
    SessionLogic _sessionLogic = new();
    PaymentLogic _paymentLogic = new();

    //Post

    [HttpPost(Name = "Payments")]

    public ActionResult<PaymentModel> PostPayment()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized();
        }

        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);

        PaymentModel paymentinfo = _paymentLogic.GetPaymentByInitiator(user.username);

        return Ok(paymentinfo);
    }

    [HttpPost("payments/refund")]

    public ActionResult<PaymentModel> PostRefund()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized();
        }

        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);

        if (user.role != "Admin")
        {
            return Unauthorized();
        }

        PaymentModel paymentinfo = _paymentLogic.GetPaymentByInitiator(user.username);
        return Ok(paymentinfo);
    }


    public ActionResult<string> PutPayment([FromBody] PaymentRequest data)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized("no authorization");
        }
        
        

        return Ok("Payment succesful");

    }

    public void GetPayment()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey))
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

    }
}