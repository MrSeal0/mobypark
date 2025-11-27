using Microsoft.Data.Sqlite;

public abstract class AAcces
{
    protected SqliteConnection _con;

    public abstract string Table();

    public AAcces(string connectionstring = "Data Source=DataSources/DataBase.db")
    {
        _con = new SqliteConnection(connectionstring);
        _con.Open();

        using (var command = _con.CreateCommand())
        {
            command.CommandText = "PRAGMA foreign_keys = ON;";
            command.ExecuteNonQuery();
        }
    }
}