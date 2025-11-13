using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using Microsoft.AspNetCore.Mvc;

namespace API_C_.Controllers;

public class CreateVehicleRequest()
{
    public required string licenseplate { get; set; }
}

[ApiController]
[Route("[controller]")]

public class VehiclesController : ControllerBase
{
    SessionLogic _sessionlogic = new();
    VehicleLogic _logic = new();

    [HttpPost("{lid}/entry")]
    public ActionResult<VehicleModel> GetVehicleInfo(string lid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized("Invalid or missing session token");
        }


        int vehicleid = Convert.ToInt32(lid);

        if (!_logic.DoesUserOwnCarByID(sessionkey, vehicleid))
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
            if (_logic.DoesUserOwnCarByPlate(sessionkey, data.licenseplate))
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
}