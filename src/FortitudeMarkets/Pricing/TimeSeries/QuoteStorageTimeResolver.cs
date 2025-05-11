// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.Quotes;
using static FortitudeMarkets.Pricing.TimeSeries.QuoteTimeSeriesStorageType;

#endregion

namespace FortitudeMarkets.Pricing.TimeSeries;

public interface IQuoteStorageTimeResolver : IStorageTimeResolver<IPublishableTickInstant>, IStorageTimeResolver<ITickInstant>
{
    List<QuoteTimeSeriesStorageType> StorageTypePreferences { get; set; }
}

public class QuoteStorageTimeResolver : IQuoteStorageTimeResolver
{
    private static DateTime unixEpoch       = DateTime.MinValue;
    private static DateTime defaultDateTime = default;

    public static IQuoteStorageTimeResolver Instance = new QuoteStorageTimeResolver();

    public List<QuoteTimeSeriesStorageType> StorageTypePreferences { get; set; } = new()
    {
        SourceTime, ClientReceivedTime, AdapterReceivedTime, AdapterSentTime, SourceBidTime, SourceAskTime
    };

    public DateTime ResolveStorageTime(ITickInstant quoteToStore)
    {
        var currentDateTime = unixEpoch;
        foreach (var extractTimeType in StorageTypePreferences)
        {
            currentDateTime = quoteToStore.SourceTime;
            if (currentDateTime != defaultDateTime && currentDateTime != unixEpoch) return currentDateTime;
        }

        return currentDateTime;
    }

    public DateTime ResolveStorageTime(IPublishableTickInstant quoteToStore)
    {
        var currentDateTime = unixEpoch;
        foreach (var extractTimeType in StorageTypePreferences)
        {
            currentDateTime =
                extractTimeType
                switch
                {
                    SourceTime          => quoteToStore.SourceTime
                  , ClientReceivedTime  => quoteToStore.ClientReceivedTime
                  , AdapterReceivedTime => quoteToStore is IPublishableLevel1Quote l1Quote ? l1Quote.AdapterReceivedTime : defaultDateTime
                  , AdapterSentTime     => quoteToStore is IPublishableLevel1Quote l1Quote ? l1Quote.AdapterSentTime : defaultDateTime
                  , SourceBidTime       => quoteToStore is IPublishableLevel1Quote l1Quote ? l1Quote.SourceBidTime : defaultDateTime
                  , SourceAskTime       => quoteToStore is IPublishableLevel1Quote l1Quote ? l1Quote.SourceAskTime : defaultDateTime
                  , _                   => quoteToStore.SourceTime
                };
            if (currentDateTime != defaultDateTime && currentDateTime != unixEpoch) return currentDateTime;
        }

        return currentDateTime;
    }
}
