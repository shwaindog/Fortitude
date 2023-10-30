#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeIO.Protocols.Serialization;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;
using FortitudeMarketsCore.Pricing.PQ.Subscription;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization;

internal class PQQuoteFeedDeserializer<T> : PQDeserializerBase<T> where T : class, IPQLevel0Quote
{
    protected static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQQuoteFeedDeserializer<T>));

    private bool feedIsStopped = true;

    public PQQuoteFeedDeserializer(ISourceTickerQuoteInfo identifier) : base(identifier)
    {
        if (!string.IsNullOrEmpty(identifier.Ticker))
            throw new ArgumentException("Expected no ticker to be specified.");
    }

    protected override bool ShouldPublish => true;

    public override object Deserialize(DispatchContext dispatchContext)
    {
        dispatchContext.DeserializerTimestamp = TimeContext.UtcNow;
        var msgHeader = dispatchContext.MessageHeader as PQQuoteTransmissionHeader;
        if (msgHeader == null || msgHeader.Origin != PQFeedType.Update) return PublishedQuote;
        UpdateQuote(dispatchContext, PublishedQuote, msgHeader.SequenceId);
        PushQuoteToSubscribers(PQSyncStatus.Good, dispatchContext.DispatchLatencyLogger);
        if (feedIsStopped)
            OnSyncOk(this);
        else
            OnReceivedUpdate(this);
        feedIsStopped = false;
        return PublishedQuote;
    }

    public override bool HasTimedOutAndNeedsSnapshot(DateTime utcNow)
    {
        int elapsed;
        if ((elapsed = (int)(utcNow - PublishedQuote.ClientReceivedTime).TotalMilliseconds) <=
            InSyncState<PQLevel0Quote>.PQTimeoutMs && !feedIsStopped)
            return false;
        feedIsStopped = true;
        Logger.Info("Stale detected on feed {0}, {1}ms elapsed with no update",
            Identifier.Source, elapsed);
        PushQuoteToSubscribers(PQSyncStatus.Stale);
        return true;
    }
}
