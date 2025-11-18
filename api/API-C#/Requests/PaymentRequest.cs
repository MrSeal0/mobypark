public class PaymentRequest()
{
    public required string Transaction {get; set;}
    public required double Amount {get; set;}
    public string initiator {get; set;}
    public DateTime created_at {get; set;} = DateTime.Now;
    public bool Completed {get; set;} = false;
    public string Hash {get; set;} = System.Guid.NewGuid().ToString();
}

