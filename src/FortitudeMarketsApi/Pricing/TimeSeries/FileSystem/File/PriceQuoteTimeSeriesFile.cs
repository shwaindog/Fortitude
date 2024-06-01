// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeIO.Protocols;
using FortitudeIO.TimeSeries;
using FortitudeIO.TimeSeries.FileSystem.File;
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;

#endregion

namespace FortitudeMarketsApi.Pricing.TimeSeries.FileSystem.File;

public interface IPriceQuoteTimeSeriesFile : ITimeSeriesFile
{
    IPriceQuoteFileHeader PriceQuoteFileHeader { get; }
}

public interface IPriceQuoteTimeSeriesFile<TEntry> : IPriceQuoteTimeSeriesFile, ITimeSeriesEntryFile<TEntry>
    where TEntry : class, ITimeSeriesEntry<TEntry>, IVersionedMessage
{
    new IPriceQuoteFileHeader PriceQuoteFileHeader { get; set; }
}

public struct CreateSourceTickerQuoteFile
{
    public CreateSourceTickerQuoteFile(CreateFileParameters fileParameters, ISourceTickerQuoteInfo sourceTickerQuoteInfo)
    {
        FileParameters        = fileParameters;
        SourceTickerQuoteInfo = sourceTickerQuoteInfo;
    }

    public CreateFileParameters FileParameters { get; }

    public ISourceTickerQuoteInfo SourceTickerQuoteInfo { get; }
}
