using API_C_.Controllers;

public class PaymentAcces : AAcces
{
    public override string Table() => "payments";

    public PaymentModel GetPaymentById(int id)
    {
        string sql = $"SELECT * FROM {Table()} WHERE ID = @ID";
        return _con.QueryFirstOrDefault<PaymentModel>(sql, new { ID = id });
    }

    public PaymentModel GetPaymentByInitiator(string initiator)
    {
        string sql = $"SELECT * FROM {Table()} WHERE initiator = @Initiator";
        return _con.QueryFirstOrDefault<PaymentModel>(sql, new { Initiator = initiator });
    }

    public void CompletePayment(string username)
    {
        string sql = $"Update {Table()} SET completed_at = @Completed WHERE username = @Username";
        _con.Execute(sql, new { Completed = DateTime.Now, Username = username });
    }
}