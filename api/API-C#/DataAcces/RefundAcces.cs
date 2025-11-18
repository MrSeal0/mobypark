using API_C_.Controllers;
using Dapper;

public class RefundAcces : AAcces
{
    public override string Table() => "Refunds";

    public void CreateNewRefund(RefundRequest refundData)
    {
        string sql = $"INSERT INTO {Table()} VALUES Amount = @Amount, Coupled_To = @Coupled, Processed_By = @Processor, Hash = @hash";
        _con.Execute(sql, new { Amount = refundData.Amount,  Coupled = refundData.Coupled_To, Processor = refundData.Processed_By, Hash = refundData.Hash});
    }
}