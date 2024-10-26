#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

[TestClass]
public class PQScalingTests
{
    [TestMethod]
    public void RangeOfValues_AutoScaling_GivesExpectedValues()
    {
        var originalValue = 0.00002345m;
        byte calcFlag;
        var convertedValue = PQScaling.AutoScale(originalValue, 10, out calcFlag);
        Assert.AreEqual(234u, convertedValue);
        Assert.AreEqual(1, calcFlag);

        originalValue = -0.00002345m;
        convertedValue = PQScaling.AutoScale(originalValue, 10, out calcFlag);
        Assert.AreEqual(234u, convertedValue);
        Assert.AreEqual(17, calcFlag);

        originalValue = 2345000000m;
        convertedValue = PQScaling.AutoScale(originalValue, 4, out calcFlag);
        Assert.AreEqual(2345u, convertedValue);
        Assert.AreEqual(14, calcFlag);

        originalValue = -2345000000m;
        convertedValue = PQScaling.AutoScale(originalValue, 4, out calcFlag);
        Assert.AreEqual(2345u, convertedValue);
        Assert.AreEqual(30, calcFlag);

        originalValue = 234500000000m;
        convertedValue = PQScaling.AutoScale(originalValue, 4, out calcFlag);
        Assert.AreEqual(23450u, convertedValue);
        Assert.AreEqual(15, calcFlag);

        originalValue = -234500000000m;
        convertedValue = PQScaling.AutoScale(originalValue, 4, out calcFlag);
        Assert.AreEqual(23450u, convertedValue);
        Assert.AreEqual(31, calcFlag);

        originalValue = 123456.7890112m;
        convertedValue = PQScaling.AutoScale(originalValue, 9, out calcFlag);
        Assert.AreEqual(123456789u, convertedValue);
        Assert.AreEqual(5, calcFlag);

        originalValue = -123456.7890112m;
        convertedValue = PQScaling.AutoScale(originalValue, 9, out calcFlag);
        Assert.AreEqual(123456789u, convertedValue);
        Assert.AreEqual(21, calcFlag);
    }
}
