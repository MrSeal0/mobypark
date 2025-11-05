using System.Net;
using System.Net.Cache;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;

namespace API_C_.Controllers;

[ApiController]
[Route("[controller]")]

public class PaymentsController : ControllerBase
{
    [HttpGet(Name = "Payments")]

    public void Post()
    {
        if(!Request.Headers.TryGetValue("Authorization", out var sessionKey) || !SessionManager.get_session(sessionKey))
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}