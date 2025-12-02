using System.Security.Cryptography;

public interface ISessionLogic
{
    public string CreateSession(int uid);
    public void EndSession(string sessionkey);
    public AccountModel GetUserBySession(string session);
}

public class SessionLogic : ISessionLogic
{
    SessionAcces _acces = new();
    UsersAcces _useracces = new();

    string SessionKey()
    {
        var Bytes = new byte[32];

        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(Bytes);
        }

        return BitConverter.ToString(Bytes);
    }

    public string CreateSession(int uid)
    {
        string sessionkey = SessionKey();

        _acces.AddSession(sessionkey, uid);
        return sessionkey;
    }

    public void EndSession(string sessionkey)
    {
        _acces.RemoveSession(sessionkey);
    }

    public AccountModel GetUserBySession(string session)
    {
        int uid = _acces.UidFromSession(session);
        return _useracces.AccountFromUid(uid);
    }
}