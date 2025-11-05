using API_C_.Controllers;
using Dapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public class UsersAcces : AAcces
{
    public override string Table() => "users";

    public bool IsTaken(string username)
    {
        string sql = $"SELECT EXISTS(SELECT 1 FROM {Table()} WHERE username = @Username LIMIT 1)";
        return _con.QueryFirstOrDefault<bool>(sql, new { Username = username });
    }

    public void InsertAccount(RegisterRequest info)
    {
        string sql = $"INSERT INTO {Table()} (username, password, full_name, email, phone, role, creation_date, birth_year, active) VALUES (@username, @password, @name, @email, @phone, @role, @cdate, @byear, @active)";
        _con.Execute(sql, info);
    }

    public AccountModel AccountFromUid(int uid)
    {
        string sql = $"SELECT * FROM {Table()} WHERE ID = @Uid ";
        return _con.QueryFirstOrDefault<AccountModel>(sql, new { Uid = uid});
    }

    public AccountModel GetByUsername(string username)
    {
        string sql = $"SELECT * FROM {Table()} WHERE username = @Username";
        return _con.QueryFirstOrDefault<AccountModel>(sql, new { Username = username });
    }
}