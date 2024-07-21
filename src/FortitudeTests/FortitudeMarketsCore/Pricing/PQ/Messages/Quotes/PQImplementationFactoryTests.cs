// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.LastTraded;
using FortitudeMarketsApi.Pricing.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using static FortitudeMarketsApi.Configuration.ClientServerConfig.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.TickerDetailLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

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
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        var pqImplementationFactory = new PQImplementationFactory();

        var pqTickInstant = pqImplementationFactory.GetConcreteMapping<IPQTickInstant>(sourceTickerInfo);
        Assert.AreEqual(typeof(PQTickInstant), pqTickInstant.GetType());

        var pqLevel1Quote = pqImplementationFactory.GetConcreteMapping<IPQLevel1Quote>(sourceTickerInfo);
        Assert.AreEqual(typeof(PQLevel1Quote), pqLevel1Quote.GetType());

        var pqLevel2Quote = pqImplementationFactory.GetConcreteMapping<IPQLevel2Quote>(sourceTickerInfo);
        Assert.AreEqual(typeof(PQLevel2Quote), pqLevel2Quote.GetType());

        var pqLevel3Quote = pqImplementationFactory.GetConcreteMapping<IPQLevel3Quote>(sourceTickerInfo);
        Assert.AreEqual(typeof(PQLevel3Quote), pqLevel3Quote.GetType());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void NonSupportedPQType_GetConcreteMapping_GetsConcreateImplementationOfInterface()
    {
        var sourceTickerInfo =
            new SourceTickerInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3Quote, Unknown
               , 20, 0.00001m, 0.0001m, 30000m, 50000000m, 1000m, 1
               , layerFlags: LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
               , lastTradedFlags: LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName | LastTradedFlags.LastTradedVolume |
                                  LastTradedFlags.LastTradedTime);
        var pqImplementationFactory = new PQImplementationFactory();

        pqImplementationFactory.GetConcreteMapping<PQLevel1QuoteTests.DummyLevel1Quote>(sourceTickerInfo);
    }
}
