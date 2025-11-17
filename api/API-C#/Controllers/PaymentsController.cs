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
    PaymentAcces _paymentAcces = new();

    //Post

    [HttpPost(Name = "Payments")]

    public ActionResult<string> GetPayment([FromBody] PaymentRequest data)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized("invalid token");
        }

        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);

        PaymentModel paymentinfo = _paymentAcces.GetPaymentByInitiator(user.username);

        Response.StatusCode = (int)HttpStatusCode.OK;
        return Ok(paymentinfo);
    }

    [HttpPost("payments/refund")]

    public ActionResult<PaymentModel> GetRefund()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);

        if (user.role != "Admin")
        {
            return Unauthorized("");
        }

        PaymentModel paymentinfo = _paymentAcces.GetPaymentByInitiator(user.username);
        return Ok(paymentinfo);
    }


    public void Put()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey))
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

    }

    public void Get()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey))
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

    }
}