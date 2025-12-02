using Moq;
using Xunit;

namespace unit_testing;
public class HashingTest
{
    ArgonHasher Ahasher = new();

    [Theory]
    [InlineData("PlasticSoep123")]
    [InlineData("PlasticSoep123!")]
    public void TestVerifyPasswordArgon(string password)
    {
        string hashedPassword = Ahasher.HashPassword(password);
        Assert.True(Ahasher.VerifyPassword(password, hashedPassword));
    }

    [Theory]
    [InlineData("PlasticSoep123")]
    [InlineData("PlasticSoep123!")]
    public void TestVerifyPasswordMD5(string password)
    {
        string hashed = Encryption.Hash(password);
        Assert.True(Encryption.Verify(password, hashed));
    }
}