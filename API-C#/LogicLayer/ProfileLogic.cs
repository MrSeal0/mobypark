using System.Security.Cryptography.X509Certificates;

public class ProfileLogic
{
    ArgonHasher argonhasher = new();
    UsersAcces _acces = new();

    public bool VerifyPassword(string password, int uid)
    {
        AccountModel userinfo = _acces.AccountFromUid(uid);

        if (userinfo.password.StartsWith("$argon2=") && argonhasher.VerifyPassword(password, userinfo.password))
        {
            return true;
        }
        else if (Encryption.Hash(password) == userinfo.password)
        {
            return true;
        }
        Console.WriteLine(password);
        Console.WriteLine(argonhasher.HashPassword(password));
        Console.WriteLine(userinfo.password);
        return false;
    }

    public void ChangePassword(int uid, string password)
    {
        string hashedpassword = argonhasher.HashPassword(password);
        _acces.UpdatePasswordByID(uid, hashedpassword);
    }

    public AccountModel GetUserInfo(int uid)
    {
        return _acces.AccountFromUid(uid);
    }
}