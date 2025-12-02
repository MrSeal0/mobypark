using System.Net;
using API_C_.Controllers;

public class LoginLogic
{
    ArgonHasher hasher = new();
    IUsersAcces _acces;
    ISessionLogic _sessionlogic;

    public LoginLogic(IUsersAcces useracces = null, ISessionLogic sessionlogic = null)
    {
        _acces = useracces ?? new UsersAcces();
        _sessionlogic = sessionlogic ?? new SessionLogic();
    }

    
    public string Login(LoginRequest userInfo)
    {
        AccountModel? user = _acces.GetByUsername(userInfo.username);

        if (user is not null)
        {
            if (user.password.StartsWith("$argon2=") && hasher.VerifyPassword(userInfo.password, user.password))
            {
                return  _sessionlogic.CreateSession(user.ID);
            }
            else if (Encryption.Hash(userInfo.password) == user.password)
            {
                _acces.UpdatePasswordByUsername(userInfo.username, hasher.HashPassword(userInfo.password));
                return _sessionlogic.CreateSession(user.ID);
            }
        }
        return null;
    }
}