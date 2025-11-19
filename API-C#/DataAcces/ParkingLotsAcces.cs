using API_C_.Controllers;
using Dapper;
using SQLitePCL;

public class ParkingLotsAcces : AAcces
{
    public override string Table() => "parkinglots";

    public ParkingLotModel GetLotById(int id)
    {
        string sql = $"SELECT * FROM {Table()} WHERE ID = @ID";
        return _con.QueryFirstOrDefault<ParkingLotModel>(sql, new { ID = id });
    }

    public void CreateParkingLot(CreateLotRequest data)
    {
        string sql = $"INSERT INTO {Table()} (name, location, address, capacity, reserved, tariff, daytariff, created_at, coordinates) VALUES (@NAME, @LOCATION, @ADRESS, @CAPACITY, @RESERVED, @TARRIF, @DAYTARRIF, @CREATED_AT, @COORDINATES)";
        _con.Execute(sql, new { NAME = data.name, LOCATION = data.location, ADRESS = data.adress, CAPACITY = data.capacity, RESERVED = 0, TARRIF = data.tarrif, DAYTARRIF = data.daytarrif, CREATED_AT = DateTime.Now, COORDINATES = data.coordinates });
    }

    public void EditParkingLot(int lid, EditLotRequest data)
    {
        string sql = $"UPDATE {Table()} SET name = @NAME, location = @LOCATION, address = @ADRESS, capacity = @CAPACITY, tariff = @TARIFF, daytariff = @DAYTARIFF, coordinates = @COORDINATES WHERE ID = @LID";
        _con.Execute(sql, new { LID = lid, NAME = data.name, LOCATION = data.location, ADRESS = data.adress, CAPACITY = data.capacity, TARIFF = data.tarrif, DAYTARIFF = data.daytarrif, COORDINATES = data.coordinates });
    }

    public void DeleteParkinglot(int pid)
    {
        string sql = $"UPDATE {Table()} SET deleted_at = @DELETEDAT, isdeleted = 1 WHERE ID = @PID";
        _con.Execute(sql, new { DELETEDAT = DateTime.Now, PID = pid });
    }

    public List<ParkingLotModel> GetAllLots()
    {
        string sql = $"SELECT * FROM {Table()} WHERE isdeleted = 0";
        return _con.Query<ParkingLotModel>(sql).ToList();
    }

    public ParkingLotModel DoesLotExist(int pid)
    {
        string sql = $"SELECT * FROM {Table()} WHERE ID = @PID AND isdeleted = 0";
        return _con.QueryFirstOrDefault<ParkingLotModel>(sql, new { PID = pid });
    }
}