using FortitudeIO.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using static FortitudeMarkets.Pricing.TimeSeries.QuoteTimeSeriesStorageType;

namespace FortitudeMarkets.Pricing.TimeSeries;

public interface IFeedEventStorageTimeResolver : IStorageTimeResolver<IFeedEventStatusUpdate>, IStorageTimeResolver<ITradingStatusFeedEvent>
{
    List<QuoteTimeSeriesStorageType> StorageTypePreferences { get; set; }
}

public class FeedEventStorageTimeResolver : IFeedEventStorageTimeResolver
{
    private static readonly DateTime DefaultDateTime = default;

    public static IFeedEventStorageTimeResolver Instance = new FeedEventStorageTimeResolver();

    public List<QuoteTimeSeriesStorageType> StorageTypePreferences { get; set; } =
        [SourceTime, ClientReceivedTime, AdapterReceivedTime, AdapterSentTime, SourceBidTime, SourceAskTime];
    

    public DateTime ResolveStorageTime(ITradingStatusFeedEvent trdingStsEvtToStore)
    {
        var currentDateTime = DefaultDateTime;
        foreach (var extractTimeType in StorageTypePreferences)
        {
            currentDateTime =
                extractTimeType
                switch
                {
                    SourceTime          => trdingStsEvtToStore.LastSourceFeedUpdateTime
                  , ClientReceivedTime  => trdingStsEvtToStore.ClientReceivedTime
                  , AdapterReceivedTime => trdingStsEvtToStore.AdapterReceivedTime
                  , AdapterSentTime     => trdingStsEvtToStore.AdapterSentTime
                  , _                   => trdingStsEvtToStore.ClientReceivedTime
                };
            if (currentDateTime != DefaultDateTime && currentDateTime != DefaultDateTime) return currentDateTime;
        }

        return currentDateTime;
    }
    
    public DateTime ResolveStorageTime(IFeedEventStatusUpdate eventToStore)
    {
        var currentDateTime = DefaultDateTime;
        currentDateTime = eventToStore.ClientReceivedTime;
        if (currentDateTime == DefaultDateTime)
        {
            throw new ArgumentException("Attempting to store a Feed Event with no valid time");
        }

        return currentDateTime;
    }
}