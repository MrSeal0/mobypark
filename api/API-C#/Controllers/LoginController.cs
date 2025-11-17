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

    public AuthResult Post([FromBody] LoginRequest userinfo)
    {
        LoginLogic _logic = new();

        string? key = _logic.Login(userinfo);
        if (key is not null)
        {
            Response.StatusCode = (int)HttpStatusCode.OK;
            return new(true, key);
        }
        else
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return new(false);
        }
    }
}