using FortitudeMarketsCore.Pricing.PQ.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector
{
    [TestClass]
    public class SourceQuoteRefTraderValueDatePriceVolumeLayerFactoryTests : PQPriceVolumeFactoryTestsBase
    {
        [TestMethod]
        public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
        {
            var pvlFactory = new SourceQuoteRefTraderValueDatePriceVolumeLayerFactory(null, null);

            var emptyPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer();

            Assert.AreEqual(typeof(PQSourceQuoteRefTraderValueDatePriceVolumeLayer), pvlFactory.LayerCreationType);
            Assert.AreEqual(emptyPvl, pvlFactory.CreateNewLayer());
        }

        [TestMethod]
        public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
        {
            var pvlFactory = new SourceQuoteRefTraderValueDatePriceVolumeLayerFactory(null, null);

            var simplePvl = pvlFactory.UpgradeLayer(SimplePvl);
            Assert.IsTrue(SimplePvl.AreEquivalent(simplePvl));

            var srcPvl = pvlFactory.UpgradeLayer(SourcePvl);
            Assert.IsTrue(SourcePvl.AreEquivalent(srcPvl));

            var srcQtRefPvl = pvlFactory.UpgradeLayer(SourceQtRefPvl);
            Assert.IsTrue(SourceQtRefPvl.AreEquivalent(srcQtRefPvl));

            var vlDtPvl = pvlFactory.UpgradeLayer(VlDtPvl);
            Assert.IsTrue(VlDtPvl.AreEquivalent(vlDtPvl));

            var trdrPvl = pvlFactory.UpgradeLayer(TraderPvl);
            Assert.IsTrue(TraderPvl.AreEquivalent(trdrPvl));

            var srcQtRefTrdrVlDtPvl = pvlFactory.UpgradeLayer(SrcQtRefTrdrVlDtPvl);
            Assert.AreEqual(srcQtRefTrdrVlDtPvl, SrcQtRefTrdrVlDtPvl);
        }
    }
}