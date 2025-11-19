public class ParkingSessionModel
{
    public int ID { get; set; }
    public int parking_lot_id { get; set; }
    public int user_id { get; set; }
    public string? license_plate { get; set; }
    public DateTime start_time { get; set; }
    public DateTime end_time { get; set; }
    public int duration_minutes { get; set; }
    public double cost { get; set; }
    public string? payment_status { get; set; }
}