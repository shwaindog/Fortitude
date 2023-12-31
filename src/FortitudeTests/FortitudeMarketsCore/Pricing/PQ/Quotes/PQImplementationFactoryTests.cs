﻿using System;
using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Quotes
{
    [TestClass]
    public class PQImplementationFactoryTests
    {

        private uint retryWaitMs = 2000;
        private bool allowCatchup = true;
        [TestMethod]
        public void NewPQImplementationFactory_GetConcreteMapping_GetsConcreateImplementationOfInterface()
        {
            var sourceTickerQuoteInfo = new SourceTickerClientAndPublicationConfig(uint.MaxValue, "TestSource",
                "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
                LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
                | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                          | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime, null, 
                retryWaitMs, allowCatchup);
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

        [TestMethod, ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void NonSupportedPQType_GetConcreteMapping_GetsConcreateImplementationOfInterface()
        {
            var sourceTickerQuoteInfo = new SourceTickerClientAndPublicationConfig(uint.MaxValue, "TestSource",
                "TestTicker", 20, 0.00001m, 30000m, 50000000m, 1000m, 1,
                LayerFlags.Volume | LayerFlags.Price | LayerFlags.TraderName | LayerFlags.TraderSize
                | LayerFlags.TraderCount, LastTradedFlags.PaidOrGiven | LastTradedFlags.TraderName
                                          | LastTradedFlags.LastTradedVolume | LastTradedFlags.LastTradedTime, null, 
                retryWaitMs, allowCatchup);
            var pqImplementationFactory = new PQImplementationFactory();

            pqImplementationFactory.GetConcreteMapping<PQLevel1QuoteTests.DummyLevel1Quote>(sourceTickerQuoteInfo);
        }
    }
}