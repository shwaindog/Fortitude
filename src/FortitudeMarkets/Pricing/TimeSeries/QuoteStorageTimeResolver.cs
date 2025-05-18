// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using static FortitudeMarkets.Pricing.TimeSeries.QuoteTimeSeriesStorageType;

#endregion

namespace FortitudeMarkets.Pricing.TimeSeries;

public interface IQuoteStorageTimeResolver : IStorageTimeResolver<IPublishableTickInstant>, IStorageTimeResolver<ITickInstant>
{
    List<QuoteTimeSeriesStorageType> StorageTypePreferences { get; set; }
}

public class QuoteStorageTimeResolver : IQuoteStorageTimeResolver
{
    private static readonly DateTime DefaultDateTime = default;

    public static IQuoteStorageTimeResolver Instance = new QuoteStorageTimeResolver();

    public List<QuoteTimeSeriesStorageType> StorageTypePreferences { get; set; } =
        [SourceTime, ClientReceivedTime, AdapterReceivedTime, AdapterSentTime, SourceBidTime, SourceAskTime];
    
    
    public DateTime ResolveStorageTime(ITickInstant quoteToStore)
    {
        var currentDateTime = DefaultDateTime;
        currentDateTime = quoteToStore.SourceTime;
        if (currentDateTime == DefaultDateTime)
        {
            throw new ArgumentException("Attempting to store a Quote with no valid time");
        }

        return currentDateTime;
    }

    public DateTime ResolveStorageTime(IPublishableTickInstant quoteToStore)
    {
        var currentDateTime = DefaultDateTime;
        foreach (var extractTimeType in StorageTypePreferences)
        {
            currentDateTime =
                extractTimeType
                switch
                {
                    SourceTime          => quoteToStore.SourceTime
                  , ClientReceivedTime  => quoteToStore.ClientReceivedTime
                  , AdapterReceivedTime => quoteToStore is IPublishableLevel1Quote l1Quote ? l1Quote.AdapterReceivedTime : DefaultDateTime
                  , AdapterSentTime     => quoteToStore is IPublishableLevel1Quote l1Quote ? l1Quote.AdapterSentTime : DefaultDateTime
                  , SourceBidTime       => quoteToStore is IPublishableLevel1Quote l1Quote ? l1Quote.SourceBidTime : DefaultDateTime
                  , SourceAskTime       => quoteToStore is IPublishableLevel1Quote l1Quote ? l1Quote.SourceAskTime : DefaultDateTime
                  , _                   => quoteToStore.SourceTime
                };
            if (currentDateTime != DefaultDateTime) return currentDateTime;
        }
        if (currentDateTime == DefaultDateTime)
        {
            throw new ArgumentException("Attempting to store a Quote with no valid time");
        }

        return currentDateTime;
    }
}
