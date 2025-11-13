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
        string sql = $"SELECT * FROM {Table()} WHERE license_plate = @plate";
        return _con.QueryFirstOrDefault<VehicleModel>(sql, new { plate = licenseplate });
    }

    public VehicleModel GetVehicleByID(int id)
    {
        string sql = $"SELECT * FROM {Table()} WHERE ID = @ID";
        return _con.QueryFirstOrDefault<VehicleModel>(sql, new { ID = id });
    }

    public void CreateVehicle(RdwAuto data, int user_id)
    {
        string sql = $"INSERT INTO {Table()} (user_id, license_plate, make, model, color, year, created_at) VALUES (@uid, @plate, @make, @model, @color, @year, @created_at)";
        _con.Execute(sql, new { uid = user_id, plate = data.kenteken, make = data.merk, model = data.handelsbenaming, color = data.eerste_kleur, year = Convert.ToInt32(data.datum_eerste_toelating.Substring(0, 4)), created_at = DateTime.Now });
    }

    public string GetLicensePlateByID(int id)
    {
        string sql = $"SELECT license_plate FROM {Table()} WHERE ID = @ID";
        return _con.QueryFirst<string>(sql, new { ID = id });
    }
}