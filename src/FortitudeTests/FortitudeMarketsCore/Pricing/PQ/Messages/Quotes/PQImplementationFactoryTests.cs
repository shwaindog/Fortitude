// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using static FortitudeIO.TimeSeries.MarketClassificationExtensions;
using static FortitudeMarketsApi.Pricing.Quotes.QuoteLevel;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;

[TestClass]
public class PQImplementationFactoryTests
{
    [TestMethod]
    public void NewPQImplementationFactory_GetConcreteMapping_GetsConcreateImplementationOfInterface()
    {
        var sourceTickerQuoteInfo =
            new SourceTickerQuoteInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3, Unknown
               , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
               , LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
               , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                             | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        var pqImplementationFactory = new PQImplementationFactory();

        var pqLevel0Quote = pqImplementationFactory.GetConcreteMapping<IPQLevel0Quote>(sourceTickerQuoteInfo);
        Assert.AreEqual(typeof(PQLevel0Quote), pqLevel0Quote.GetType());

        var pqLevel1Quote = pqImplementationFactory.GetConcreteMapping<IPQLevel1Quote>(sourceTickerQuoteInfo);
        Assert.AreEqual(typeof(PQLevel1Quote), pqLevel1Quote.GetType());

        var pqLevel2Quote = pqImplementationFactory.GetConcreteMapping<IPQLevel2Quote>(sourceTickerQuoteInfo);
        Assert.AreEqual(typeof(PQLevel2Quote), pqLevel2Quote.GetType());

        var pqLevel3Quote = pqImplementationFactory.GetConcreteMapping<IPQLevel3Quote>(sourceTickerQuoteInfo);
        Assert.AreEqual(typeof(PQLevel3Quote), pqLevel3Quote.GetType());
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentOutOfRangeException))]
    public void NonSupportedPQType_GetConcreteMapping_GetsConcreateImplementationOfInterface()
    {
        var sourceTickerQuoteInfo =
            new SourceTickerQuoteInfo
                (ushort.MaxValue, "TestSource", ushort.MaxValue, "TestTicker", Level3, Unknown
               , 20, 0.00001m, 30000m, 50000000m, 1000m, 1
               , LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize | LayerFlags.TraderCount
               , LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                             | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime);
        var pqImplementationFactory = new PQImplementationFactory();

        pqImplementationFactory.GetConcreteMapping<PQLevel1QuoteTests.DummyLevel1Quote>(sourceTickerQuoteInfo);
    }
}
