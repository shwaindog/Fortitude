// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.TimeSeries.FileSystem.File.Header;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsApi.Pricing.TimeSeries.FileSystem.File;

public interface IPriceFileHeader : IFileSubHeader
{
    ISourceTickerQuoteInfo SourceTickerQuoteInfo { get; }
}
