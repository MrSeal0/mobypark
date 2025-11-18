using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace API_C_.Controllers
{
    public class ReservationRequest
    {
        public required string Licenseplate { get; set; }
        public required DateTime Startdate { get; set; }
        public required DateTime Enddate { get; set; }
        public required int Parkinglot { get; set; }
        public string? User { get; set; }
    }

    [ApiController]
    [Route("[controller]")]
    public class ReservationsController : ControllerBase
    {
        private readonly SessionLogic _sessionLogic = new();
        private readonly ReservationLogic _logic = new();

        [HttpPost(Name = "reservations")]
        public IActionResult Post([FromBody] ReservationRequest data)
        {

            string? token = Request.Headers["Authorization"].FirstOrDefault();
            if (token is null)
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new JsonResult(new { error = "Missing session token" });
            }

            AccountModel? sessionUser;
            try
            {
                sessionUser = _sessionLogic.GetUserBySession(token);
            }
            catch
            {
                Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return new JsonResult(new { error = "Invalid session token" });
            }

            var result = _logic.CreateReservation(data, sessionUser);

            Response.StatusCode = (int)result.StatusCode;
            return new JsonResult(result.Body);
        }
    }
}
