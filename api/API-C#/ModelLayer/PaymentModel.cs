public class PaymentModel
{
    public int ID { get; set; }
    public int TData { get; set; }
    public double Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Hash { get; set; }
    public string Initiator { get; set; }
    public string Transaction { get; set; }

}
