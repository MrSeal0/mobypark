using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace API_C_.Controllers;

[ApiController]
[Route("[Controller]")]

public class ReservationsController : ControllerBase
{
    ReservationsLogic _logic = new();
    SessionLogic _sessionlogic = new();
    ParkingLotLogic _parkinglotlogic = new();


    [HttpPost]
    public ActionResult<string> CreateReservation([FromBody] ReservationRequest data)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            return Unauthorized("Invalid or missing session token");
        }

        if (!_parkinglotlogic.DoesLotExist(data.parkinglot))
        {
            return NotFound("Parking lot does not exist");
        }

        if (_sessionlogic.GetUserBySession(sessionkey).role == "ADMIN" && data.uid != default)
        {
            _logic.CreateReservation(data, data.uid);
            return Ok("Reservation created succesfully");
        }
        else
        {
            _logic.CreateReservation(data, _sessionlogic.GetUserBySession(sessionkey).ID);
            return Ok("Reservation created succesfully");
        }
    }


    [HttpPut("{rid}")]
    public ActionResult<string> EditReservation([FromBody] EditReservationRequest data, int rid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            return Unauthorized("Invalid or missing session token");
        }

        if (!_logic.DoesReservationExist(rid))
        {
            return NotFound("Reservation does not exist");
        }

        if (_sessionlogic.GetUserBySession(sessionkey).role == "ADMIN")
        {
            _logic.EditReservation(data, rid);
            return Ok("Reservation edited succesfully");
        }
        else if (_logic.IsMyResrvation(rid, _sessionlogic.GetUserBySession(sessionkey).ID))
        {
            _logic.EditReservation(data, rid);
            return Ok("Reservation edited succesfully");
        }
        else
        {
            return NotFound("Reservation does not exist");
        }
    }


    [HttpDelete("{rid}")]
    public ActionResult<string> DeleteReservation(int rid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            return Unauthorized("Invalid or missing session token");
        }

        if (_logic.IsMyResrvation(rid, _sessionlogic.GetUserBySession(sessionkey).ID) || _sessionlogic.GetUserBySession(sessionkey).role == "ADMIN")
        {
            _logic.DeleteReservation(rid);
            return Ok("Reservation succesfully deleted");
        }
        else
        {
            return NotFound("Reservation not found");
        }
    }


    [HttpGet("{rid}")]
    public ActionResult<ReservationModel> GetReservation(int rid)
    {
        if (!Request.Headers.TryGetValue("Authorization", out var sessionkey) || _sessionlogic.GetUserBySession(sessionkey) == null)
        {
            return Unauthorized("Invalid or missing session token");
        }

        if (_logic.IsMyResrvation(rid, _sessionlogic.GetUserBySession(sessionkey).ID) || _sessionlogic.GetUserBySession(sessionkey).role == "ADMIN") 
        {
            if (_logic.GetReservationById(rid) == null)
            {
                return NotFound("Reservation not found");
            }
            return Ok(_logic.GetReservationById(rid));
        }
        else
        {
            return NotFound("Reservation not found");
        }
    }
}