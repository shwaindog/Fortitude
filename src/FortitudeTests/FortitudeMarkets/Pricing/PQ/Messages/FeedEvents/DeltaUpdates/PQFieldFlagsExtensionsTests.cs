// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;

[TestClass]
public class PQFieldFlagsExtensionsTests
{
    [TestMethod]
    public void AskFieldUpdate_IsAsk_ReturnsIsAskTrueAndIsBidFalse()
    {
        var fieldUpdate = new PQFieldUpdate(PQFeedFields.QuoteLayerPrice, PQDepthKey.AskSide, 0);

        Assert.IsTrue(fieldUpdate.IsAsk());
        Assert.IsFalse(fieldUpdate.IsBid());
    }

    [TestMethod]
    public void BidFieldUpdate_IsBid_ReturnsIsAskFalseAndIsBidTrue()
    {
        var fieldUpdate = new PQFieldUpdate(PQFeedFields.QuoteLayerPrice, 0);

        Assert.IsFalse(fieldUpdate.IsAsk());
        Assert.IsTrue(fieldUpdate.IsBid());
    }
}
