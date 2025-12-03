public class ReservationsLogic
{
    IReservationsAcces _acces;
    IParkingLotsessionslogic _psessionlogic;
    IParkingLotLogic _plotlogic;

    public ReservationsLogic(IReservationsAcces reservationacces = null, IParkingLotsessionslogic parkinglotssessionlogic = null, IParkingLotLogic parkinglotlogic = null)
    {
        _acces = reservationacces ?? new ReservationsAcces();
        _psessionlogic = parkinglotssessionlogic ?? new ParkingLotsessionslogic();
        _plotlogic = parkinglotlogic ?? new ParkingLotLogic();
    }
    public void CreateReservation(ReservationRequest data, int uid)
    {
        ParkingLotModel targetlot = _plotlogic.GetLotByID(data.parkinglot);
        double cost = _psessionlogic.calculatefee(Convert.ToInt32((data.enddate - data.startdate).TotalMinutes), targetlot.tariff, targetlot.daytariff);
        _acces.CreateReservation(data, uid, cost);
    }

    public void EditReservation(EditReservationRequest data, int rid)
    {
        ReservationModel old = GetReservationById(rid);

        data.vehicle_id = data.vehicle_id == default ? old.vehicle_id : data.vehicle_id;
        data.startdate = data.startdate == default ? old.start_time : data.startdate;
        data.enddate = data.enddate == default ? old.end_time : data.enddate;
        data.parkinglot = data.parkinglot == default ? old.parking_lot_id : data.parkinglot;

        _acces.EditReservation(data, rid);
    }

    public ReservationModel GetReservationById(int id)
    {
        return _acces.GetReservationByID(id);
    }

    public bool DoesReservationExist(int id)
    {
        return GetReservationById(id) == null ? false : true;
    }

    public bool IsMyResrvation(int rid, int uid)
    {
        ReservationModel result = GetReservationById(rid);
        if(result == null)
        {
            return false;
        }
        else
        {
           return result.user_id == uid ? true : false;
        }
    }

    public void DeleteReservation(int rid)
    {
        _acces.DeleteReservation(rid);
    }

    public List<ReservationModel> GetVehicleReservations(int vid)
    {
        return _acces.GetVehicleReservations(vid);
    }
}