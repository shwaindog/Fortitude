// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.File;

#endregion

namespace FortitudeMarketsApi.Pricing.TimeSeries.FileSystem.File;

public interface IPriceQuoteTimeSeriesFile : ITimeSeriesFile
{
    IPriceQuoteFileHeader PriceQuoteFileHeader { get; }
}
