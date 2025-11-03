using System.Security.Principal;

public static class SessionManager
{
    public static Dictionary<string, Dictionary<string, string>> Sessions = new();

    public static void add_session(string token, Dictionary<string, string> user)
    {
        Sessions[token] = user;
    }

    public static void remove_session(string token)
    {
        Sessions.Remove(token);
    }

    public static bool get_session(string token)
    {
        return Sessions.ContainsKey(token);
    }
}