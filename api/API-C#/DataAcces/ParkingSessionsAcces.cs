using API_C_.Controllers;
using Dapper;

public class ParkingSessionsAcces : AAcces
{
    public override string Table() => "parking_Sessions";

    public bool IsParked(string licenseplate)
    {
        string sql = $"SELECT EXISTS(SELECT 1 FROM {Table()} WHERE license_plate = @Plate AND end_time != NULL LIMIT 1)";
        return _con.QueryFirstOrDefault<bool>(sql, new { Plate = licenseplate });
    }

    public void StartParkSession(string licenseplate, int uid, int plid)
    {
        string sql = $"INSERT INTO {Table()} (parking_lot_id, user_id, license_plate, start_time, payment_status) VALUES (@PLid, @Uid, @LP, @ST, @PS)";
        _con.Execute(sql, new { PLid = plid, Uid = uid, LP = licenseplate, ST = DateTime.Now, PS = "pending" });
    }

    public void StopParkSession(string licenseplate)
    {
        string gettimesql = $"";
        string sql = $"UPDATE {Table()} SET end_time = @ET, duration";
    }
}