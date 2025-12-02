using API_C_.Controllers;
using Dapper;

public interface IRefundAcces
{
    public void CreateNewRefund(RefundRequest refundData);
    public void RefundPayment(int pid, RefundRequest RR);
}

public class RefundAcces : AAcces, IRefundAcces
{
    public override string Table() => "refunds";

    public void CreateNewRefund(RefundRequest refundData)
    {
        string sql = $"INSERT INTO {Table()} (amount, coupled_to, processed_by, hash) VALUES (@amount, @coupled_to, @processed_by, @hash)";
        _con.Execute(sql, new { amount = refundData.Amount,  coupled_to = refundData.Coupled_To, processed_by = refundData.Processed_By, hash = refundData.Hash});
    }

    public void RefundPayment(int pid, RefundRequest RR)
    {
        string sql = $"INSERT INTO {Table()} (paymentID, amount, coupled_to, processed_by, hash) VALUES (@paymentID, @amount, @coupled_to, @processed_by, @hash)";
        _con.Execute(sql, new { paymentID = pid ,amount = RR.Amount,  coupled_to = RR.Coupled_To, processed_by = RR.Processed_By, hash = RR.Hash});
    }
}