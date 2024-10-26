﻿#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class PQTraderPriceVolumeLayerFactoryTests : PQPriceVolumeFactoryTestsBase
{
    [TestMethod]
    public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var pvlFactory = new PQTraderPriceVolumeLayerFactory(NameIdLookupGenerator.Clone());

        var emptyPvl = new PQTraderPriceVolumeLayer(NameIdLookupGenerator.Clone());

        Assert.AreEqual(typeof(PQTraderPriceVolumeLayer), pvlFactory.LayerCreationType);
        Assert.AreEqual(emptyPvl, pvlFactory.CreateNewLayer());
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var pvlFactory = new PQTraderPriceVolumeLayerFactory(new PQNameIdLookupGenerator(0));

        var simplePvl = pvlFactory.UpgradeLayer(SimplePvl);
        Assert.IsTrue(SimplePvl.AreEquivalent(simplePvl));

        var srcPvl = pvlFactory.UpgradeLayer(SourcePvl);
        Assert.AreEqual(SourcePvl.Price, srcPvl.Price);
        Assert.AreEqual(SourcePvl.Volume, srcPvl.Volume);

        var srcQtRefPvl = pvlFactory.UpgradeLayer(SourceQtRefPvl);
        Assert.AreEqual(SourceQtRefPvl.Price, srcQtRefPvl.Price);
        Assert.AreEqual(SourceQtRefPvl.Volume, srcQtRefPvl.Volume);

        var vlDtPvl = pvlFactory.UpgradeLayer(VlDtPvl);
        Assert.AreEqual(VlDtPvl.Price, vlDtPvl.Price);
        Assert.AreEqual(VlDtPvl.Volume, vlDtPvl.Volume);

        var trdrPvl = pvlFactory.UpgradeLayer(TraderPvl);
        Assert.AreEqual(TraderPvl, trdrPvl);

        var srcQtRefTrdrVlDtPvl = pvlFactory.UpgradeLayer(SrcQtRefTrdrVlDtPvl);
        Assert.IsTrue(srcQtRefTrdrVlDtPvl.AreEquivalent(SrcQtRefTrdrVlDtPvl));
    }
}
