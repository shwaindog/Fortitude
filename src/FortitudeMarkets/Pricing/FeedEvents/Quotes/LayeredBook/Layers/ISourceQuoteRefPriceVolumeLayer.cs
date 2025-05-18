// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public interface ISourceQuoteRefPriceVolumeLayer : ISourcePriceVolumeLayer,
    ICloneable<ISourceQuoteRefPriceVolumeLayer>
{
    uint SourceQuoteReference { get; }

    new ISourceQuoteRefPriceVolumeLayer Clone();
}

public interface IMutableSourceQuoteRefPriceVolumeLayer : IMutableSourcePriceVolumeLayer,
    ISourceQuoteRefPriceVolumeLayer, ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>, ITrackableReset<IMutableSourceQuoteRefPriceVolumeLayer>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new uint SourceQuoteReference { get; set; }

    new IMutableSourceQuoteRefPriceVolumeLayer Clone();
    new IMutableSourceQuoteRefPriceVolumeLayer ResetWithTracking();
}
