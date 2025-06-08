// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using static FortitudeMarkets.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarkets.Pricing.FeedEvents.TickerInfo.TickerQuoteDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

[TestClass]
public class PQImplementationFactoryTests
{
    [TestMethod]
    public void NewPQImplementationFactory_GetConcreteMapping_GetsConcreateImplementationOfInterface()
    {
        var sourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.00001m, 0.0001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        var pqImplementationFactory = new PQImplementationFactory();

        var pqTickInstant = pqImplementationFactory.GetConcreteMapping<IPQPublishableTickInstant>(sourceTickerInfo);
        Assert.AreEqual(typeof(PQPublishableTickInstant), pqTickInstant.GetType());

        var pqLevel1Quote = pqImplementationFactory.GetConcreteMapping<IPQPublishableLevel1Quote>(sourceTickerInfo);
        Assert.AreEqual(typeof(PQPublishableLevel1Quote), pqLevel1Quote.GetType());

        var pqLevel2Quote = pqImplementationFactory.GetConcreteMapping<IPQPublishableLevel2Quote>(sourceTickerInfo);
        Assert.AreEqual(typeof(PQPublishableLevel2Quote), pqLevel2Quote.GetType());

        var pqLevel3Quote = pqImplementationFactory.GetConcreteMapping<IPQPublishableLevel3Quote>(sourceTickerInfo);
        Assert.AreEqual(typeof(PQPublishableLevel3Quote), pqLevel3Quote.GetType());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void NonSupportedPQType_GetConcreteMapping_GetsConcreteImplementationOfInterface()
    {
        var sourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.00001m, 0.0001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.OrderTraderName | LayerFlags.OrderSize | LayerFlags.OrdersCount
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        var pqImplementationFactory = new PQImplementationFactory();

        pqImplementationFactory.GetConcreteMapping<PQTradingStatusFeedEvent>(sourceTickerInfo);
    }
}
