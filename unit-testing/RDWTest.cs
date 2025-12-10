using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace unit_testing;

public class RDWTest
{
    [Fact]
    public async Task TestRDWApi()
    {
        var res = await RdwService.GetAutoByKentekenAsync("XRTT13");

        Assert.IsType(typeof(RdwAuto), res);
    }
}