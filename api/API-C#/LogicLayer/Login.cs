public class Login
{
    public bool Success { get; set; }
    public AccountModel? Account { get; set; }
    var hasher = new PasswordHasher();

    public bool ValidateLogin(string? password)
    {
        //get the hashed password via dataAccess from database
        string? hashedpassword = null;

        bool isValid = hasher.VerifyPassword(password, hashedpassword);
    }
}