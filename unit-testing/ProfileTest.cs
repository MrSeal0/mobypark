using Microsoft.AspNetCore.Mvc;
using Xunit;
using Moq;

namespace unit_testing;

public class ProfileTest
{
    [Theory]
    [InlineData("validpassword", 1, "newpassword", true)]
    [InlineData("wrongpassword", 1, "newpassword", false)]
    public void ChangePasswordTest(string oldpass, int uid, string newpass, bool result)
    {
        var UsersAccesMock = new Mock<IUsersAcces>();
        UsersAccesMock
            .Setup(u => u.AccountFromUid(1))
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

        UsersAccesMock
            .Setup(u => u.UpdatePasswordByID(1, It.IsAny<string>()));

        ProfileLogic _Logic = new(UsersAccesMock.Object);

        Assert.Equal(result, _Logic.ChangePassword(oldpass, uid, newpass));
    }
}
