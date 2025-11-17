using System.Net;
using System.Net.Cache;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace API_C_.Controllers;

[ApiController]
[Route("[controller]")]


public class RegisterController : ControllerBase
{
    ArgonHasher hasher = new();

    [HttpPost(Name = "register")]


    public string Post([FromBody] RegisterRequest userinfo)
    {
        userinfo.password = hasher.HashPassword(userinfo.password);
        RegisterLogic logic = new();
        HttpStatusCode result = logic.CreateAccount(userinfo);
        Response.StatusCode = (int)result;


        if (result == HttpStatusCode.Conflict)
        {
            return $"Username \"{userinfo.username}\" is already in use";
        }
        else
        {
            return "Account created succesfully!";
        }
    }

    
}