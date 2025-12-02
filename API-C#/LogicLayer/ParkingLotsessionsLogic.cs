public class ParkingLotsessionslogic
{
    IParkingSessionAcces _acces;
    IParkingLotsAcces _lotacces;
    IVehicleLogic _vlogic;

    public ParkingLotsessionslogic(IParkingSessionAcces parkingsessionsacces = null, IParkingLotsAcces parkinglotsacces = null, IVehicleLogic vehiclelogic = null)
    {
        _acces = parkingsessionsacces ?? new ParkingSessionsAcces();
        _lotacces = parkinglotsacces ?? new ParkingLotsAcces();
        _vlogic = vehiclelogic ?? new VehicleLogic();
    }
    public bool IsParked(string licenseplate)
    {
        bool isparked = _acces.IsParked(licenseplate);
        return isparked;
    }

    public void StartParkSession(string licenseplate, int uid, int plid)
    {
        _acces.StartParkSession(licenseplate, uid, plid);
    }

    public double calculatefee(int TotalMinutes, double pricePerHour, double pricePerDay)
    {
        double totalHours = TotalMinutes / 60.0;
        int fullDays = (int)(totalHours / 24);

        double remainingHours = totalHours - fullDays * 24;

        double totalCost =  Math.Round(fullDays * pricePerDay + remainingHours * pricePerHour, 2, MidpointRounding.ToEven);
        return totalCost;
    }

    public void StopParkingSession(string licenseplate)
    {
        ParkingSessionModel current = _acces.GetCurrentSession(licenseplate);
        current.end_time = DateTime.Now;
        current.duration_minutes = Convert.ToInt32((current.end_time - current.start_time).TotalMinutes);
        ParkingLotModel lotinfo = _lotacces.GetLotById(current.parking_lot_id);
        current.cost = calculatefee(current.duration_minutes, lotinfo.tariff, lotinfo.daytariff);
        _acces.EndParkingSession(current);
    }

    public void DeleteParkingSession(int sid)
    {
        _acces.DeleteParkingSession(sid);
    }

    public List<ParkingSessionModel> GetUserSessionsForLot(int lot, int uid)
    {
        return _acces.GetAllUsersSessionsFromLot(lot, uid);
    }

    public List<ParkingSessionModel> GetAllSessionsForLot(int lot)
    {
        return _acces.GetAllSessionsFromLot(lot);
    }

    public ParkingSessionModel GetSession(int sid)
    {
        return _acces.GetSessionById(sid);
    }

    public ParkingSessionModel GetSessionFromUser(int sid, int uid)
    {
        return GetSession(sid).user_id == uid ? GetSession(sid) : null;
    }

    public List<ParkingSessionModel> GetVehicleSessionsHistory(int vid)
    {
        return _acces.GetVehicleSessionsHistory(_vlogic.GetVehicleByID(vid).license_plate);
    }
}