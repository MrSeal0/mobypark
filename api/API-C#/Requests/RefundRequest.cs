

public class RefundRequest()
{
    public required int PaymentID {get; set;}
    public required string Transaction {get; set;}
    public required double Amount {get; set;}
    public required string Coupled_To {get; set;}
    public string Processed_By {get; set;}
    public DateTime Created_At {get; set;} = DateTime.Now;
    public bool Completed {get; set;} = false;
    public string Hash {get; set;} = System.Guid.NewGuid().ToString();
}

