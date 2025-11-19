public class PaymentModel
{
    public int ID { get; set; }
    public int t_data_id { get; set; }
    public double Amount { get; set; }
    public DateTime created_at { get; set; }
    public DateTime completed_at { get; set; }
    public string Hash { get; set; }
    public string Initiator { get; set; }
    public string Transaction { get; set; }

}
