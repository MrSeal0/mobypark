public class AccountModel
{
    public required string Username { get; set; }
    public required string HashedPassword { get; set; }
    public required string Name { get; set; }
    public required string SessionToken { get; set; }

    public AccountModel(string username, string hashedPassword, string name)
    {
        Username = username;
        HashedPassword = hashedPassword;
        Name = name;
        SessionToken = // add random generator
    }
}