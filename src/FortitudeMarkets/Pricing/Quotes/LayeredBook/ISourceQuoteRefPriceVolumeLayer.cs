// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Types;

#endregion

namespace FortitudeMarkets.Pricing.Quotes.LayeredBook;

public interface ISourceQuoteRefPriceVolumeLayer : ISourcePriceVolumeLayer,
    ICloneable<ISourceQuoteRefPriceVolumeLayer>
{
    uint SourceQuoteReference { get; }

    new ISourceQuoteRefPriceVolumeLayer Clone();
}

public interface IMutableSourceQuoteRefPriceVolumeLayer : IMutableSourcePriceVolumeLayer,
    ISourceQuoteRefPriceVolumeLayer, ICloneable<IMutableSourceQuoteRefPriceVolumeLayer>
{
    new uint SourceQuoteReference { get; set; }

    new IMutableSourceQuoteRefPriceVolumeLayer Clone();
}
