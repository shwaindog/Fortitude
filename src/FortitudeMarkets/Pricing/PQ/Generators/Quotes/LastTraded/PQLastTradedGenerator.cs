// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Generators.Quotes.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarkets.Pricing.Quotes.LastTraded;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.Quotes.Generators.LastTraded;

public class PQLastTradedGenerator : LastTradedGenerator
{
    private readonly IPQNameIdLookupGenerator consistentNameIdLookupGenerator
        = new PQNameIdLookupGenerator(PQQuoteFields.LastTradedDictionaryUpsertCommand);

    public PQLastTradedGenerator(GenerateLastTradeInfo generateLastTradeInfo)
        : base(generateLastTradeInfo) { }


    protected override void InitializeRecentlyTraded(IRecentlyTraded recentlyTraded)
    {
        if (recentlyTraded is IPQRecentlyTraded pqRecentlyTraded) pqRecentlyTraded.NameIdLookup.CopyFrom(consistentNameIdLookupGenerator);
    }

    protected override void SetTraderName(IMutableLastTraderPaidGivenTrade lastTraderPaidGivenTrade, string traderName)
    {
        var id = consistentNameIdLookupGenerator.GetOrAddId(traderName);
        lastTraderPaidGivenTrade.TraderName = traderName;
    }
}
