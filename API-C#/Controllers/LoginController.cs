using System.Net;
using System.Net.Cache;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.WebEncoders.Testing;

namespace API_C_.Controllers;

[ApiController]
[Route("[controller]")]

public class LoginController : ControllerBase
{
    [HttpPost(Name = "login")]

    public ActionResult<AuthResult> Post([FromBody] LoginRequest userinfo)
    {
        LoginLogic _logic = new();

        string? key = _logic.Login(userinfo);
        if (key is not null)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            return Ok(new AuthResult(true, key));
        }
        else
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized(new AuthResult(false));
        }
    }
}