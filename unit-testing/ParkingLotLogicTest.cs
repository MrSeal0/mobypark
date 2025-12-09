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


    // [Theory]
    // [InlineData(1)]
    // [InlineData(2)]

    // public void DeleteParkingLotTest(int lid)
    // {
    //     var access = new Mock<IParkingLotsAcces>();
    //     var logic = new ParkingLotLogic(access.Object);

    // }

}