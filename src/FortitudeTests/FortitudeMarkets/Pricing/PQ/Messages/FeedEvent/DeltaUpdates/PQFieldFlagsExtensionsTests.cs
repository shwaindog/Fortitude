// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;

[TestClass]
public class PQFieldFlagsExtensionsTests
{
    [TestMethod]
    public void AskFieldUpdate_IsAsk_ReturnsIsAskTrueAndIsBidFalse()
    {
        var fieldUpdate = new PQFieldUpdate(PQQuoteFields.Price, PQDepthKey.AskSide, 0);

        Assert.IsTrue(fieldUpdate.IsAsk());
        Assert.IsFalse(fieldUpdate.IsBid());
    }

    [TestMethod]
    public void BidFieldUpdate_IsBid_ReturnsIsAskFalseAndIsBidTrue()
    {
        var fieldUpdate = new PQFieldUpdate(PQQuoteFields.Price, 0);

        Assert.IsFalse(fieldUpdate.IsAsk());
        Assert.IsTrue(fieldUpdate.IsBid());
    }
}
