using Moq;
using Xunit;

public class VehicleLogicTests
{
    private readonly Mock<IVehicleAcces> _vehicleAccesMock;
    private readonly Mock<ISessionAcces> _sessionAccesMock;
    private readonly VehicleLogic _logic;

    public VehicleLogicTests()
    {
        _vehicleAccesMock = new Mock<IVehicleAcces>();
        _sessionAccesMock = new Mock<ISessionAcces>();

        _logic = new VehicleLogic(_vehicleAccesMock.Object, _sessionAccesMock.Object);
    }

    // Test for when vehicle exissts and should return true
    [Fact]
    public void IsVehicleRegisterdTestTrue()
    {
        var vehicle = new VehicleModel {ID = 1, license_plate = "R323PG"};
        _vehicleAccesMock.Setup(a => a.GetVehicleByLicenseplate("R323PG")).Returns(vehicle);

        var result = _logic.IsVehicleRegisterd("R323PG");

        Assert.True(result);

    }


    // Test for when vehicle doesnt exists
    [Fact]

    public void IsVehicleRegisterdNonExistend()
    {
        _vehicleAccesMock.Setup(a => a.GetVehicleByLicenseplate("R323PG")).Returns((VehicleModel)null);

        var result = _logic.IsVehicleRegisterd("R323PG");
        Assert.False(result);
    }



    // Test for when user owns car

    [Fact]

    public void DoesUserOwnCarTestTrue()
    {
        string token = "token";
        string plate = "R323PG";

        var vehicle = new VehicleModel
        {
            user_id = 1,
            license_plate = plate
        };

        _vehicleAccesMock.Setup(a => a.GetVehicleByLicenseplate(plate)).Returns(vehicle);
        _sessionAccesMock.Setup(a => a.UidFromSession(token)).Returns(1);

        var result = _logic.DoesUserOwnCar(token, plate);

        Assert.True(result);
    }

    
    


}