// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

[TestClass]
public class PQScalingTests
{
    [TestMethod]
    public void RangeOfValues_AutoScaling_GivesExpectedValues()
    {
        var          originalValue = 0.00002345m;
        PQFieldFlags calcFlag;
        var          convertedValue = PQScaling.AutoScale(originalValue, 10, out calcFlag);
        Assert.AreEqual(234u, convertedValue);
        Assert.AreEqual((PQFieldFlags)1, calcFlag);

        originalValue  = -0.00002345m;
        convertedValue = PQScaling.AutoScale(originalValue, 10, out calcFlag);
        Assert.AreEqual(234u, convertedValue);
        Assert.AreEqual((PQFieldFlags)17, calcFlag);

        originalValue  = 2345000000m;
        convertedValue = PQScaling.AutoScale(originalValue, 4, out calcFlag);
        Assert.AreEqual(2345u, convertedValue);
        Assert.AreEqual((PQFieldFlags)14, calcFlag);

        originalValue  = -2345000000m;
        convertedValue = PQScaling.AutoScale(originalValue, 4, out calcFlag);
        Assert.AreEqual(2345u, convertedValue);
        Assert.AreEqual((PQFieldFlags)30, calcFlag);

        originalValue  = 234500000000m;
        convertedValue = PQScaling.AutoScale(originalValue, 4, out calcFlag);
        Assert.AreEqual(23450u, convertedValue);
        Assert.AreEqual((PQFieldFlags)15, calcFlag);

        originalValue  = -234500000000m;
        convertedValue = PQScaling.AutoScale(originalValue, 4, out calcFlag);
        Assert.AreEqual(23450u, convertedValue);
        Assert.AreEqual((PQFieldFlags)31, calcFlag);

        originalValue  = 123456.7890112m;
        convertedValue = PQScaling.AutoScale(originalValue, 9, out calcFlag);
        Assert.AreEqual(123456789u, convertedValue);
        Assert.AreEqual((PQFieldFlags)5, calcFlag);

        originalValue  = -123456.7890112m;
        convertedValue = PQScaling.AutoScale(originalValue, 9, out calcFlag);
        Assert.AreEqual(123456789u, convertedValue);
        Assert.AreEqual((PQFieldFlags)21, calcFlag);
    }
}
