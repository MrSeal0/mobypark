using System.Reflection;

public class ReservationRequest
{
    public required int vehicle_id { get; set; }
    public required DateTime startdate { get; set; }
    public required DateTime enddate { get; set; }
    public required int parkinglot { get; set; }
    public int uid { get; set; }
}