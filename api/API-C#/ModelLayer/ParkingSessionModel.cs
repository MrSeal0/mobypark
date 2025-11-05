public class ParkingSessionModel
{
    int ID { get; set; }
    int parking_lot_id { get; set; }
    int user_id { get; set; }
    string? license_plate { get; set; }
    DateTime start_time { get; set; }
    DateTime end_time { get; set; }
    int duration_minutes { get; set; }
    double cost { get; set; }
    string payment_status { get; set; }
}