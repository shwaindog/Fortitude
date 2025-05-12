// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.LayerSelector;

[TestClass]
public class PQFullSupportPriceVolumeLayerFactoryTests : PQPriceVolumeFactoryTestsBase
{
    [TestMethod]
    public void NewPQLastTradeFactory_EntryCreationTypeAndCreateNewLastTradeEnty_ReturnExpected()
    {
        var pvlFactory = new PQFullSupportPriceVolumeLayerFactory(
                                                                                      new PQNameIdLookupGenerator(0));

        var emptyPvl = new PQFullSupportPriceVolumeLayer(NameIdLookupGenerator);

        Assert.AreEqual(typeof(PQFullSupportPriceVolumeLayer), pvlFactory.LayerCreationType);
        var fromFactory = pvlFactory.CreateNewLayer();
        fromFactory.HasUpdates = false;
        Assert.AreEqual(emptyPvl, fromFactory);
    }

    [TestMethod]
    public void InitialisedOtherTypes_UpgradeLayer_PreservesAsMuchCommonSupportedFields()
    {
        var pvlFactory = new PQFullSupportPriceVolumeLayerFactory(
                                                                                      new PQNameIdLookupGenerator(0));

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
