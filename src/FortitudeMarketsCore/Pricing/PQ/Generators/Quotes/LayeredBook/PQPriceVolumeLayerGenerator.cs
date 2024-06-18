// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeMarketsApi.Pricing.LayeredBook;
using FortitudeMarketsCore.Pricing.Generators.Quotes.LayeredBook;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DictionaryCompression;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.LayeredBook;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.Generators.LayeredBook;

public class PQPriceVolumeLayerGenerator : PriceVolumeLayerGenerator
{
    private readonly IPQNameIdLookupGenerator consistentNameIdGenerator;

    public PQPriceVolumeLayerGenerator(GenerateBookLayerInfo generateLayerInfo, IPQNameIdLookupGenerator quoteTrackingGenerator) :
        base(generateLayerInfo) =>
        consistentNameIdGenerator = quoteTrackingGenerator;


    protected override void SetExecutable(IMutableSourcePriceVolumeLayer sourcePvL, bool executable)
    {
        if (sourcePvL is PQSourcePriceVolumeLayer pqSourcePvl) pqSourcePvl.IsExecutableUpdated = !executable;

        if (sourcePvL is PQSourceQuoteRefTraderValueDatePriceVolumeLayer pqSourceQtRefTrdrPvl) pqSourceQtRefTrdrPvl.IsExecutableUpdated = !executable;

        sourcePvL.Executable = executable;
    }

    protected override void SetTraderName(IMutableTraderLayerInfo traderLayerInfo, string name)
    {
        var id = consistentNameIdGenerator.GetOrAddId(name);
        traderLayerInfo.TraderName = name;
    }

    protected override void SetSourceName(IMutableSourcePriceVolumeLayer sourcePvL, string name)
    {
        var id = consistentNameIdGenerator.GetOrAddId(name);
        sourcePvL.SourceName = name;
    }
}
