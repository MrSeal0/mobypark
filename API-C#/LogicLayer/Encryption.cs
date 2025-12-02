using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;

public static class Encryption
{
    public static string Hash(string plaintext)
    {
        byte[] inputBytes = Encoding.ASCII.GetBytes(plaintext);
        byte[] hashedBytes;

        using (MD5 md5Hash = MD5.Create())
        {
            hashedBytes = md5Hash.ComputeHash(inputBytes);
        }

        StringBuilder sb = new();
        for (int i = 0; i < hashedBytes.Length; i++)
        {
            sb.Append(hashedBytes[i].ToString("x2"));
        }
        return sb.ToString();
    }

    public static bool Verify(string plaintext, string hash)
    {
        string hashes = Hash(plaintext);

        return hashes == hash;
    }
}