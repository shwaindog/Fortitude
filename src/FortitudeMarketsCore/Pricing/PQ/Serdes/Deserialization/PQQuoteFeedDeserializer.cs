// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization.SyncState;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

internal class PQQuoteFeedDeserializer<T> : PQQuoteDeserializerBase<T> where T : class, IPQTickInstant
{
    protected static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQQuoteFeedDeserializer<T>));

    private bool feedIsStopped = true;

    public PQQuoteFeedDeserializer(ISourceTickerInfo identifier) : base(identifier)
    {
        if (!string.IsNullOrEmpty(identifier.Ticker)) throw new ArgumentException("Expected no ticker to be specified.");
    }

    public PQQuoteFeedDeserializer(PQQuoteFeedDeserializer<T> toClone) : base(toClone) { }

    protected override bool ShouldPublish => true;


    public override unsafe T Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0) throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0) throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext bufferContext)
        {
            var sockBuffContext                                           = bufferContext as SocketBufferReadContext;
            if (sockBuffContext != null) sockBuffContext.DeserializerTime = TimeContext.UtcNow;
            using var fixedBuffer                                         = bufferContext.EncodedBuffer!;
            var       ptr                                                 = fixedBuffer.ReadBuffer + fixedBuffer.BufferRelativeReadCursor;
            var       sequenceId                                          = StreamByteOps.ToUInt(ref ptr);
            UpdateQuote(bufferContext, PublishedQuote, sequenceId);
            PushQuoteToSubscribers(FeedSyncStatus.Good, sockBuffContext?.DispatchLatencyLogger);
            if (feedIsStopped)
                OnSyncOk(this);
            else
                OnReceivedUpdate(this);
            feedIsStopped = false;
            return PublishedQuote;
        }

        throw new ArgumentException("Expected readContext to be of type IBufferContext");
    }

    public override bool HasTimedOutAndNeedsSnapshot(DateTime utcNow)
    {
        int elapsed;
        if ((elapsed = (int)(utcNow - PublishedQuote.ClientReceivedTime).TotalMilliseconds) <=
            InSyncState<PQTickInstant>.PQTimeoutMs && !feedIsStopped)
            return false;
        feedIsStopped = true;
        Logger.Info("Stale detected on feed {0}, {1}ms elapsed with no update",
                    Identifier.Source, elapsed);
        PushQuoteToSubscribers(FeedSyncStatus.Stale);
        return true;
    }

    public override IMessageDeserializer Clone() => new PQQuoteFeedDeserializer<T>(this);
}
