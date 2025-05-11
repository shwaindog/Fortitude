using System.Text.Json.Serialization;
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.Quotes.TickerInfo;

namespace FortitudeMarkets.Pricing.FeedEvent;

public interface ITickEventFeed
{
    [JsonIgnore] TickerQuoteDetailLevel TickerQuoteDetailLevel { get; }

    [JsonIgnore] bool     IsReplay           { get; }
    [JsonIgnore] DateTime SourceTime         { get; }
    [JsonIgnore] DateTime ClientReceivedTime { get; }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    FeedSyncStatus FeedSyncStatus { get; }

    ITickInstant Quote { get; }

    [JsonIgnore] ISourceTickerInfo? SourceTickerInfo { get; }
}