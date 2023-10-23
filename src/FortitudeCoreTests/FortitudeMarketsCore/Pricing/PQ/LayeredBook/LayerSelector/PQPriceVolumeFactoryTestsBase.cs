#region

using FortitudeMarketsCore.Pricing.PQ.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.LayeredBook.LayerSelector;

public class PQPriceVolumeFactoryTestsBase
{
    protected PQPriceVolumeLayer SimplePvl = null!;
    protected PQSourcePriceVolumeLayer SourcePvl = null!;
    protected PQSourceQuoteRefPriceVolumeLayer SourceQtRefPvl = null!;
    protected PQSourceQuoteRefTraderValueDatePriceVolumeLayer SrcQtRefTrdrVlDtPvl = null!;
    protected PQTraderPriceVolumeLayer TraderPvl = null!;
    protected PQValueDatePriceVolumeLayer VlDtPvl = null!;

    [TestInitialize]
    public void Setup()
    {
        SimplePvl = new PQPriceVolumeLayer(1.234567m, 3_000_000);
        SourcePvl = new PQSourcePriceVolumeLayer(2.345678m, 4_000_000, null,
            "TestSourceName", true);
        SourceQtRefPvl = new PQSourceQuoteRefPriceVolumeLayer(3.456789m, 5_000_000, null,
            "TestSourceNameForQtRef", true, 42_192_192);
        VlDtPvl = new PQValueDatePriceVolumeLayer(4.567890m, 6_000_000, new DateTime(2018, 1, 7, 18, 0, 0));
        TraderPvl = new PQTraderPriceVolumeLayer(5.678901m, 7_000_000)
        {
            [0] = new PQTraderLayerInfo(new PQNameIdLookupGenerator(0), "TestTraderName1", 1_000_000)
            , [1] = new PQTraderLayerInfo(new PQNameIdLookupGenerator(0), "TestTraderName2", 2_000_000)
        };
        SrcQtRefTrdrVlDtPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(6.789012m, 8_000_000, null,
            null, new DateTime(2018, 1, 6, 15, 0, 0), "TestSourceNameForSrcQtRefTrdrVlDt", true, 123445);
    }
}
