public class ParkingLotLogic
{
    ParkingLotsAcces _acces = new();

    public void CreateParkingLot(CreateLotRequest data)
    {
        _acces.CreateParkingLot(data);
    }

    public void EditParkingLot(int lid, EditLotRequest data)
    {
        ParkingLotModel lotinfo = _acces.GetLotById(lid);

        data.name = data.name == null ? lotinfo.name : data.name;
        data.location = data.location == null ? lotinfo.location : data.location;
        data.adress = data.adress == null ? lotinfo.adress : data.adress;
        data.capacity = data.capacity == -1 ? lotinfo.capacity : data.capacity;
        data.tarrif = data.tarrif == -1 ? lotinfo.tariff : data.tarrif;
        data.daytarrif = data.daytarrif == -1 ? lotinfo.daytariff : data.daytarrif;
        data.coordinates = data.coordinates == null ? lotinfo.coordinates : data.coordinates;

        _acces.EditParkingLot(lid, data);
    }

    public void DeleteParkingLot(int lid)
    {
        _acces.DeleteParkinglot(lid);
    }

    public List<ParkingLotModel> GetAllLots()
    {
        return _acces.GetAllLots();
    }

    public bool DoesLotExist(int id)
    {
        ParkingLotModel result = _acces.DoesLotExist(id);
        return result == null ? false : true;
    }

    public ParkingLotModel GetLotByID(int id)
    {
        return _acces.GetLotById(id);
    }
}