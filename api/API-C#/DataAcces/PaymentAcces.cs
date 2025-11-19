using System.Transactions;
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
        string sql = $"INSERT INTO {Table()} (amount, created_at, hash, initiator, \"transaction\") VALUES (@amount, @created_at, @hash, @initiator, @transaction)";
        _con.Execute(sql, new {amount = paymentData.Amount, created_at = paymentData.Created_At, hash = paymentData.Hash, initiator = paymentData.Initiator, transaction = paymentData.Transaction });
    }

    public void CompletePayment(int pid)
    {
        string sql = $"UPDATE {Table()} SET completed_at = @Completed WHERE ID = @id";
        _con.Execute(sql, new { Completed = DateTime.Now, id = pid });
    }

    public PaymentModel GetInfoForTData(int id)
    {
        string sql = $"SELECT * FROM {Table()} WHERE ID = @Id";
        return _con.QueryFirstOrDefault<PaymentModel>(sql, new { Id = id });
    }

    public void CompleteTData(int pid, TDataRequest tdata)
    {
        string sql = $"INSERT INTO t_data (amount, bank, date, issuer, method) VALUES (@amount, @bank, @date, @issuer, @method) RETURNING ID";
        int id = _con.Execute(sql, new {amount = tdata.Amount, bank = tdata.Bank, date = tdata.Date, issuer = tdata.Issuer, method = tdata.Method});
        string sql2 = $"UPDATE {Table()} SET t_data_id = @TID WHERE ID = @id";
        _con.Execute(sql2, new {TID = id, id = pid});
    }

    public PaymentModel GetPaymentByID(int pid)
    {
        string sql = $"SELECT * FROM {Table()} WHERE ID = @id";
        return _con.QueryFirstOrDefault<PaymentModel>(sql, new { id = pid});
    }
}