using System.ComponentModel.DataAnnotations.Schema;
using Dapper;
using SQLitePCL;

public class ReservationsAcces : AAcces
{
    public override string Table() => "reservations";

    public void CreateReservation(ReservationRequest data, int uid, double cost)
    {
        string sql = $"INSERT INTO {Table()} (user_id, parking_lot_id, vehicle_id, start_time, end_time, status, creation_date, cost) VALUES (@UID, @PID, @VID, @ST, @ET, 'pending', @CDATE, @COST)";
        _con.Execute(sql, new { UID = uid, @PID = data.parkinglot, @VID = data.vehicle_id, @ST = data.startdate, ET = data.enddate, CDATE = DateTime.Now, @COST = cost });
    }

    public ReservationModel GetReservationByID(int id)
    {
        string sql = $"SELECT * FROM {Table()} WHERE ID = @ID AND isdeleted = 0";
        return _con.QueryFirstOrDefault<ReservationModel>(sql, new { ID = id});
    }

    public void EditReservation(EditReservationRequest data, int rid)
    {
        string sql = $"UPDATE {Table()} SET parking_lot_id = @PID, vehicle_id = @VID, start_time = @ST, end_time = @ET WHERE ID = @RID AND isdeleted = 0";
        _con.Execute(sql, new { PID = data.parkinglot, VID = data.vehicle_id, ST = data.startdate, ET = data.enddate, RID = rid });
    }

    public void DeleteReservation(int rid)
    {
        string sql = $"UPDATE {Table()} SET deleted_at = @DA, isdeleted = 1 WHERE ID = @RID";
        _con.Execute(sql, new { DA = DateTime.Now, RID = rid });
    }

    public List<ReservationModel> GetVehicleReservations(int vid)
    {
        string sql = $"SELECT * FROM {Table()} WHERE vehicle_id = @VID and isdeleted = 0";
        return _con.Query<ReservationModel>(sql, new { VID = vid }).ToList();
    }
}