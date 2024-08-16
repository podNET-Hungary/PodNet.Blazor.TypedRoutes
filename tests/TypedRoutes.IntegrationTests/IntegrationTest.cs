using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PodNet.Blazor.TypedRoutes.IntegrationTests;

/// <summary>
/// This integration test can be considered a smoke test. The test itself won't even compile if the generator
/// doesn't execute correctly, and the test is only there to test for some common scenarios. All other testing is 
/// done in the *.Tests project.
/// </summary>
[TestClass]
public class IntegrationTest
{
    [TestMethod]
    public void DefaultHappyPathWorks()
    {
        Assert.AreEqual("/singleroute", Cases.SingleRoute.PageUri);
        Assert.AreEqual("/parameterized/2000-01-01/", Cases.ParameterizedRoute.PageUri(new DateTime(2000, 01, 01)));
        var dateWithTime = new DateTime(2000, 01, 01, 12, 15, 00);
        Assert.AreEqual($"/parameterized/2000-01-01/{dateWithTime:s}", Cases.ParameterizedRoute.PageUri(new DateTime(2000, 01, 01), dateWithTime));
    }
}
