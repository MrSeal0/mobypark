using API_C_.Controllers;
using Microsoft.Extensions.Primitives;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

public interface IVehicleLogic
{
    
    public bool IsVehicleRegisterd(string plate);
    public bool DoesUserOwnCar(string token, string plate);
    public bool DoesUserOwnCar(string token, int id);
    public Task<bool> CreateVehicle(string plate, string token);
    public VehicleModel GetVehicleByPlate(string plate);
    public VehicleModel GetVehicleByID(int id);
    public void ChangeCarNickname(int id, string name);
    public void DeleteCar(int id);
}

public class VehicleLogic : IVehicleLogic
{
    VehicleAcces _acces = new();
    SessionAcces _sessionacces = new();
    public bool IsVehicleRegisterd(string plate)
    {
        return GetVehicleByPlate(plate) != null ? true : false;
    }

    public bool DoesUserOwnCar(string token, string plate)
    {
        VehicleModel result = GetVehicleByPlate(plate);
        if (result == null) return false;

        return _sessionacces.UidFromSession(token) == result.user_id;
    }

    public bool DoesUserOwnCar(string token, int id)
    {
        VehicleModel result = GetVehicleByID(id);
        if (result == null) return false;

        return _sessionacces.UidFromSession(token) == result.user_id;
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

    public void ChangeCarNickname(int id, string name)
    {
        _acces.ChangeCarName(id, name);
    }

    public void DeleteCar(int id)
    {
        _acces.DeletedCar(id);
    }
}