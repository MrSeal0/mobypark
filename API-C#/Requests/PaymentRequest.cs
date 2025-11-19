public class PaymentRequest()
{
    public required string Transaction {get; set;}
    public required double Amount {get; set;}
    public string? Initiator {get; set;}
    public DateTime Created_At {get; set;} = DateTime.Now;
    public bool Completed {get; set;} = false;
    public string Hash {get; set;} = System.Guid.NewGuid().ToString();
}

