using System.Diagnostics;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.VisualBasic;
using Moq;
using Xunit;

namespace unit_testing;

public class LoginTest
{   
    [Theory]
    [InlineData("validusername", "validpassword", true)]
    [InlineData("invalidusername", "invalidpassword", false)]
    [InlineData("validusername", "invalidpassword", false)]
    [InlineData("invalidusername", "validpassword", false)]
    public void TestLogin(string user, string pass, bool result)
    {
        //this mocks the useracces class and returns set values to skip the dataacces layer
        var useraccesMock = new Mock<IUsersAcces>();
        useraccesMock
            .Setup(u => u.GetByUsername("validusername")) // checks for set input
            .Returns(new AccountModel // returns set value
            {
          ID = 1,
          username = "validusername",
          password = "$argon2=bfLtPlK84t0TO5cprSK7hM2hDY0E1NFiU9/hD1uTIgEZrcwKNBC2f5dvaW4wtuMA",
          name = "name",
          email = "email",
          phone = "+31892349",
          role = "user",
          cdate = DateTime.Now,
          byear = 2000,
          active = 1
            });

        useraccesMock
            .Setup(u => u.GetByUsername("invalidusername")) // checks set input
            .Returns((AccountModel?)null); // returns set value
        
        var sessionlogicMock = new Mock<ISessionLogic>(); //mocks the sessionlogic class and returns set data aswell
        sessionlogicMock
            .Setup(s => s.CreateSession(1))
            .Returns("validsessiontoken");


        LoginLogic _logic = new(useraccesMock.Object, sessionlogicMock.Object); // enter the two mocks made to override the normal acces and logic layer refences

        LoginRequest userinfo = new LoginRequest
        {
            username = user,
            password = pass
        };

        string loginresult = _logic.Login(userinfo);

        Debug.WriteLine(Encryption.Hash("validpassword"));

        Assert.Equal(result, loginresult == "validsessiontoken");
    }
}