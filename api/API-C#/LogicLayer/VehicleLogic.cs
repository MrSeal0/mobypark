using API_C_.Controllers;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public class RDWRequest()
{
    public string license_plate { get; set; }
    public string make { get; set; }

    public string model { get; set; }

    public string color { get; set; }
    public string year { get; set; }
}

public class VehicleLogic
{
    VehicleAcces _acces = new();
    SessionAcces _sessionacces = new();
    public bool IsVehicleRegisterd(string plate)
    {
        return GetVehicleByPlate(plate) != null ? true : false;
    }

    public bool DoesUserOwnCarByPlate(string token, string plate)
    {
        VehicleModel vehicle = GetVehicleByPlate(plate);
        return _sessionacces.UidFromSession(token) == vehicle.user_id ? true : false;
    }

    public bool DoesUserOwnCarByID(string token, int id)
    {
        return DoesUserOwnCarByPlate(token, _acces.GetLicensePlateByID(id));
    }

    public async Task<bool> CreateVehicle(string plate, string token)
    {
        int uid = _sessionacces.UidFromSession(token);
        var data = await RdwService.GetAutoByKentekenAsync(plate);
        if (data is RdwAuto)
        {
            _acces.CreateVehicle(data, uid);
            return true;
        }
        else
        {
            return false;
        }
    }

    public VehicleModel GetVehicleByPlate(string plate)
    {
        return _acces.GetVehicleByLicenseplate(plate);
    }

    public VehicleModel GetVehicleByID(int id)
    {
        return _acces.GetVehicleByID(id);
    }
}