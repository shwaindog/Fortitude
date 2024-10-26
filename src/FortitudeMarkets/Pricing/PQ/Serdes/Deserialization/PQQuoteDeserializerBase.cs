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
using FortitudeMarkets.Pricing.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes;
using FortitudeMarkets.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Serdes.Deserialization;

public abstract class PQQuoteDeserializerBase<T> : MessageDeserializer<T>, IPQQuoteDeserializer<T>
    where T : class, IPQTickInstant
{
    private const byte SupportFromVersion = 1;
    private const byte SupportToVersion   = 1;

    private readonly PQSerializationFlags serializationFlags;

    protected readonly IList<IObserver<T>> Subscribers = new List<IObserver<T>>();

    protected Func<ISourceTickerInfo, T> QuoteFactory;

    protected PQQuoteDeserializerBase
    (ISourceTickerInfo tickerPricingSubscriptionConfig,
        PQSerializationFlags serializationFlags = PQSerializationFlags.ForSocketPublish)
    {
        this.serializationFlags = serializationFlags;
        Identifier              = tickerPricingSubscriptionConfig;
        QuoteFactory            = sqi => ConcreteFinder.GetConcreteMapping<T>(sqi);
        PublishedQuote          = ConcreteFinder.GetConcreteMapping<T>(tickerPricingSubscriptionConfig);
    }

    protected PQQuoteDeserializerBase(PQQuoteDeserializerBase<T> toClone) : base(toClone)
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

    public ISourceTickerInfo     Identifier { get; }
    public IPQQuoteDeserializer? Previous   { get; set; }
    public IPQQuoteDeserializer? Next       { get; set; }


    public event Action<IPQQuoteDeserializer>? ReceivedUpdate;
    public event Action<IPQQuoteDeserializer>? SyncOk;
    public event Action<IPQQuoteDeserializer>? OutOfSync;


    public void OnReceivedUpdate(IPQQuoteDeserializer quoteDeserializer)
    {
        var onReceivedUpdate = ReceivedUpdate;
        onReceivedUpdate?.Invoke(quoteDeserializer);
    }

    public void OnSyncOk(IPQQuoteDeserializer quoteDeserializer)
    {
        var onSyncOk = SyncOk;
        onSyncOk?.Invoke(quoteDeserializer);
    }

    public void OnOutOfSync(IPQQuoteDeserializer quoteDeserializer)
    {
        var onOutOfSync = OutOfSync;
        onOutOfSync?.Invoke(quoteDeserializer);
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

    public unsafe int UpdateQuote(IMessageBufferContext readContext, T ent, uint sequenceId)
    {
        if (readContext is SocketBufferReadContext sockBuffContext)
        {
            ent.ClientReceivedTime  = sockBuffContext?.DetectTimestamp ?? DateTime.MinValue;
            ent.SocketReceivingTime = sockBuffContext?.ReceivingTimestamp ?? DateTime.MinValue;
            ent.ProcessedTime       = sockBuffContext?.DeserializerTime ?? DateTime.Now;
        }

        using var fixedBuffer = readContext.EncodedBuffer!;

        var fptr   = fixedBuffer.ReadBuffer;
        var offset = readContext.EncodedBuffer!.BufferRelativeReadCursor;
        //Console.Out.WriteLine($"{TimeContext.LocalTimeNow:O} Deserializing {sequenceId} with {length} bytes.");
        var   storageFlags = StorageFlags.IncludesSequenceId;
        byte  version;
        byte* end;
        uint  msgSize;

        var startAddress = fptr + offset;
        var ptr          = startAddress;

        if (serializationFlags != PQSerializationFlags.ForSocketPublish)
        {
            storageFlags = (StorageFlags)(*ptr++);

            if ((storageFlags & StorageFlags.ThreeByteMessageSize) == StorageFlags.ThreeByteMessageSize)
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
            else if ((storageFlags & StorageFlags.OneByteMessageSize) > 0)
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

        if (serializationFlags == PQSerializationFlags.ForSocketPublish || (storageFlags & StorageFlags.IncludesSequenceId) > 0)
            sequenceId = StreamByteOps.ToUInt(ref ptr);
        ent.PQSequenceId = sequenceId;

        while (ptr < end)
        {
            var    flags = *ptr++;
            ushort id;
            if ((flags & PQFieldFlags.IsExtendedFieldId) == 0)
                id = *ptr++;
            else
                id = StreamByteOps.ToUShort(ref ptr);
            var pqFieldUpdate = new PQFieldUpdate(id, StreamByteOps.ToUInt(ref ptr), flags);
            // logger.Info("de-{0}-{1}", sequenceId, pqFieldUpdate);
            var moreBytes = ent.UpdateField(pqFieldUpdate);
            if (moreBytes <= 0 || ptr + moreBytes + 4 > end) continue;
            var stringUpdate = new PQStringUpdate
            {
                DictionaryId = StreamByteOps.ToInt(ref ptr), Value = StreamByteOps.ToString(ref ptr, moreBytes)
              , Command      = (pqFieldUpdate.Flag & PQFieldFlags.IsUpsert) == PQFieldFlags.IsUpsert ? CrudCommand.Upsert : CrudCommand.Delete
            };
            var fieldStringUpdate = new PQFieldStringUpdate
            {
                Field = pqFieldUpdate, StringUpdate = stringUpdate
            };
            // logger.Info("de-{1}-{0}", fieldStringUpdate);
            ent.UpdateFieldString(fieldStringUpdate);
        }
        // logger.Info("Deserialized Quote {0}: SequenceId:{1} on Deserializer.InstanceNum {2}",
        //     ent.GetType().Name, ent.PQSequenceId, InstanceNumber);
        return (int)msgSize;
    }

    public void PushQuoteToSubscribers
    (FeedSyncStatus syncStatus,
        IPerfLogger? detectionToPublishLatencyTraceLogger = null)
    {
        if (!Subscribers.Any() && !AllDeserializedNotifiers.Any()) return;
        var tl = PublishPQQuoteDeserializerLatencyTraceLoggerPool.StartNewTrace();
        PublishedQuote.Lock.Acquire();
        try
        {
            PublishedQuote.FeedSyncStatus = syncStatus;
            if (!ShouldPublish) return;
            PublishedQuote.DispatchedTime = TimeContext.UtcNow;
            if (tl.Enabled) tl.Add("Ticker", Identifier.Ticker);
            detectionToPublishLatencyTraceLogger?.Add(SocketDataLatencyLogger.BeforePublish);
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < Subscribers.Count; i++) Subscribers[i].OnNext(PublishedQuote);
            OnNotify(PublishedQuote);
            if (tl.Enabled) tl.Add("Source", Identifier.Source);
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
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQQuoteDeserializerBase<>));

    private static IPerfLoggerPool PublishPQQuoteDeserializerLatencyTraceLoggerPool =
        PerfLoggingPoolFactory.Instance.GetLatencyTracingLoggerPool("clientCallback",
                                                                    TimeSpan.FromMilliseconds(10), typeof(UserCallback));
    // ReSharper restore InconsistentNaming
    // ReSharper restore StaticMemberInGenericType
    // ReSharper restore FieldCanBeMadeReadOnly.Local
}
