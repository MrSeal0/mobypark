public class Register
{
    public const int MinPasswordLength = 8;
    var hasher = new PasswordHasher();
    public bool validValidatePassword(string? password, string? confirmedPassword)
    {
        if (password != confirmedPassword)
            return false;

        if (string.IsNullOrWhiteSpace(password) || password.Length < MinPasswordLength)
            return false;

        bool hasLetter = false;
        bool hasDigit = false;
        foreach (char c in password)
        {
            if (char.IsLetter(c)) hasLetter = true;
            if (char.IsDigit(c)) hasDigit = true;
            if (hasLetter && hasDigit) break;
        }
        if (!hasLetter || !hasDigit)
            return false;
        else
            return true;
    }

    public void HashPassword(string password)
    {
        string hashedPassword = hasher.HashPassword(password);

        //save hashed password from dataAccess to database
    }

    public (AccountModel?, string? errormessage) CreateAccount(string username, string? password, string? confirmedPassword, string name)
    {
        if (!ValidatePassword(password, confirmedPassword)[0])
            return (null, "password invalid");

        if (username == null || username == "" || name == null || name == "")
            return (null, "name and username are required");
            
        new AccountModel(username, HashPassword(password), name);
    }
}