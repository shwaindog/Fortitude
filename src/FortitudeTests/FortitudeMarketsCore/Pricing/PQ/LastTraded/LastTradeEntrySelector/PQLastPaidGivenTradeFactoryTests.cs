using FortitudeMarketsCore.Pricing.PQ.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LastTraded.LastTradeEntrySelector
{
    [TestClass]
    public class PQLastPaidGivenTradeFactoryTests : PQLastTradeFactoryTestsBase
    {
        [TestMethod]
        public void NewPQLastPaidGivenTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
        {
            var ltFactory = new PQLastPaidGivenTradeFactory();

            var emptyPaidGivenTrade = new PQLastPaidGivenTrade();

            Assert.AreEqual(typeof(PQLastPaidGivenTrade), ltFactory.EntryCreationType);
            Assert.AreEqual(emptyPaidGivenTrade, ltFactory.CreateNewLastTradeEntry());
        }

        [TestMethod]
        public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
        {
            var ltFactory = new PQLastPaidGivenTradeFactory();

            var simpleLastTrade = ltFactory.UpgradeLayer(populatedLastTrade);
            Assert.IsTrue(populatedLastTrade.AreEquivalent(simpleLastTrade));

            var paidGivenLastTrade = ltFactory.UpgradeLayer(populatedLastPaidGivenTrade);
            Assert.AreEqual(paidGivenLastTrade, populatedLastPaidGivenTrade);

            var traderLastTrade =
                ltFactory.UpgradeLayer(populatedLastTraderPQLastPaidGivenTrade);
            Assert.IsTrue(traderLastTrade.AreEquivalent(populatedLastTraderPQLastPaidGivenTrade));
        }
    }
}