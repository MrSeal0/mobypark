public class RegisterRequest
{
    public required string username { get;  set; }
    public required string password { get;  set; }
    public required string name { get;  set; }
    public required string email { get;  set; }
    public required string phone { get;  set; }
    public string role { get;  set; } = "USER";
    public DateTime cdate { get;  set; } = DateTime.Now;
    public required int byear { get;  set; }
    public int active { get;  set; } = 1;
}