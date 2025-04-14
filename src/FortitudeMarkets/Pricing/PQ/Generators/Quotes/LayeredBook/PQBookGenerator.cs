// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Generators.Quotes.LayeredBook;

public class PQBookGenerator : BookGenerator
{
    private readonly IPQNameIdLookupGenerator
        consistentAskNameIdGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);
    private readonly IPQNameIdLookupGenerator
        consistentBidNameIdGenerator = new PQNameIdLookupGenerator(PQFieldKeys.LayerNameDictionaryUpsertCommand);

    public PQBookGenerator(QuoteBookValuesGenerator quoteBookGenerator) : base(quoteBookGenerator)
    {
        consistentAskNameIdGenerator.GetOrAddId(TraderPriceVolumeLayer.TraderCountOnlyName);
        consistentBidNameIdGenerator.GetOrAddId(TraderPriceVolumeLayer.TraderCountOnlyName);
    }

    public override void InitializeBook(IMutableOrderBook newBook)
    {
        if (newBook is IPQOrderBook pqOrderBook)
            pqOrderBook.NameIdLookup.CopyFrom(newBook.BookSide == BookSide.BidBook
                                                  ? consistentBidNameIdGenerator
                                                  : consistentAskNameIdGenerator, CopyMergeFlags.FullReplace);
        base.InitializeBook(newBook);
    }

    protected override void SetExecutable(BookSide side, IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, bool executable, bool? prevExecutable)
    {
        var isExecutableUpdated = executable == prevExecutable || !executable;

        if (sourcePriceVolumeLayer is PQSourcePriceVolumeLayer pqSourcePvl) pqSourcePvl.IsExecutableUpdated                   = isExecutableUpdated;
        if (sourcePriceVolumeLayer is PQSourceQuoteRefPriceVolumeLayer pqSourceQtRefPvl) pqSourceQtRefPvl.IsExecutableUpdated = isExecutableUpdated;
        if (sourcePriceVolumeLayer is PQSourceQuoteRefTraderValueDatePriceVolumeLayer pqSourceQtRefTrdrPvl)
            pqSourceQtRefTrdrPvl.IsExecutableUpdated = isExecutableUpdated;

        base.SetExecutable(side, sourcePriceVolumeLayer, executable, prevExecutable);
    }

    protected override void SetSourceName
        (BookSide side, IMutableSourcePriceVolumeLayer sourcePriceVolumeLayer, string sourceName, ushort sourceId, ushort? prevSourceId)
    {
        var pqSrcId = side switch
                      {
                          BookSide.AskBook => consistentAskNameIdGenerator.GetOrAddId(sourceName)
                        , BookSide.BidBook => consistentBidNameIdGenerator.GetOrAddId(sourceName)
                        , _                => 0
                      };
        var isSourceNameUpdated = sourceId == prevSourceId;

        if (sourcePriceVolumeLayer is PQSourcePriceVolumeLayer pqSourcePvl) pqSourcePvl.IsSourceNameUpdated                   = isSourceNameUpdated;
        if (sourcePriceVolumeLayer is PQSourceQuoteRefPriceVolumeLayer pqSourceQtRefPvl) pqSourceQtRefPvl.IsSourceNameUpdated = isSourceNameUpdated;
        if (sourcePriceVolumeLayer is PQSourceQuoteRefTraderValueDatePriceVolumeLayer pqSourceQtRefTrdrPvl)
            pqSourceQtRefTrdrPvl.IsSourceNameUpdated = isSourceNameUpdated;

        base.SetSourceName(side, sourcePriceVolumeLayer, sourceName, sourceId, prevSourceId);
    }

    protected override void SetTraderName(BookSide side, IMutableTraderLayerInfo traderLayerInfo, int pos, string traderName, string? prevTraderName)
    {
        var pqSrcId = side switch
                      {
                          BookSide.AskBook => consistentAskNameIdGenerator.GetOrAddId(traderName)
                        , BookSide.BidBook => consistentBidNameIdGenerator.GetOrAddId(traderName)
                        , _                => 0
                      };
        base.SetTraderName(side, traderLayerInfo, pos, traderName, prevTraderName);
    }

    protected override void SetTraderVolume
        (BookSide side, IMutableTraderLayerInfo traderLayerInfo, int pos, decimal traderVolume, decimal? prevTraderVolume)
    {
        var isTraderVolumeUpdated = traderVolume > 0 || traderVolume == prevTraderVolume;

        if (traderLayerInfo is PQTraderLayerInfo pqSourcePvl) pqSourcePvl.IsTraderVolumeUpdated = isTraderVolumeUpdated;

        base.SetTraderVolume(side, traderLayerInfo, pos, traderVolume, prevTraderVolume);
    }
}
