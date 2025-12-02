using API_C_.Controllers;
using Dapper;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

public interface IUsersAcces
{
    public bool IsTaken(string username);
    public void InsertAccount(RegisterRequest info);
    public AccountModel AccountFromUid(int uid);
    public AccountModel GetByUsername(string username);
    public void UpdatePasswordByUsername(string username, string password);
    public void UpdatePasswordByID(int uid, string password);
}

public class UsersAcces : AAcces, IUsersAcces
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
        return _con.QueryFirstOrDefault<AccountModel>(sql, new { Uid = uid });
    }

    public AccountModel GetByUsername(string username)
    {
        string sql = $"SELECT * FROM {Table()} WHERE username = @Username";
        return _con.QueryFirstOrDefault<AccountModel>(sql, new { Username = username });
    }

    public void UpdatePasswordByUsername(string username, string password)
    {
        string sql = $"Update {Table()} SET password = @Password WHERE username = @Username";
        _con.Execute(sql, new { Password = password, Username = username });
    }

    public void UpdatePasswordByID(int uid, string password)
    {
        string sql = $"Update {Table()} SET password = @Password WHERE ID = @UID";
        _con.Execute(sql, new { Password = password, UID = uid });
    }
}