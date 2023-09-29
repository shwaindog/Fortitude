using FortitudeMarketsCore.Pricing.PQ.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector
{
    [TestClass]
    public class SourcePriceVolumeLayerFactoryTests : PQPriceVolumeFactoryTestsBase
    {
        [TestMethod]
        public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
        {
            var pvlFactory = new SourcePriceVolumeLayerFactory(null);

            var emptyPvl = new PQSourcePriceVolumeLayer();

            Assert.AreEqual(typeof(PQSourcePriceVolumeLayer), pvlFactory.LayerCreationType);
            Assert.AreEqual(emptyPvl, pvlFactory.CreateNewLayer());
        }

        [TestMethod]
        public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
        {
            var pvlFactory = new SourcePriceVolumeLayerFactory(null);

            var simplePvl = pvlFactory.UpgradeLayer(SimplePvl);
            Assert.IsTrue(SimplePvl.AreEquivalent(simplePvl));

            var srcPvl = pvlFactory.UpgradeLayer(SourcePvl);
            Assert.AreEqual(srcPvl, SourcePvl);

            var srcQtRefPvl = pvlFactory.UpgradeLayer(SourceQtRefPvl);
            Assert.IsTrue(srcQtRefPvl.AreEquivalent(SourceQtRefPvl));

            var vlDtPvl = pvlFactory.UpgradeLayer(VlDtPvl);
            Assert.AreEqual(VlDtPvl.Price, vlDtPvl.Price);
            Assert.AreEqual(VlDtPvl.Volume, vlDtPvl.Volume);

            var trdrPvl = pvlFactory.UpgradeLayer(TraderPvl);
            Assert.AreEqual(TraderPvl.Price, trdrPvl.Price);
            Assert.AreEqual(TraderPvl.Volume, trdrPvl.Volume);

            var srcQtRefTrdrVlDtPvl = pvlFactory.UpgradeLayer(SrcQtRefTrdrVlDtPvl);
            Assert.IsTrue(srcQtRefTrdrVlDtPvl.AreEquivalent(SrcQtRefTrdrVlDtPvl));
        }
    }
}