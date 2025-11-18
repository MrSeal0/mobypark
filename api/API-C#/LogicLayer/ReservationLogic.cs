using System.Net;

namespace API_C_.Controllers
{
    public class ReservationResult
    {
        public HttpStatusCode StatusCode { get; set; }
        public object Body { get; set; } = new();
    }

    public class ParkingLot
    {
        public int Id { get; set; }
        public int Reserved { get; set; }
    }

    public class ReservationLogic
    {
        private static Dictionary<string, dynamic> _reservations = new();
        private static Dictionary<int, ParkingLot> _parkingLots = new()
        {
            { 1, new ParkingLot { Id = 1, Reserved = 0 } },
            { 2, new ParkingLot { Id = 2, Reserved = 3 } }
        };

        private Dictionary<string, dynamic> LoadReservationData()
        {
            return _reservations;
        }

        private Dictionary<int, ParkingLot> LoadParkingLotData()
        {
            return _parkingLots;
        }

        private void SaveReservationData(Dictionary<string, dynamic> reservations)
        {
            _reservations = reservations;
        }

        private void SaveParkingLotData(Dictionary<int, ParkingLot> parkingLots)
        {
            _parkingLots = parkingLots;
        }

        public ReservationResult CreateReservation(ReservationRequest data, AccountModel sessionUser)
        {
            var reservations = LoadReservationData();
            var parkingLots = LoadParkingLotData();

            if (string.IsNullOrWhiteSpace(data.Licenseplate))
                return Error(HttpStatusCode.BadRequest, "licenseplate");

            if (data.Startdate == default)
                return Error(HttpStatusCode.BadRequest, "startdate");

            if (data.Enddate == default)
                return Error(HttpStatusCode.BadRequest, "enddate");

            if (!parkingLots.ContainsKey(data.Parkinglot))
                return new ReservationResult
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Body = new { error = "Parking lot not found", field = "parkinglot" }
                };

            if (sessionUser.role == "ADMIN")
            {
                if (string.IsNullOrWhiteSpace(data.User))
                    return Error(HttpStatusCode.BadRequest, "user");
            }
            else
            {
                data.User = sessionUser.username;
            }

            string rid = (reservations.Count + 1).ToString();

            var reservation = new
            {
                id = rid,
                data.Licenseplate,
                data.Startdate,
                data.Enddate,
                data.Parkinglot,
                data.User
            };

            reservations[rid] = reservation;
            parkingLots[data.Parkinglot].Reserved++;

            SaveReservationData(reservations);
            SaveParkingLotData(parkingLots);

            return new ReservationResult
            {
                StatusCode = HttpStatusCode.Created,
                Body = new { status = "Success", reservation }
            };
        }

        private ReservationResult Error(HttpStatusCode code, string field)
        {
            return new ReservationResult
            {
                StatusCode = code,
                Body = new { error = "Required field missing", field }
            };
        }
    }
}
