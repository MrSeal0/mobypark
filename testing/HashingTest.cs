using System.Reflection;

namespace testing;
[TestClass]
public class HashingTest
{
    ArgonHasher hasher = new();

    [TestMethod]
    [DataRow("PlasticFles123")]
    [DataRow("PlasticFles123!")]
    public void TestHashPassword(string password)
    {
        Assert.IsFalse(hasher.HashPassword(password) == password);
    }


    [TestMethod]
    [DataRow("PlasticSoep123")]
    [DataRow("PlasticSoep123!")]
    public void TestVerifyPassword(string password)
    {
        string hashedPassword = hasher.HashPassword(password);
        Assert.IsTrue(hasher.VerifyPassword(password, hashedPassword));
    }

    
}