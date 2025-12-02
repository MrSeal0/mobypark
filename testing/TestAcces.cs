using Microsoft.Data.Sqlite;
public abstract class TestAcces
{
    protected SqliteConnection _con;

    public abstract string Table();

    public TestAcces(string connectionstring = "Data Source=../../../../DataSources/TestDataBase.db")
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