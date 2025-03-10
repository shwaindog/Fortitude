// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarkets.Pricing.Generators.Quotes.LayeredBook;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.Generators.LayeredBook;
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

    public PQBookGenerator(BookGenerationInfo bookGenerationInfo) : base(bookGenerationInfo) { }

    protected override IPriceVolumeLayerGenerator CreatedPriceVolumeLayerGenerator(BookSide side, GenerateBookLayerInfo generateLayerInfo)
    {
        if (side == BookSide.BidBook)
        {
            consistentBidNameIdGenerator.GetOrAddId(TraderPriceVolumeLayer.TraderCountOnlyName);
            return new PQPriceVolumeLayerGenerator(generateLayerInfo, consistentBidNameIdGenerator);
        }
        consistentAskNameIdGenerator.GetOrAddId(TraderPriceVolumeLayer.TraderCountOnlyName);
        return new PQPriceVolumeLayerGenerator(generateLayerInfo, consistentAskNameIdGenerator);
    }

    public override void InitializeBook(IMutableOrderBook newBook)
    {
        if (newBook is IPQOrderBook pqOrderBook)
            pqOrderBook.NameIdLookup.CopyFrom(newBook.BookSide == BookSide.BidBook
                                                  ? consistentBidNameIdGenerator
                                                  : consistentAskNameIdGenerator);
        base.InitializeBook(newBook);
    }
}
