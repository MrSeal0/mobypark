public class ReservationModel
{
    public int ID { get; set; }
    public int user_id { get; set; }
    public int parking_lot_id { get; set; }
    public int vehicle_id { get; set; }
    public DateTime start_time { get; set; }
    public DateTime end_time { get; set; }
    public string status { get; set; }
    public double cost { get; set; }
    public int transaction_id { get; set; }
}