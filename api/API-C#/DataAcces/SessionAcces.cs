using API_C_.Controllers;
using Dapper;

public class SessionAcces : AAcces
{
    public override string Table() => "sessions";

    public void AddSession(string session, int uid)
    {
        string sql = $"INSERT INTO {Table()} (SessionID, UserID, CreatedAt) VALUES (@Session, @Uid, @createdAt)";
        _con.Execute(sql, new { Session = session, Uid = uid, createdAt = DateTime.Now });
    }

    public void RemoveSession(string session)
    {
        string sql = $"DELETE FROM {Table()} WHERE SessionID = @Session";
        _con.Execute(sql, new { Session = session });
    }
    public int UidFromSession(string session)
    {
        string sql = $"SELECT UserID FROM {Table()} WHERE SessionID = @Session";
        return _con.QueryFirstOrDefault<int>(sql, new { Session = session });
    }
}