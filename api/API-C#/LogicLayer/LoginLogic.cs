using System.Net;
using API_C_.Controllers;

public class LoginLogic
{
    UsersAcces _acces = new();
    SessionLogic _sessionlogic = new();
    
    public string Login(LoginRequest uinfo)
    {
        AccountModel user = _acces.GetByUsername(uinfo.username);
        if (user is not null)
        {
            if (uinfo.password == user.password)
            {
                return  _sessionlogic.CreateSession(user.ID);
            }
        }
        return null;
    }
}