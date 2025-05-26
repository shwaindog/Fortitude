// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.FeedEvents.Generators.Quotes.LastTraded;
using FortitudeMarkets.Pricing.FeedEvents.LastTraded;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.LastTraded;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Quotes.LastTraded;

public class PQLastTradedGenerator : LastTradedGenerator
{
    private readonly IPQNameIdLookupGenerator consistentNameIdLookupGenerator
        = new PQNameIdLookupGenerator(PQFeedFields.LastTradedStringUpdates);

    public PQLastTradedGenerator(GenerateLastTradeInfo generateLastTradeInfo)
        : base(generateLastTradeInfo) { }


    protected override void InitializeRecentlyTraded(IOnTickLastTraded recentlyTraded)
    {
        if (recentlyTraded is IPQOnTickLastTraded pqOnTickLastTraded) pqOnTickLastTraded.NameIdLookup.CopyFrom(consistentNameIdLookupGenerator);
    }

    protected override void SetTraderName(IMutableLastExternalCounterPartyTrade lastExternalCounterPartyTrade, string traderName)
    {
        var id = consistentNameIdLookupGenerator.GetOrAddId(traderName);
        lastExternalCounterPartyTrade.ExternalTraderName = traderName;
    }
}
