public class PaymentModel
{
    public int ID { get; set; }
    public string TData { get; set; } = string.Empty;
    public double Amount { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
    public string Hash { get; set; } = string.Empty;
    public string Initiator { get; set; } = string.Empty;
    public string Transaction { get; set; } = string.Empty;

    public override string ToString()
    {
        return "";
    }

}