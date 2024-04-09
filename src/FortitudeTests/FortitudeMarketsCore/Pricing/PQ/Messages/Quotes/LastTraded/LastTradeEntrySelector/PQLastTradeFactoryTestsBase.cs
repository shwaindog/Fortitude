#region

using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;

#endregion

namespace FortitudeTests.FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded.LastTradeEntrySelector;

public class PQLastTradeFactoryTestsBase
{
    protected PQLastPaidGivenTrade populatedLastPaidGivenTrade = null!;
    protected PQLastTrade populatedLastTrade = null!;
    protected PQLastTraderPaidGivenTrade populatedLastTraderPQLastPaidGivenTrade = null!;

    [TestInitialize]
    public void Setup()
    {
        populatedLastTrade = new PQLastTrade(1.234567m, new DateTime(2018, 01, 07, 12, 15, 4));
        populatedLastPaidGivenTrade = new PQLastPaidGivenTrade(2.345678m,
            new DateTime(2018, 01, 06, 11, 14, 4));
        populatedLastTraderPQLastPaidGivenTrade = new PQLastTraderPaidGivenTrade(3.456789m,
            new DateTime(2018, 01, 05, 10, 13, 4))
        {
            TraderName = "PopulatedTraderName"
        };
    }
}
