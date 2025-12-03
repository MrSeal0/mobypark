using System.Diagnostics;
using Microsoft.Data.Sqlite;

public abstract class AAcces
{
    protected SqliteConnection _con;

    public abstract string Table();

    public AAcces()
    {
        _con = new SqliteConnection("Data Source=DataSources/DataBase.db");
        _con.Open();

        using (var command = _con.CreateCommand())
        {
            command.CommandText = "PRAGMA foreign_keys = ON;";
            command.ExecuteNonQuery();
        }
    }
}