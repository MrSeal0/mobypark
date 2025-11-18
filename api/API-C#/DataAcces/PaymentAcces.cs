using API_C_.Controllers;
using Dapper;

public class PaymentAcces : AAcces
{
    public override string Table() => "Payments";

    public PaymentModel GetPaymentById(int id)
    {
        string sql = $"SELECT * FROM {Table()} WHERE ID = @ID";
        return _con.QueryFirstOrDefault<PaymentModel>(sql, new { ID = id });
    }

    public List<PaymentModel> GetPaymentsByInitiator(string initiator)
    {
        string sql = $"SELECT * FROM {Table()} WHERE initiator = @Initiator";
        return _con.Query<PaymentModel>(sql, new { Initiator = initiator }).ToList();
    }

    public void CreateNewPayment(PaymentRequest paymentData)
    {
        
    }

    public void CompletePayment(int pid)
    {
        string sql = $"Update {Table()} SET completed_at = @Completed WHERE ID = @id";
        _con.Execute(sql, new { Completed = DateTime.Now, id = pid });
    }
}