using System.Net;
using API_C_.Controllers;

public class LoginLogic
{
    ArgonHasher hasher = new();
    UsersAcces _acces = new();
    SessionLogic _sessionlogic = new();
    UsersAcces _userAcces = new();
    
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
                _userAcces.UpdatePasswordByUsername(userInfo.username, hasher.HashPassword(userInfo.password));
                return _sessionlogic.CreateSession(user.ID);
            }
        }
        return null;
    }
}