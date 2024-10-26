// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public interface ISourcePriceVolumeLayer : IPriceVolumeLayer, ICloneable<ISourcePriceVolumeLayer>
{
    string? SourceName { get; }
    bool    Executable { get; }

    new ISourcePriceVolumeLayer Clone();
}

public interface IMutableSourcePriceVolumeLayer : ISourcePriceVolumeLayer, IMutablePriceVolumeLayer
{
    new string? SourceName { get; set; }
    new bool    Executable { get; set; }

    new IMutableSourcePriceVolumeLayer Clone();
}
