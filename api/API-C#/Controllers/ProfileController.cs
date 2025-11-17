using Microsoft.AspNetCore.Mvc;
using System.Net;
using System.Net.Cache;
using System.Reflection.Metadata.Ecma335;

namespace API_C_.Controllers;

[ApiController]
[Route("profile")]
public class ProfileController : ControllerBase
{
    private readonly SessionLogic _sessionLogic = new();
    private readonly UsersAcces _usersAccess = new();

    [HttpPost]
    public IActionResult UpdateProfile([FromBody] ProfileRequest request)
    {
        
        var token = Request.Headers["Authorization"].ToString();

        if (string.IsNullOrEmpty(token))
        {
            return Unauthorized(new { message = "Unauthorized: Missing session token" });
        }

        
        var sessionUser = _sessionLogic.GetSession(token);
        if (sessionUser == null)
        {
            return Unauthorized(new { message = "Unauthorized: Invalid session token" });
        }

        
        request.Username = sessionUser.Username;

        
        if (!string.IsNullOrWhiteSpace(request.Password))
        {
            request.Password = Encryption.HashMD5(request.Password);
        }

        
        _usersAccess.SaveUserData(request);

        return Ok(new { message = "User updated successfully" });
    }
}
