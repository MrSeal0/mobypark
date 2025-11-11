using API_C_.Controllers;
using Dapper;

public class ParkingLotsAcces : AAcces
{
    public override string Table() => "parkinglots";

    public ParkingLotModel GetLotById(int id)
    {
        string sql = $"SELECT * FROM {Table()} WHERE ID = @ID";
        return _con.QueryFirstOrDefault<ParkingLotModel>(sql, new { ID = id });
    }
}