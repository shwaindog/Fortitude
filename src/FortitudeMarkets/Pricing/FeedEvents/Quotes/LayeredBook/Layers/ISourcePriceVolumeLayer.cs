// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.FeedEvents.Quotes.LayeredBook.Layers;

public interface ISourcePriceVolumeLayer : IPriceVolumeLayer, ICloneable<ISourcePriceVolumeLayer>
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    string? SourceName { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    bool Executable { get; }

    new ISourcePriceVolumeLayer Clone();
}

public interface IMutableSourcePriceVolumeLayer : ISourcePriceVolumeLayer, IMutablePriceVolumeLayer
{
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    new string? SourceName { get; set; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
    new bool Executable { get; set; }

    new IMutableSourcePriceVolumeLayer Clone();
}
