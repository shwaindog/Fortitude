// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

[TestClass]
public class PQDepthKeyExtensionsTests
{
    [TestMethod]
    public void SingleByteToDepthKeyCorrectlyTranslates()
    {
        byte singleByteAskSide = ((ushort)PQDepthKey.AskSide >> 8) | 1;

        var asDepthKey = singleByteAskSide.ToDepthKey();

        Assert.IsTrue((asDepthKey & PQDepthKey.AskSide) != 0);
        Assert.AreEqual(1, asDepthKey.KeyToDepth());
    }

    [TestMethod]
    public void SingleByteDeterminesIsNotTwoByteDepth()
    {
        byte singleByteAskSide = ((ushort)PQDepthKey.AskSide >> 8) | 1;

        Assert.IsFalse(singleByteAskSide.IsTwoByteDepth());
    }

    [TestMethod]
    public void SingleByteDeterminesIsTwoByteDepth()
    {
        byte singleByteAskSide = ((ushort)(PQDepthKey.AskSide | PQDepthKey.TwoBytes) >> 8) | 1;

        Assert.IsTrue(singleByteAskSide.IsTwoByteDepth());
    }
}
