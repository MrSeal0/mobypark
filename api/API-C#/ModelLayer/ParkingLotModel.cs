using Microsoft.Extensions.Primitives;

public class ParkingLotModel
{
    public int ID { get; set; }
    public string? name { get; set; }
    public string? location { get; set; }
    public string? adress { get; set; }
    public int capacity { get; set; }
    public int reserved { get; set; }
    public double tarrif { get; set; }
    public double daytarrif { get; set; }
    public string? created_at { get; set; }
    public string? coordinates { get; set; }

}