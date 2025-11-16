using System.Net;
using System.Net.Cache;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;

namespace API_C_.Controllers;

public class RegisterRequest
{
    public required string username { get;  set; }
    public required string password { get;  set; }
    public required string name { get;  set; }
    public required string email { get;  set; }
    public required string phone { get;  set; }
    public string role { get;  set; } = "USER";
    public DateTime cdate { get;  set; } = DateTime.Now;
    public required int byear { get;  set; }
    public int active { get;  set; } = 1;
}

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