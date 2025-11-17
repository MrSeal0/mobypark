using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;
using API_C_.Controllers;
using Dapper;
using SQLitePCL;

public class VehicleAcces : AAcces
{

    public override string Table() => "Vehicles";

    public VehicleModel GetVehicleByLicenseplate(string licenseplate)
    {
        string sql = $"SELECT * FROM {Table()} WHERE license_plate = @plate AND isdeleted = 0";
        return _con.QueryFirstOrDefault<VehicleModel>(sql, new { plate = licenseplate });
    }

    public VehicleModel GetVehicleByID(int id)
    {
        string sql = $"SELECT * FROM {Table()} WHERE ID = @ID AND isdeleted = 0";
        return _con.QueryFirstOrDefault<VehicleModel>(sql, new { ID = id });
    }

    public void CreateVehicle(RdwAuto data, int user_id)
    {
        string sql = $"INSERT INTO {Table()} (user_id, license_plate, make, model, color, year, created_at, isdeleted) VALUES (@uid, @plate, @make, @model, @color, @year, @created_at, @ISDELETED)";
        _con.Execute(sql, new { uid = user_id, plate = data.kenteken, make = data.merk, model = data.handelsbenaming, color = data.eerste_kleur, year = Convert.ToInt32(data.datum_eerste_toelating.Substring(0, 4)), created_at = DateTime.Now, ISDELETED = 0 });
    }

    public string GetLicensePlateByID(int id)
    {
        string sql = $"SELECT license_plate FROM {Table()} WHERE ID = @ID AND isdeleted = 0";
        return _con.QueryFirstOrDefault<string>(sql, new { ID = id });
    }

    public void ChangeCarName(int id, string name)
    {
        string sql = $"UPDATE {Table()} SET name = @NAME WHERE ID = @ID AND isdeleted = 0";
        _con.Execute(sql, new { NAME = name, ID = id});
    }

    public void DeletedCar(int id)
    {
        string sql = $"UPDATE {Table()} SET isdeleted = 1, deleted_at = @DELETEDAT WHERE ID = @ID AND isdeleted = 0";
        _con.Execute(sql, new { DELETEDAT = DateTime.Now, ID = id});
    }
}