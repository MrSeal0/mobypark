public class ParkingLotsessionslogic
{
    ParkingSessionsAcces _acces = new();
    ParkingLotsAcces _lotacces = new();
    public bool IsParked(string licenseplate)
    {
        bool isparked = _acces.IsParked(licenseplate);
        return isparked;
    }

    public void StartParkSession(string licenseplate, int uid, int plid)
    {
        _acces.StartParkSession(licenseplate, uid, plid);
    }

    public double calculatefee(int TotalMinutes, double price, double dayprice)
    {
        double hours = (double)TotalMinutes / 60;
        int days = Convert.ToInt32(hours / 24);
        hours = hours - (days * 24);
        double totalfee = ((double)days * dayprice) + (hours * price);
        return totalfee;
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
}