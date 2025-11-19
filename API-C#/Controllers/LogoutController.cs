using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace API_C_.Controllers;

[ApiController]
[Route("[controller]")]

public class LogoutController : ControllerBase
{
    [HttpPost(Name = "logout")]

    public string Post()
    {
        SessionLogic _logic = new();


        if (Request.Headers.TryGetValue("Authorization", out var sessionkey))
        {
            _logic.EndSession(sessionkey);
            Response.StatusCode = (int)HttpStatusCode.OK;
            return "Logged out succesfully";
        }
        else
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "Unauthorized: Invalid or missing session token";
        }
    }
}