using Moq;
using Xunit;

namespace unit_testing;

public class ReservationTest
{
    [Theory]
    [InlineData(1, true)]
    [InlineData(2, true)]
    [InlineData(3, true)]
    [InlineData(4, true)]
    [InlineData(5, true)]
    [InlineData(6, true)]
    [InlineData(7, false)]
    public void TestGetReservationById(int id, bool expectation)
    {
        var reservationaccesMock = new Mock<IReservationsAcces>();
        var parkinglotsessionlogicMock = new Mock<IParkingLotsessionslogic>();
        var parkinglotlogicMock = new Mock<IParkingLotLogic>();

        reservationaccesMock
            .Setup(r => r.GetReservationByID(It.Is<int>(n => n < 7)))
            .Returns((int id) =>
                id < 7 ? new ReservationModel{ ID = id } : null
            );
        
        ReservationsLogic _logic = new(reservationaccesMock.Object, parkinglotsessionlogicMock.Object, parkinglotlogicMock.Object);


        var reservation = _logic.GetReservationById(id);
        if(expectation)
        {
            Assert.NotNull(reservation);
            Assert.Equal(id, reservation.ID);
        }
        else
        {
            Assert.Null(reservation);
        }
    }
}