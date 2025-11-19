public class TDataRequest()
{
    public double Amount {get; set;}
    public required string Method {get; set;}
    public DateTime Date {get; set;}
    public string? Issuer {get; set;}
    public required string Bank {get; set;}
}