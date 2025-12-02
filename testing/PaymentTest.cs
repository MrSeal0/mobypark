using System.Transactions;
using Dapper;
using System.Reflection;
using System;
using System.Collections.Immutable;
using System.ComponentModel.DataAnnotations.Schema;
using API_C_.Controllers;

namespace testing;
[TestClass]
public class PaymentTest : TestAcces
{
    public override string Table() => "Payments";

    [TestMethod]
    [DataRow("JorisTest", 10)]
    [DataRow("OokJoris", 12)]
    public void PaymentGetTest(string user, int am)
    {
        string sql = $"INSERT INTO {Table()} (amount, created_at, hash, initiator, \"transaction\") VALUES (@amount, @created_at, @hash, @initiator, @transaction)";
        _con.Execute(sql, new {amount = am, created_at = DateTime.Now, hash = "idk", initiator = user, transaction = "abc123"});

        string sql2 = $"SELECT amount FROM {Table()} WHERE initiator = @Initiator";
        int newAmount = _con.Execute(sql2, new { Initiator = user });

        Assert.IsTrue(newAmount == am);
    }
}