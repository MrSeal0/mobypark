using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;
using API_C_.Controllers;
using Dapper;
using Microsoft.AspNetCore.Mvc;

namespace testing;
[TestClass]
public class AccountTest : TestAcces
{
    RegisterLogic _registerLogic = new();
    public override string Table() => "users";

    [TestMethod]
    [DataRow("SambalGek", "Test123!", "Sam", "sambal@gmail.com", "0611254681", 2005)]
    [DataRow("Bobber", "Alice", "Bob", "bobby@gmail.com", "0629291840", 2000)]
    public void CreateAccountTest(string userN, string passW, string name, string email, string phone, int birthyear)
    {
        RegisterRequest RRQ = new()
        {
            username = userN,
            password = passW,
            name = name,
            email = email,
            phone = phone,
            byear = birthyear
        };

        if(IsTaken(userN))
            DeleteTaken(userN);

        Assert.IsFalse(IsTaken(userN));

        _registerLogic.CreateAccount(RRQ);

        Assert.IsTrue(IsTaken(userN));
        
    }

    public void LoginTest()
    {
        
    }

    public void LogoutTest()
    {
        
    }



    public void DeleteTaken(string username)
    {
        string sql = $"DELETE * FROM {Table()} WHERE username = @Username";
        _con.Execute(sql, new { Username = username });
    }

    public bool IsTaken(string username)
    {
        string sql = $"SELECT EXISTS(SELECT 1 FROM {Table()} WHERE username = @Username LIMIT 1)";
        return _con.QueryFirstOrDefault<bool>(sql, new { Username = username });
    }
}