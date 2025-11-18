public class CreateLotRequest
{
    public required string name { get; set; }
    public required string location { get; set; }

    public required string adress { get; set; }

    public required int capacity { get; set; }

    public required double tarrif { get; set; }

    public required double daytarrif { get; set; }
    public required string coordinates { get; set; }

}