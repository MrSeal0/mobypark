using API_C_.Controllers;
using Dapper;

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
}