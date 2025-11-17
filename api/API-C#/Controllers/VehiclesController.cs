using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;

namespace API_C_.Controllers;

[ApiController]
[Route("[controller]")]

public class VehiclesController : ControllerBase
{
    SessionLogic _sessionlogic = new();
    VehicleLogic _logic = new();


    // NOT RIGHT OOPS
    [HttpPost("{lid}/entry")]
    public ActionResult<VehicleModel> GetVehicleInfo(string lid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized("Invalid or missing session token");
        }


        int vehicleid = Convert.ToInt32(lid);

        if (!_logic.DoesUserOwnCar(sessionkey, vehicleid))
        {
            return Unauthorized("Vehicle does not exist");
        }

        return _logic.GetVehicleByID(vehicleid);
    }

    [HttpPost(Name = "vehicles")]
    public async Task<string> CreateVehicle([FromBody] CreateVehicleRequest data)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "Unauthorized: Invalid or missing session token";
        }

        data.licenseplate.Replace("-", "");


        if (_logic.IsVehicleRegisterd(data.licenseplate))
        {
            if (_logic.DoesUserOwnCar(sessionkey, data.licenseplate))
            {
                Response.StatusCode = (int)HttpStatusCode.Conflict;
                return "Conflict: This license plate is already registerd to your account.";
            }
            else
            {
                Response.StatusCode = (int)HttpStatusCode.Conflict;
                return "Conflict: This license plate is already registerd to a different user.";
            }
        }

        if (await _logic.CreateVehicle(data.licenseplate, sessionkey))
        {
            Response.StatusCode = (int)HttpStatusCode.Created;
            return "Vehicle added";
        }
        else
        {
            Response.StatusCode = (int)HttpStatusCode.BadRequest;
            return "Invalid license plate";
        }
    }

    [HttpPut("{lid}")]

    public ActionResult<string> ChangeCarNickname(int lid, [FromBody] CarNickNameRequest req)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "Unauthorized: Invalid or missing session token";
        }

        if (!_logic.DoesUserOwnCar(sessionkey, lid))
        {
            return Unauthorized("Vehicle does not exist");
        }

        _logic.ChangeCarNickname(lid, req.name);
        return Ok("Nickname changed succesfully!");
    }

    [HttpDelete("{lid}")]

    public ActionResult<string> DeleteCar(int lid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "Unauthorized: Invalid or missing session token";
        }

        if (!_logic.DoesUserOwnCar(sessionkey, lid))
        {
            return Unauthorized("Vehicle does not exist");
        }

        _logic.DeleteCar(lid);
        return Ok("Car deleted succesfully!");
    }
}