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
            return Unauthorized("unauthorized");
        }

        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);

        paymentData.Initiator = user.username;

        _paymentLogic.CreateNewPayment(paymentData);
        return Ok("payment created");

        
    }

    [HttpPost("refund")]

    public ActionResult<string> PostRefund([FromBody] RefundRequest refundData)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized("missing authorization");
        }

        AccountModel admin = _sessionLogic.GetUserBySession(sessionKey);

        if (admin.role != "ADMIN")
        {
            return Unauthorized();
        }

        refundData.Processed_By = admin.username;

        _paymentLogic.CreateNewRefund(refundData);
        return Ok("refunded succesfully");
    }

    [HttpPost("refund/{pid}")]

    public ActionResult<string> PostRefundByID(int pid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized("missing authorization");
        }

        AccountModel admin = _sessionLogic.GetUserBySession(sessionKey);

        if (admin.role != "ADMIN")
        {
            return Unauthorized();
        }

        _paymentLogic.RefundPayment(pid, admin);



        return Ok("refunded succesfully");
    }

    //put
    [HttpPut("{pid}")]
    public ActionResult<string> PutPayment(int pid, [FromBody] TDataRequest TData)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized("no authorization");
        }

        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);
        
        PaymentModel pmodel = _paymentLogic.CompletePayment(pid);
        TData.Issuer = user.username;
        TData.Date = DateTime.Now;
        TData.Amount = pmodel.Amount;
        _paymentLogic.CompleteTData(pid, TData);


        return Ok("Finished payment");

    }

    [HttpGet("{username}")]
    public ActionResult<PaymentModel> GetUserPayments(string username)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized();
        }

        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);

        if (user.role != "ADMIN")
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

    [HttpGet("")]

    public ActionResult<List<PaymentModel>> GetMyPayments()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionLogic.GetUserBySession(sessionKey) == null)
        {
            return Unauthorized();
        }
        AccountModel user = _sessionLogic.GetUserBySession(sessionKey);
        List<PaymentModel> paymentinfo = _paymentLogic.GetPaymentsByInitiator(user.username);
        return Ok(paymentinfo);

    }
}