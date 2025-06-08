// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Reactive.Disposables;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeCommon.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary;
using FortitudeIO.Protocols.Serdes.Binary.Sockets;
using FortitudeIO.Transports.Network.Logging;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public abstract class PQMessageDeserializerBase<T> : MessageDeserializer<T>, IPQMessageDeserializer<T>
    where T : class, IPQMessage
{
    private const byte SupportFromVersion = 1;
    private const byte SupportToVersion   = 1;

    private readonly PQSerializationFlags serializationFlags;

    protected readonly IList<IObserver<T>> Subscribers = new List<IObserver<T>>();

    protected Func<ISourceTickerInfo, T> QuoteFactory;

    protected PQMessageDeserializerBase
    (ISourceTickerInfo tickerPricingSubscriptionConfig,
        PQSerializationFlags serializationFlags = PQSerializationFlags.ForSocketPublish)
    {
        this.serializationFlags = serializationFlags;
        Identifier              = tickerPricingSubscriptionConfig;
        QuoteFactory            = sqi => ConcreteFinder.GetConcreteMapping<T>(sqi);
        PublishedQuote          = ConcreteFinder.GetConcreteMapping<T>(tickerPricingSubscriptionConfig);
    }

    protected PQMessageDeserializerBase(PQMessageDeserializerBase<T> toClone) : base(toClone)
    {
        serializationFlags = toClone.serializationFlags;
        Identifier         = toClone.Identifier;
        ReceivedUpdate     = toClone.ReceivedUpdate;
        SyncOk             = toClone.SyncOk;
        OutOfSync          = toClone.OutOfSync;
        QuoteFactory       = toClone.QuoteFactory;
        PublishedQuote     = (T)toClone.PublishedQuote.Clone();
    }

    protected byte StorageVersion { get; set; }

    public static IPQImplementationFactory ConcreteFinder { get; set; } = new PQImplementationFactory();

    protected virtual bool ShouldPublish => PublishedQuote.HasUpdates;

    public T PublishedQuote { get; protected set; }

    public ISourceTickerInfo       Identifier { get; }
    public IPQMessageDeserializer? Previous   { get; set; }
    public IPQMessageDeserializer? Next       { get; set; }

    public event Action<IPQMessageDeserializer>? ReceivedUpdate;
    public event Action<IPQMessageDeserializer>? SyncOk;
    public event Action<IPQMessageDeserializer>? OutOfSync;

    public void OnReceivedUpdate(IPQMessageDeserializer quoteDeserializer)
    {
        var onReceivedUpdate = ReceivedUpdate;
        onReceivedUpdate?.Invoke(quoteDeserializer);
    }

    public void OnSyncOk(IPQMessageDeserializer quoteDeserializer)
    {
        SyncOk?.Invoke(quoteDeserializer);
    }

    public void OnOutOfSync(IPQMessageDeserializer quoteDeserializer)
    {
        OutOfSync?.Invoke(quoteDeserializer);
    }

    public virtual bool HasTimedOutAndNeedsSnapshot(DateTime utcNow) => false;

    public virtual bool CheckResync(DateTime utcNow) => false;

    public IDisposable Subscribe(IObserver<T> observer)
    {
        PublishedQuote.Lock.Acquire();
        try
        {
            Subscribers.Add(observer);
        }
        finally
        {
            PublishedQuote.Lock.Release();
        }

        return Disposable.Create(() =>
        {
            PublishedQuote.Lock.Acquire();
            try
            {
                Subscribers.Remove(observer);
            }
            finally
            {
                PublishedQuote.Lock.Release();
            }
        });
    }

    public unsafe int UpdateEntity(IMessageBufferContext readContext, T ent, uint sequenceId)
    {
        ent.UpdateComplete(ent.PQSequenceId);

        using var fixedBuffer = readContext.EncodedBuffer!;

        var fptr   = fixedBuffer.ReadBuffer;
        var offset = readContext.EncodedBuffer!.BufferRelativeReadCursor;
        //Console.Out.WriteLine($"{TimeContext.LocalTimeNow:O} Deserializing {sequenceId} with {length} bytes.");
        var   messageFlags = PQMessageFlags.IncludesSequenceId;
        byte  version;
        byte* end;
        uint  msgSize;

        var startAddress = fptr + offset;
        var ptr          = startAddress;

        if (serializationFlags != PQSerializationFlags.ForSocketPublish)
        {
            messageFlags = (PQMessageFlags)(*ptr++);

            if ((messageFlags & PQMessageFlags.ThreeByteMessageSize) == PQMessageFlags.ThreeByteMessageSize)
            {
                uint firstByte = *ptr++;
                if (BitConverter.IsLittleEndian)
                {
                    msgSize =   StreamByteOps.ToUShort(ref ptr);
                    msgSize <<= 8;
                    msgSize |=  firstByte;
                }
                else
                {
                    msgSize =  firstByte << 16;
                    msgSize |= StreamByteOps.ToUShort(ref ptr);
                }
            }
            else if ((messageFlags & PQMessageFlags.OneByteMessageSize) > 0)
            {
                msgSize = *ptr++;
            }
            else
            {
                msgSize = StreamByteOps.ToUShort(ref ptr);
            }

            version = StorageVersion;
            end     = startAddress + msgSize;
        }
        else
        {
            msgSize = readContext.MessageHeader.MessageSize;
            version = readContext.MessageHeader.Version;
            end     = startAddress + msgSize - PQQuoteMessageHeader.HeaderSize;
        }

        if (version < SupportFromVersion || version > SupportToVersion)
        {
            logger.Warn("Received unsupported message version {0} will skip processing", version);
            return (int)msgSize;
        }

        if (serializationFlags == PQSerializationFlags.ForSocketPublish || (messageFlags & PQMessageFlags.IncludesSequenceId) > 0)
            sequenceId = StreamByteOps.ToUInt(ref ptr);
        ent.UpdateStarted(sequenceId);
        if (readContext is SocketBufferReadContext sockBuffContext && serializationFlags == PQSerializationFlags.ForSocketPublish)
        {
            ent.ClientReceivedTime         = sockBuffContext.DetectTimestamp;
            ent.InboundSocketReceivingTime = sockBuffContext.ReceivingTimestamp;
            ent.InboundProcessedTime       = sockBuffContext.DeserializerTime;
        }
        ent.IsCompleteUpdate = !messageFlags.HasIsNotLastMessageOfUpdateFlag();
        ent.PQSequenceId     = sequenceId;

        while (ptr < end)
        {
            var flags     = (PQFieldFlags)(*ptr++);
            var id        = (PQFeedFields)(*ptr++);

            var hasDepth = false;
            var depthKey = PQDepthKey.None;
            if (flags.HasDepthKeyFlag())
            {
                hasDepth = true;
                var depthByte = *ptr++;
                if (depthByte.IsTwoByteDepth())
                    depthKey = depthByte.ToDepthKey(*ptr++);
                else
                    depthKey = depthByte.ToDepthKey();
                // depthKey = depthByte.IsTwoByteDepth() ? depthByte.ToDepthKey(*ptr++) : depthByte.ToDepthKey();
            }

            PQPricingSubFieldKeys subId = PQPricingSubFieldKeys.None;

            if (flags.HasSubIdFlag()) subId = (PQPricingSubFieldKeys)(*ptr++);

            ushort auxiliaryPayload = 0;

            if (flags.HasAuxiliaryPayloadFlag()) auxiliaryPayload = StreamByteOps.ToUShort(ref ptr);

            var payload = StreamByteOps.ToUInt(ref ptr);

            var pqFieldUpdate = hasDepth
                ? new PQFieldUpdate(id, depthKey, subId, auxiliaryPayload, payload, flags)
                : new PQFieldUpdate(id, subId, auxiliaryPayload, payload, flags);
            // logger.Info("de-{0}-{1}", sequenceId, pqFieldUpdate);
            var moreBytes = ent.UpdateField(pqFieldUpdate);
            if (moreBytes <= 0 || ptr + moreBytes + 4 > end) continue;
            var stringUpdate = new PQStringUpdate
            {
                DictionaryId = StreamByteOps.ToInt(ref ptr), Value = StreamByteOps.ToString(ref ptr, moreBytes)
              , Command      = (CrudCommand)pqFieldUpdate.SubIdByte
            };
            var fieldStringUpdate = new PQFieldStringUpdate
            {
                Field = pqFieldUpdate, StringUpdate = stringUpdate
            };
            // logger.Info("de-{1}-{0}", sequenceId, fieldStringUpdate);
            ent.UpdateFieldString(fieldStringUpdate);
        }
        if (serializationFlags == PQSerializationFlags.ForSocketPublish)
        {
            ent.FeedMarketConnectivityStatus |= messageFlags.HasSnapshotFlag() 
                ? FeedConnectivityStatusFlags.FromAdapterSnapshot 
                : FeedConnectivityStatusFlags.None;
        }

        // logger.Info("Deserialized Quote {0}: SequenceId:{1} on Deserializer.InstanceNum {2}",
        //     ent.GetType().Name, ent.PQSequenceId, InstanceNumber);
        return (int)msgSize;
    }

    public void PushQuoteToSubscribers
    (FeedSyncStatus syncStatus,
        IPerfLogger? detectionToPublishLatencyTraceLogger = null)
    {
        if (!PublishedQuote.IsCompleteUpdate)
        {
            return;
        }
        if (!Subscribers.Any() && !AllDeserializedNotifiers.Any()) return;
        var tl = PublishPQQuoteDeserializerLatencyTraceLoggerPool.StartNewTrace();
        PublishedQuote.Lock.Acquire();
        try
        {
            PublishedQuote.FeedSyncStatus = syncStatus;
            if (!ShouldPublish) return;
            PublishedQuote.SubscriberDispatchedTime = TimeContext.UtcNow;
            if (tl.Enabled) tl.Add("Ticker", Identifier.InstrumentName);
            detectionToPublishLatencyTraceLogger?.Add(SocketDataLatencyLogger.BeforePublish);
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < Subscribers.Count; i++) Subscribers[i].OnNext(PublishedQuote);
            OnNotify(PublishedQuote);
            if (tl.Enabled) tl.Add("Source", Identifier.SourceName);
            PublishedQuote.HasUpdates = false;
        }
        catch
        {
            // ignored most likely client uncaught exception
        }
        finally
        {
            PublishPQQuoteDeserializerLatencyTraceLoggerPool.StopTrace(tl);
            PublishedQuote.Lock.Release();
        }
    }

    // ReSharper disable FieldCanBeMadeReadOnly.Local
    // ReSharper disable StaticMemberInGenericType
    // ReSharper disable InconsistentNaming
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQMessageDeserializerBase<>));

    private static IPerfLoggerPool PublishPQQuoteDeserializerLatencyTraceLoggerPool =
        PerfLoggingPoolFactory.Instance.GetLatencyTracingLoggerPool("clientCallback",
                                                                    TimeSpan.FromMilliseconds(10), typeof(UserCallback));
    // ReSharper restore InconsistentNaming
    // ReSharper restore StaticMemberInGenericType
    // ReSharper restore FieldCanBeMadeReadOnly.Local
}
