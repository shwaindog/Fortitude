// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

namespace FortitudeMarketsApi.Pricing.LayeredBook;

public interface IMutablePriceVolumeLayer : IPriceVolumeLayer
{
    new decimal Price   { get; set; }
    new decimal Volume  { get; set; }
    new bool    IsEmpty { get; set; }
}
