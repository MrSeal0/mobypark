using System.Net;
using API_C_.Controllers;

public class RegisterLogic
{
    public HttpStatusCode CreateAccount(RegisterRequest userinfo)
    {
        UsersAcces acces = new();
        if (acces.IsTaken(userinfo.username))
        {
            return HttpStatusCode.Conflict;
        }
        acces.InsertAccount(userinfo);
        return HttpStatusCode.Created;
    }
}