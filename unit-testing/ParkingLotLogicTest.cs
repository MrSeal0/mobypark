using System.Reflection;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.VisualBasic;
using Moq;
using Xunit;
using SQLitePCL;


namespace unit_testing;

public class ParkingLotLogicTest
{
    [Theory]
    [InlineData("LotA", "loc", "Addr", 100, 5, 30,  "0,0")]
    [InlineData("LotB", "locB",  "AddrB", 200, 10, 50, "1,1")]

    public void CreateParkingLotTest(string name, string location, string adress, int capacity, int tarrif, int daytarrif, string coordinates)
    {
        var access = new Mock<IParkingLotsAcces>();
        var logic = new ParkingLotLogic(access.Object);

        var req = new CreateLotRequest
        {
            name = name,
            location = location,
            adress = adress,
            capacity = capacity,
            tarrif = tarrif,
            daytarrif = daytarrif,
            coordinates = coordinates
        };

        logic.CreateParkingLot(req);


        access.Verify(a => a.CreateParkingLot(
            It.Is<CreateLotRequest>(r =>
        r.name == name &&
        r.location == location &&
        r.adress == adress &&
        r.capacity == capacity &&
        r.tarrif == tarrif &&
        r.daytarrif == daytarrif &&
        r.coordinates == coordinates
        )),
        Times.Once);
        
        // CreateLotRequest req = new()
        // {
        //     name = name,
        //     location = location,
        //     adress = adress,
        //     capacity = capacity,
        //     tarrif = tarrif,
        //     daytarrif = daytarrif,
        //     coordinates = coordinates
        // };
        // logic.CreateParkingLot(req);
        // // logic.Setup(p => p.CreateParkingLot(It.IsAny<CreateLotRequest>())).Verifiable();

        // // logic.Setup(p => p.CheckLatestParkingLot()).Returns(new ParkingLotModel());

        // var actual = logic.CheckLatestParkingLot();
        // Assert.NotNull(actual);
        // Assert.Equal(expected.name, actual.name);
        // Assert.Equal(expected.location, actual.location);
        // Assert.Equal(expected.adress, actual.adress);
        // Assert.Equal(expected.capacity, actual.capacity);
        // Assert.Equal(expected.tariff, actual.tariff);
        // Assert.Equal(expected.daytariff, actual.daytariff);
        // Assert.Equal(expected.coordinates, actual.coordinates);
        
    }

    [Theory]
    [InlineData(1, null, -1, "Wijnhaven 34a", 50 )]
    [InlineData(1, "NewAdress", -1, "NewAdress", 50 )]
    [InlineData(1, null, 45, "Wijnhaven 34a", 45 )]
    public void EditParkingLot(int lid, string newAdress, int newDayTariff, string expectedAdress, int expectedDayTariff)
    {
        var access = new Mock<IParkingLotsAcces>();
        var original = new ParkingLotModel
        {
            ID = lid,
            name = "ParkinglotA",
            location = "Wijnhaven",
            adress = "Wijnhaven 34a",
            capacity = 100,
            tariff = 5,
            daytariff = 50,
            coordinates = "1,1"

        };
        access.Setup(a => a.GetLotById(lid)).Returns(original);
        var logic = new ParkingLotLogic(access.Object);

        var editedReq = new EditLotRequest
        {
            name = null,
            location = null,
            adress = newAdress,
            capacity = -1,
            tarrif = -1,
            daytarrif = newDayTariff,
            coordinates = null
        };
        logic.EditParkingLot(lid, editedReq);
        access.Verify(a => a.EditParkingLot(lid, It.Is<EditLotRequest>(r =>
            r.name == original.name &&
            r.location == original.location &&
            r.adress == expectedAdress &&
            r.capacity == original.capacity &&
            r.tarrif == original.tariff &&
            r.daytarrif == expectedDayTariff &&
            r.coordinates == original.coordinates
            )),
            Times.Once);
    }


    [Theory]
    [InlineData(1)]
    [InlineData(2)]

    public void DeleteParkingLotTest(int lid)
    {
        var access = new Mock<IParkingLotsAcces>();
        var logic = new ParkingLotLogic(access.Object);

        logic.DeleteParkingLot(lid);

        access.Verify(a => a.DeleteParkinglot(lid), Times.Once);

    }

    
    [Fact]

    public void GetAllLotsTest()
    {
        var access = new Mock<IParkingLotsAcces>();

        var fakeLots = new List<ParkingLotModel>
        {
            new ParkingLotModel{ID = 1, name = "Parking1", location = "Wijnhaven", adress = "wijnhaven 34a", capacity = 10, tariff = 3, daytariff = 30, coordinates = "1,1"},
            new ParkingLotModel{ID = 2, name = "Parking2", location = "Dordrecht", adress = "Spuiweg 20", capacity = 20, tariff = 4, daytariff = 40, coordinates = "2,2"}
        };

        access.Setup(a => a.GetAllLots()).Returns(fakeLots);

        var logic = new ParkingLotLogic(access.Object);

        var result = logic.GetAllLots();

        Assert.NotNull(result);
        Assert.Equal(fakeLots.Count(), result.Count());
        Assert.Equal(fakeLots[0].name, result[0].name);
        Assert.Equal(fakeLots[1].name, result[1].name);

        access.Verify(a => a.GetAllLots(), Times.Once);


    }


    [Theory]
    [InlineData(1, true)]
    [InlineData(2, false)]

    public void DoesLotExist(int lid, bool exists)
    {
        var access = new Mock<IParkingLotsAcces>();

        access.Setup(a => a.DoesLotExist(lid)).Returns(lid == 1 ? new ParkingLotModel { ID = lid, name = "LotA"} : null);

        var logic = new ParkingLotLogic(access.Object);

        var result = logic.DoesLotExist(lid);

        Assert.Equal(exists, result);
    }

    [Fact]

    public void GetLotByIDTest()
    {
        var access = new Mock<IParkingLotsAcces>();

        var fakelot = new ParkingLotModel
        {
            ID = 1,
            name = "ParkingLotA",
            location = "Rotterdam",
            adress = "Wijnhaven 4",
            capacity = 100,
            tariff = 4,
            daytariff = 50,
            coordinates = "1,1"
        };

        access.Setup(a => a.GetLotById(1)).Returns(fakelot);

        var logic = new ParkingLotLogic(access.Object);

        var result = logic.GetLotByID(1);

        Assert.NotNull(result);
        Assert.Equal(fakelot.ID, result.ID);
        Assert.Equal(fakelot.name, result.name);
        Assert.Equal(fakelot.location, result.location);
        Assert.Equal(fakelot.adress, result.adress);
        Assert.Equal(fakelot.capacity, result.capacity);
        Assert.Equal(fakelot.tariff, result.tariff);
        Assert.Equal(fakelot.daytariff, result.daytariff);
        Assert.Equal(fakelot.coordinates, result.coordinates);
    }


    [Fact]
    public void CheckLatestParkingLotTest()
    {
        var access = new Mock<IParkingLotsAcces>();

        var latestLot = new ParkingLotModel
        {
            ID = 1,
            name = "ParkingLotA",
            location = "Rotterdam",
            adress = "Wijnhaven 5",
            capacity = 100,
            tariff = 4,
            daytariff = 40,
            coordinates = "1,1"
        };

        access.Setup(a => a.GetLatestLot()).Returns(latestLot);

        var logic = new ParkingLotLogic(access.Object);

        var result = logic.CheckLatestParkingLot();

        Assert.NotNull(result);
        Assert.Equal(latestLot.ID, result.ID);
        Assert.Equal(latestLot.name, result.name);
        Assert.Equal(latestLot.location, result.location);
        Assert.Equal(latestLot.adress, result.adress);
        Assert.Equal(latestLot.capacity, result.capacity);
        Assert.Equal(latestLot.tariff, result.tariff);
        Assert.Equal(latestLot.daytariff, result.daytariff);
        Assert.Equal(latestLot.coordinates, result.coordinates);
        
    }
}