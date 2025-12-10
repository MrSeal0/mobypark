using System.Net;
using System.Net.Cache;
using System.Reflection.Metadata.Ecma335;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.WebEncoders.Testing;

namespace API_C_.Controllers;

[ApiController]
[Route("[controller]")]

public class ProfileController : ControllerBase
{
    SessionLogic _sessionlogic = new();
    ProfileLogic _logic = new();

    [HttpPut]
    public ActionResult<string> ChangePassword([FromBody] ChangePasswordRequest data)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized("Invalid or missing session token");
        }

        int uid = _sessionlogic.GetUserBySession(sessionkey).ID;

        bool res = _logic.ChangePassword(data.password, uid, data.new_password);

        return res ? Ok("Password changed succesfully!") : Unauthorized("current password incorrect.");
    }


    [HttpGet]
    public ActionResult<AccountModel> GetUserInfo()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized("Invalid or missing session token");
        }

        return Ok(_logic.GetUserInfo(_sessionlogic.GetUserBySession(sessionkey).ID));
    }


    [HttpGet("{uid}")]
    public ActionResult<AccountModel> GetUserInfoUid(int uid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized("Invalid or missing session token");
        }
        if (uid == _sessionlogic.GetUserBySession(sessionkey).ID)
        {
            return Ok(_logic.GetUserInfo(uid));
        }
        else if (_sessionlogic.GetUserBySession(sessionkey).role == "ADMIN")
        {
            return Ok(_logic.GetUserInfo(uid));
        }
        else
        {
            return Unauthorized();
        }
    }
}