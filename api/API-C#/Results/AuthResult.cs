public class AuthResult
{
    public bool Success { get; set; }
    public string? SessionToken { get; set; }

    public AuthResult(bool success, string? sessionToken = null)
    {
        Success = success;
        SessionToken = sessionToken;
    }
}
