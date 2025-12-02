using Moq;
using Xunit;

namespace unit_testing;

public class ParkingLotsessionsTest
{
    [Theory]
    [InlineData("1", false)]
    [InlineData("2", true)]
    public void TestIsParked(string plate, bool result)
    {
        var parkingsessionaccesMock = new Mock<IParkingSessionAcces>();
        var parkinglotaccesMock = new Mock<IParkingLotsAcces>();
        var vehiclelogicMock = new Mock<IVehicleLogic>();
        parkingsessionaccesMock
            .Setup(ps => ps.IsParked("1"))
            .Returns(false);

        parkingsessionaccesMock
            .Setup(ps => ps.IsParked("2"))
            .Returns(true);

        ParkingLotsessionslogic _logic = new(parkingsessionaccesMock.Object, parkinglotaccesMock.Object, vehiclelogicMock.Object);

        Assert.Equal(result, _logic.IsParked(plate));
    }

    [Theory]
    [InlineData(37, 2, 16, 1.23)]
    [InlineData(3473, 3, 16, 61.65)]
    [InlineData(237, 7, 16, 27.65)]

    public void TestCalculateFee(int totalmins, double price, double dayprice, double ans)
    {
        var parkingsessionaccesMock = new Mock<IParkingSessionAcces>();
        var parkinglotaccesMock = new Mock<IParkingLotsAcces>();
        var vehiclelogicMock = new Mock<IVehicleLogic>();
        ParkingLotsessionslogic _logic = new(parkingsessionaccesMock.Object, parkinglotaccesMock.Object, vehiclelogicMock.Object);

        double calcresult = _logic.calculatefee(totalmins, price, dayprice);

        Assert.Equal(ans, calcresult);
    }
}