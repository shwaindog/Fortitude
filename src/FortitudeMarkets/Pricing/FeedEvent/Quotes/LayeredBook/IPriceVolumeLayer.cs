// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.LayeredBook;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

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
public interface IPriceVolumeLayer : IReusableObject<IPriceVolumeLayer>, IInterfacesComparable<IPriceVolumeLayer>
{
    [JsonIgnore] LayerType  LayerType          { get; }
    [JsonIgnore] LayerFlags SupportsLayerFlags { get; }

    decimal Price  { get; }
    decimal Volume { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    bool IsEmpty { get; }
}

public interface IMutablePriceVolumeLayer : IPriceVolumeLayer
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal Price { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new decimal Volume { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    new bool IsEmpty { get; set; }
}
