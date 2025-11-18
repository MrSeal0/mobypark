using System.Net;
using System.Net.Cache;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

namespace API_C_.Controllers;

[ApiController]
[Route("[controller]")]


public class PaymentsController : ControllerBase
{
    SessionLogic _sessionLogic = new();
    PaymentLogic _paymentLogic = new();

    //Post


    [HttpPost(Name = "Payments")]

    public ActionResult<string> PostPayment([FromBody] PaymentRequest paymentData)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized();
        }

        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);

        paymentData.initiator = user.username;

        _paymentLogic.CreateNewPayment(paymentData);
        return Ok("payment created");
    }

    [HttpPost("payments/refund")]

    public ActionResult<string> PostRefund([FromBody] RefundRequest refundData)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized();
        }

        AccountModel admin = _sessionLogic.GetUserBySession(sessionKey);

        if (admin.role != "Admin")
        {
            return Unauthorized();
        }

        refundData.Processed_By = admin.username;

        _paymentLogic.CreateNewRefund(refundData);
        return Ok("refunded succesfully");
    }

    //put
    [HttpPut("{pid}")]
    public ActionResult<string> PutPayment(int pid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized("no authorization");
        }
        
        _paymentLogic.CompletePayment(pid);
        return Ok("Payment succesful");

    }

    [HttpGet("{username}")]
    public ActionResult<PaymentModel> GetUserPayments(string username)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }

        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);

        if (user.role != "Admin")
        {
            return Unauthorized();
        }
        if (username == null || username == "")
        {
            return Unauthorized();
        }

        List<PaymentModel> paymentinfo = _paymentLogic.GetPaymentsByInitiator(username);

        return Ok(paymentinfo);
    }

    public ActionResult<List<PaymentModel>> GetAllPayments()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);
        List<PaymentModel> paymentinfo = _paymentLogic.GetPaymentsByInitiator(user.username);
        return Ok(paymentinfo);

    }
}