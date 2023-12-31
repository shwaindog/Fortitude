﻿using System;
using FortitudeMarketsCore.Pricing.PQ.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector
{
    [TestClass]
    public class PQLastTradeFactoryTests : PQLastTradeFactoryTestsBase
    {
        [TestMethod]
        public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
        {
            var ltFactory = new PQLastTradeFactory();

            var emptyPQLastTrade = new PQLastTrade();

            Assert.AreEqual(typeof(PQLastTrade), ltFactory.EntryCreationType);
            Assert.AreEqual(emptyPQLastTrade, ltFactory.CreateNewLastTradeEntry());
        }

        [TestMethod]
        public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
        {
            var ltFactory = new PQLastTradeFactory();

            var fromPopulatedPQLastTrade = ltFactory.UpgradeLayer(populatedLastTrade);
            Assert.AreEqual(fromPopulatedPQLastTrade, populatedLastTrade);

            var paidGivenLastTrade = ltFactory.UpgradeLayer(populatedLastPaidGivenTrade);
            Assert.IsTrue(paidGivenLastTrade.AreEquivalent(populatedLastPaidGivenTrade));

            var traderLastTrade = 
                ltFactory.UpgradeLayer(populatedLastTraderPQLastPaidGivenTrade);
            Assert.IsTrue(traderLastTrade.AreEquivalent(populatedLastTraderPQLastPaidGivenTrade));
        }
    }
}