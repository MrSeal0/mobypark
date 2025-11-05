public class ParkingLotsessionslogic
{
    ParkingSessionsAcces _acces = new();
    public bool IsParked(string licenseplate)
    {
        return _acces.IsParked(licenseplate);
    }

    public void StartParkSession(string licenseplate, int uid, int plid)
    {
        _acces.StartParkSession(licenseplate, uid, plid);
    }

    public void StopParkingSession(string licenseplate)
    {
        _acces.StopParkSession(licenseplate);
    }
}