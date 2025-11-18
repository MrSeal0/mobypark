public class EditLotRequest
{
    public string? name { get; set; }
    public string? location { get; set; }

    public string? adress { get; set; }

    public int capacity { get; set; } = -1;

    public double tarrif { get; set; } = -1;

    public double daytarrif { get; set; } = -1;
    public string? coordinates { get; set; }
}