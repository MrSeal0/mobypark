using System.Reflection;

public class EditReservationRequest
{
    public int vehicle_id { get; set; }
    public DateTime startdate { get; set; }
    public DateTime enddate { get; set; }
    public int parkinglot { get; set; }
}