// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes.LayeredBook.Layers;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

// The reason for so many sub-types of IPriceVolume Layer is to reduce Quote memory waste when
// duplicated 1000s of times in a ring.
[JsonDerivedType(typeof(PriceVolumeLayer))]
[JsonDerivedType(typeof(SourcePriceVolumeLayer))]
[JsonDerivedType(typeof(SourceQuoteRefPriceVolumeLayer))]
[JsonDerivedType(typeof(ValueDatePriceVolumeLayer))]
[JsonDerivedType(typeof(OrdersCountPriceVolumeLayer))]
[JsonDerivedType(typeof(OrdersPriceVolumeLayer))]
[JsonDerivedType(typeof(FullSupportPriceVolumeLayer))]
[JsonDerivedType(typeof(PQPriceVolumeLayer))]
[JsonDerivedType(typeof(PQSourcePriceVolumeLayer))]
[JsonDerivedType(typeof(PQSourceQuoteRefPriceVolumeLayer))]
[JsonDerivedType(typeof(PQValueDatePriceVolumeLayer))]
[JsonDerivedType(typeof(PQOrdersPriceVolumeLayer))]
[JsonDerivedType(typeof(PQOrdersCountPriceVolumeLayer))]
[JsonDerivedType(typeof(PQFullSupportPriceVolumeLayer))]
public interface IPriceVolumeLayer : IReusableQuoteElement<IPriceVolumeLayer>, IShowsEmpty
{
    [JsonIgnore] LayerType  LayerType          { get; }
    [JsonIgnore] LayerFlags SupportsLayerFlags { get; }

    decimal Price  { get; }
    decimal Volume { get; }
}

public interface IMutablePriceVolumeLayer : IReusableQuoteElement<IMutablePriceVolumeLayer>, IPriceVolumeLayer
  , ITrackableReset<IMutablePriceVolumeLayer>, IEmptyable
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal Price { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal Volume { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new bool IsEmpty { get; set; }

    new IMutablePriceVolumeLayer Clone();
}
