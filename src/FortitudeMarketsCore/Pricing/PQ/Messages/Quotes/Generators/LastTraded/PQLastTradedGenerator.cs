// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.LastTraded;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LastTraded;
using FortitudeMarketsCore.Pricing.Quotes.Generators.LastTraded;
using MathNet.Numerics.Distributions;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.Generators.LastTraded;

public class PQLastTradedGenerator : LastTradedGenerator
{
    private readonly IPQNameIdLookupGenerator consistentNameIdLookupGenerator
        = new PQNameIdLookupGenerator(PQFieldKeys.LastTraderDictionaryUpsertCommand);

    public PQLastTradedGenerator(GenerateLastTradeInfo generateLastTradeInfo, Normal normalDist, Random pseudoRandom)
        : base(generateLastTradeInfo, normalDist, pseudoRandom) { }


    protected override void InitializeRecentlyTraded(IRecentlyTraded recentlyTraded)
    {
        if (recentlyTraded is IPQRecentlyTraded pqRecentlyTraded) pqRecentlyTraded.NameIdLookup.CopyFrom(consistentNameIdLookupGenerator);
    }

    protected override void SetTraderName(IMutableLastTraderPaidGivenTrade lastTraderPaidGivenTrade, string traderName)
    {
        var id = consistentNameIdLookupGenerator.GetOrAddId(traderName);
        lastTraderPaidGivenTrade.TraderName = traderName;

        // if (lastTraderPaidGivenTrade is IPQLastTraderPaidGivenTrade pqTraderLastTrade)
        //     if (pqTraderLastTrade.TraderId != id)
        //         Debugger.Break();
    }
}
