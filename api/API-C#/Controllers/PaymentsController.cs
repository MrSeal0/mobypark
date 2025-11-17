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

    [HttpGet(Name = "Payments")]

    public void GetPayment([FromBody] PaymentRequest data)
    {
        if(!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);

        PaymentModel paymentinfo = _paymentAcces.GetPaymentByInitiator(user.username);

        Response.StatusCode = (int)HttpStatusCode.OK;
        return;
    }

    [HttpPost("payments/refund")]

    public void GetRefund()
    {
        if(!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);

        if(user.role == "Admin")
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            PaymentModel paymentinfo = _paymentAcces.GetPaymentByInitiator(user.username);
            return;
        }
        
    }
    

    public void Put()
    {
        if(!Request.Headers.TryGetValue("Authorization", out var sessionKey))
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        
    }

    public void Get()
    {
        if(!Request.Headers.TryGetValue("Authorization", out var sessionKey))
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        
    }
}