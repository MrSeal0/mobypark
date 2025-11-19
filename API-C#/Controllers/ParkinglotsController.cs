using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Net;

namespace API_C_.Controllers;

[ApiController]
[Route("parking-lots")]


public class ParkinglotsController : ControllerBase
{
    SessionLogic _sessionlogic = new();
    ParkingLotsessionslogic _parkinglotsessionslogic = new();
    ParkingLotLogic _logic = new();

    [HttpPost(Name = "parking-lots")]

    public ActionResult<string> CreateParkingLot([FromBody] CreateLotRequest data)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null || _sessionlogic.GetUserBySession(sessionkey).role != "ADMIN")
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "Unauthorized: Invalid or missing session token";
        }

        _logic.CreateParkingLot(data);
        return Ok($"Successfully created lot: {data.name}");
    }

    [HttpPost("{lid}/sessions/start")]
    public string StartSession(string lid, [FromBody] ParkingSessionRequest data)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionKey) || _sessionlogic.GetUserBySession(sessionKey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return "Unauthorized: Invalid or missing session token";
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
    public string StopSession(string lid, [FromBody] ParkingSessionRequest data)
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

    [HttpPut("{lid}")]
    public ActionResult<string> EditParkinglotInfo(int lid, [FromBody] EditLotRequest data)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null || _sessionlogic.GetUserBySession(sessionkey).role != "ADMIN")
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized("Invalid or missing session token");
        }

        _logic.EditParkingLot(lid, data);
        return Ok("Parking lot changed succesfully");
    }

    [HttpDelete("{lid}/sessions/{sid}")]
    public ActionResult<string> DeleteParkingSession(int lid, int sid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null || _sessionlogic.GetUserBySession(sessionkey).role != "ADMIN")
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized("Invalid or missing session token");
        }

        _parkinglotsessionslogic.DeleteParkingSession(sid);
        return Ok("Session deleted succesfully");
    }

    [HttpDelete("{lid}")]
    public ActionResult<string> DeleteParkinglot(int lid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null || _sessionlogic.GetUserBySession(sessionkey).role != "ADMIN")
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized("Invalid or missing session token");
        }

        _logic.DeleteParkingLot(lid);
        return Ok("Session deleted succesfully");
    }


    [HttpGet]
    public ActionResult<List<ParkingLotModel>> GetAllParkingLots()
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized("Invalid or missing session token");
        }

        return _logic.GetAllLots();
    }


    [HttpGet("{lid}/sessions")]
    public ActionResult<List<ParkingLotModel>> GetAllParkingLots(int lid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized("Invalid or missing session token");
        }

        if (_sessionlogic.GetUserBySession(sessionkey).role != "ADMIN")
        {
            return Ok(_parkinglotsessionslogic.GetAllSessionsForLot(lid));
        }

        return Ok(_parkinglotsessionslogic.GetUserSessionsForLot(lid, _sessionlogic.GetUserBySession(sessionkey).ID));
    }


    [HttpGet("{lid}/sessions/{sid}")]

    public ActionResult<List<ParkingLotModel>> GetSession(int sid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            return Unauthorized("Invalid or missing session token");
        }

        if (_sessionlogic.GetUserBySession(sessionkey).role == "ADMIN")
        {
            ParkingSessionModel admin_result = _parkinglotsessionslogic.GetSession(sid);
            if (admin_result == null)
            {
                return NotFound("Session could not be found");
            }
            else
            {
                return Ok(admin_result);
            }
        }

        ParkingSessionModel user_result = _parkinglotsessionslogic.GetSessionFromUser(sid, _sessionlogic.GetUserBySession(sessionkey).ID);
        if (user_result == null)
        {
            return NotFound("Session could not be found");
        }
        else
        {
            return Ok(user_result);
        }
    }
}