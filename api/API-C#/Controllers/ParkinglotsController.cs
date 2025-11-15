using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Net;

namespace API_C_.Controllers;


public class SessionRequest
{
    public required string Licenseplate { get; set; }
}

[ApiController]
[Route("parking-lots")]


public class ParkinglotsController : ControllerBase
{
    SessionLogic _sessionlogic = new();
    ParkingLotsessionslogic _parkinglotsessionslogic = new();

    [HttpPost(Name = "parking-lots")]

    [HttpPost("{lid}/sessions/start")]
    public string StartSession(string lid, [FromBody] SessionRequest data)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionlogic.GetUserBySession(sessionKey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "Invalid session";
        }

        AccountModel user = _sessionlogic.GetUserBySession(sessionKey);

        if (_parkinglotsessionslogic.IsParked(data.Licenseplate))
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "Cannot start a session when another sessions for this licesenplate is already started.";
        }
        _parkinglotsessionslogic.StartParkSession(data.Licenseplate, user.ID, Convert.ToInt32(lid));
        Response.StatusCode = (int)HttpStatusCode.OK;
        return $"Session started for {data.Licenseplate}";

    }

    [HttpPost("{lid}/sessions/stop")]
    public string StopSession(string lid, [FromBody] SessionRequest data)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "Invalid session";
        }

        if (!_parkinglotsessionslogic.IsParked(data.Licenseplate))
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "Cannot stop a session when there is no session for this licesenplate.";
        }

        _parkinglotsessionslogic.StopParkingSession(data.Licenseplate);
        Response.StatusCode = (int)HttpStatusCode.OK;
        return $"Session stopped for {data.Licenseplate}";

    }
}