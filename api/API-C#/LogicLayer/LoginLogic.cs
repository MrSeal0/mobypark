using System.Net;
using API_C_.Controllers;

public class LoginLogic
{
    UsersAcces _acces = new();
    SessionLogic _sessionlogic = new();
    
    public string Login(LoginRequest userInfo)
    {
        AccountModel user = _acces.GetByUsername(userInfo.username);
        if (user is not null)
        {
            if (userInfo.password == user.password)
            {
                return  _sessionlogic.CreateSession(user.ID);
            }
        }
        return null;
    }
}