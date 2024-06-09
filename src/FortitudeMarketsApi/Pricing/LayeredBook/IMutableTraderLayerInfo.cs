// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IMutableTraderLayerInfo : ITraderLayerInfo
{
    new string? TraderName   { get; set; }
    new decimal TraderVolume { get; set; }
    new bool    IsEmpty      { get; set; }

    new IMutableTraderLayerInfo Clone();
}
