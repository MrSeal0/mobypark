using System.Diagnostics;
using Microsoft.Data.Sqlite;

public abstract class AAcces
{
    protected SqliteConnection _con;

    public abstract string Table();

    public AAcces()
    {
        Debug.WriteLine(System.Environment.GetEnvironmentVariable("TEST_DB"));
        _con = new SqliteConnection(System.Environment.GetEnvironmentVariable("TEST_DB") ?? "Data Source=../../../../DataSources/DataBase.db");
        _con.Open();

        using (var command = _con.CreateCommand())
        {
            command.CommandText = "PRAGMA foreign_keys = ON;";
            command.ExecuteNonQuery();
        }
    }
}