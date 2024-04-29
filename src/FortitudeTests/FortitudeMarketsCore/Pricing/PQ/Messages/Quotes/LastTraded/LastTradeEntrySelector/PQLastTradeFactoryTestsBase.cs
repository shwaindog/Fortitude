#region

using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

public class PQLastTradeFactoryTestsBase
{
    protected IPQNameIdLookupGenerator NameIdLookupGenerator = null!;
    protected PQLastPaidGivenTrade PopulatedLastPaidGivenTrade = null!;
    protected PQLastTrade PopulatedLastTrade = null!;
    protected PQLastTraderPaidGivenTrade PopulatedLastTraderPQLastPaidGivenTrade = null!;

    [TestInitialize]
    public void Setup()
    {
        NameIdLookupGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);
        PopulatedLastTrade = new PQLastTrade(1.234567m, new DateTime(2018, 01, 07, 12, 15, 4));
        PopulatedLastPaidGivenTrade = new PQLastPaidGivenTrade(2.345678m,
            new DateTime(2018, 01, 06, 11, 14, 4));
        PopulatedLastTraderPQLastPaidGivenTrade = new PQLastTraderPaidGivenTrade(NameIdLookupGenerator, 3.456789m,
            new DateTime(2018, 01, 05, 10, 13, 4))
        {
            TraderName = "PopulatedTraderName"
        };
    }
}
