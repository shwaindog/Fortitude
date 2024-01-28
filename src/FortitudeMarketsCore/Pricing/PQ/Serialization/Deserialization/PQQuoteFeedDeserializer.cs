#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization.SyncState;

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


    public override PQLevel0Quote? Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0)
            throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0)
            throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IBufferContext bufferContext)
        {
            var sockBuffContext = bufferContext as ReadSocketBufferContext;
            if (sockBuffContext != null) sockBuffContext.DeserializerTimestamp = TimeContext.UtcNow;

            var sequenceId = StreamByteOps.ToUInt(bufferContext.EncodedBuffer!.Buffer
                , bufferContext.EncodedBuffer.ReadCursor + PQQuoteMessageHeader.SequenceIdOffset);
            UpdateQuote(bufferContext, PublishedQuote, sequenceId);
            PushQuoteToSubscribers(PQSyncStatus.Good, sockBuffContext?.DispatchLatencyLogger);
            if (feedIsStopped)
                OnSyncOk(this);
            else
                OnReceivedUpdate(this);
            feedIsStopped = false;
            return PublishedQuote as PQLevel0Quote;
        }

        throw new ArgumentException("Expected readContext to be of type IBufferContext");
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
