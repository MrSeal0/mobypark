using System.Transactions;
using Dapper;
using System.Reflection;


[TestClass]
public class PaymentTest : TestAcces
{
    [TestMethod]
    [DataRow("JorisTest", 10)]
    [DataRow("OokJoris", 12)]
    public PaymentGetTest(string user, int am)
    {
        string sql = $"INSERT INTO {Table()} (amount, created_at, hash, initiator, \"transaction\") VALUES (@amount, @created_at, @hash, @initiator, @transaction)";
        _con.Execute(sql, new {amount = am, created_at = datetime.Now(), hash = "idk", initiator = user, transaction = "abc123"});

        string sql = $"SELECT amount FROM {Table()} WHERE initiator = @Initiator";
        int newAmount = _con.Query<int>(sql, new { Initiator = user }).ToList();

        Assert.IsTrue(newAmount == am);
    }
}