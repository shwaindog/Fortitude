#region

using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

#endregion

namespace FortitudeTests.FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook.LayerSelector;

public class PQPriceVolumeFactoryTestsBase
{
    protected IPQNameIdLookupGenerator NameIdLookupGenerator = null!;
    protected PQPriceVolumeLayer SimplePvl = null!;
    protected PQSourcePriceVolumeLayer SourcePvl = null!;
    protected PQSourceQuoteRefPriceVolumeLayer SourceQtRefPvl = null!;
    protected PQSourceQuoteRefTraderValueDatePriceVolumeLayer SrcQtRefTrdrVlDtPvl = null!;
    protected PQTraderPriceVolumeLayer TraderPvl = null!;
    protected IPQNameIdLookupGenerator TraderSourceNameIdLookupGenerator = null!;
    protected PQValueDatePriceVolumeLayer VlDtPvl = null!;

    [TestInitialize]
    public void Setup()
    {
        NameIdLookupGenerator
            = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
        TraderSourceNameIdLookupGenerator = NameIdLookupGenerator.Clone();
        SimplePvl = new PQPriceVolumeLayer(1.234567m, 3_000_000);
        SourcePvl = new PQSourcePriceVolumeLayer(NameIdLookupGenerator.Clone(), 2.345678m, 4_000_000, "TestSourceName", true);
        SourceQtRefPvl = new PQSourceQuoteRefPriceVolumeLayer(NameIdLookupGenerator.Clone(), 3.456789m, 5_000_000,
            "TestSourceNameForQtRef", true, 42_192_192);
        VlDtPvl = new PQValueDatePriceVolumeLayer(4.567890m, 6_000_000, new DateTime(2018, 1, 7, 18, 0, 0));
        TraderPvl = new PQTraderPriceVolumeLayer(TraderSourceNameIdLookupGenerator, 5.678901m, 7_000_000)
        {
            [0] = new PQTraderLayerInfo(TraderSourceNameIdLookupGenerator, "TestTraderName1", 1_000_000)
            , [1] = new PQTraderLayerInfo(TraderSourceNameIdLookupGenerator, "TestTraderName2", 2_000_000)
        };
        SrcQtRefTrdrVlDtPvl = new PQSourceQuoteRefTraderValueDatePriceVolumeLayer(NameIdLookupGenerator.Clone(), 6.789012m, 8_000_000
            , new DateTime(2018, 1, 6, 15, 0, 0), "TestSourceNameForSrcQtRefTrdrVlDt", true, 123445);
    }
}
