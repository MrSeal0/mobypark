using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Mvc;

public class ProfileLogic
{
    ArgonHasher argonhasher = new();
    IUsersAcces _acces;

    public ProfileLogic(IUsersAcces acces = null)
    {
        _acces = acces ?? new UsersAcces();
    }

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

    public bool ChangePassword(string oldpassword, int uid, string new_password)
    {
        if (VerifyPassword(oldpassword, uid))
        {
            string hashedpassword = argonhasher.HashPassword(new_password);
            _acces.UpdatePasswordByID(uid, hashedpassword);
            return true;
        }
        else
        {
            return false;
        }
    }

    public AccountModel GetUserInfo(int uid)
    {
        return _acces.AccountFromUid(uid);
    }
}