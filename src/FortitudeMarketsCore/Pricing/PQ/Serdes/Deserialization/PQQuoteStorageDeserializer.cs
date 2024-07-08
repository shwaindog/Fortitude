// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using FortitudeCommon.Chronometry;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Serdes;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

internal class PQQuoteStorageDeserializer<T> : PQQuoteDeserializerBase<T> where T : class, IPQLevel0Quote
{
    protected static readonly IFLogger Logger = FLoggerFactory.Instance.GetLogger(typeof(PQQuoteFeedDeserializer<T>));

    private uint lastSequenceId = 0;

    public PQQuoteStorageDeserializer
    (ISourceTickerQuoteInfo identifier,
        PQSerializationFlags serializationFlags = PQSerializationFlags.ForStorage,
        byte storageVersion = 1)
        : base(identifier, serializationFlags)
    {
        if (string.IsNullOrEmpty(identifier.Ticker)) throw new ArgumentException("Expected no ticker to be specified.");
        StorageVersion = storageVersion;
    }

    public PQQuoteStorageDeserializer(PQQuoteStorageDeserializer<T> toClone) : base(toClone) => StorageVersion = toClone.StorageVersion;

    protected override bool ShouldPublish => true;

    public override T Deserialize(ISerdeContext readContext)
    {
        if ((readContext.Direction & ContextDirection.Read) == 0) throw new ArgumentException("Expected readContext to allow reading");
        if ((readContext.MarshalType & MarshalType.Binary) == 0) throw new ArgumentException("Expected readContext to be a binary buffer context");
        if (readContext is IMessageBufferContext bufferContext)
        {
            var sockBuffContext = bufferContext as SocketBufferReadContext;

            if (sockBuffContext != null) sockBuffContext.DeserializerTime = TimeContext.UtcNow;

            var sequenceId = lastSequenceId + 1;
            var read       = UpdateQuote(bufferContext, PublishedQuote, sequenceId);

            bufferContext.LastReadLength            =  read;
            bufferContext.EncodedBuffer!.ReadCursor += read;

            lastSequenceId = PublishedQuote.PQSequenceId;

            PushQuoteToSubscribers(PriceSyncStatus.Good, sockBuffContext?.DispatchLatencyLogger);
            return PublishedQuote;
        }

        throw new ArgumentException("Expected readContext to be of type IBufferContext");
    }

    public override bool HasTimedOutAndNeedsSnapshot(DateTime utcNow) => false;

    public override IMessageDeserializer Clone() => new PQQuoteStorageDeserializer<T>(this);
}
