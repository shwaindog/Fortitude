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
using FortitudeMarketsApi.Configuration.ClientServerConfig.PricingConfig;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes;
using FortitudeMarketsCore.Pricing.PQ.Messages.Quotes.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Serdes.Serialization;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public abstract class PQDeserializerBase<T> : MessageDeserializer<T>, IPQDeserializer<T>
    where T : class, IPQLevel0Quote
{
    private const byte SupportFromVersion = 1;
    private const byte SupportToVersion = 1;

    private readonly PQSerializationFlags serializationFlags;

    protected readonly IList<IObserver<T>> Subscribers = new List<IObserver<T>>();

    protected Func<ISourceTickerQuoteInfo, T> QuoteFactory;

    protected PQDeserializerBase(ISourceTickerQuoteInfo tickerPricingSubscriptionConfig,
        PQSerializationFlags serializationFlags = PQSerializationFlags.ForSocketPublish)
    {
        this.serializationFlags = serializationFlags;
        Identifier = tickerPricingSubscriptionConfig;
        QuoteFactory = sqi => ConcreteFinder.GetConcreteMapping<T>(sqi);
        PublishedQuote = ConcreteFinder.GetConcreteMapping<T>(tickerPricingSubscriptionConfig);
    }


    protected PQDeserializerBase(PQDeserializerBase<T> toClone) : base(toClone)
    {
        serializationFlags = toClone.serializationFlags;
        Identifier = toClone.Identifier;
        ReceivedUpdate = toClone.ReceivedUpdate;
        SyncOk = toClone.SyncOk;
        OutOfSync = toClone.OutOfSync;
        QuoteFactory = toClone.QuoteFactory;
        PublishedQuote = (T)toClone.PublishedQuote.Clone();
    }

    protected byte StorageVersion { get; set; }

    public static IPQImplementationFactory ConcreteFinder { get; set; } = new PQImplementationFactory();

    protected virtual bool ShouldPublish => PublishedQuote.HasUpdates;
    public ISourceTickerQuoteInfo Identifier { get; }
    public IPQDeserializer? Previous { get; set; }
    public IPQDeserializer? Next { get; set; }


    public event Action<IPQDeserializer>? ReceivedUpdate;
    public event Action<IPQDeserializer>? SyncOk;
    public event Action<IPQDeserializer>? OutOfSync;

    public void OnReceivedUpdate(IPQDeserializer quoteDeserializer)
    {
        var onReceivedUpdate = ReceivedUpdate;
        onReceivedUpdate?.Invoke(quoteDeserializer);
    }

    public void OnSyncOk(IPQDeserializer quoteDeserializer)
    {
        var onSyncOk = SyncOk;
        onSyncOk?.Invoke(quoteDeserializer);
    }

    public void OnOutOfSync(IPQDeserializer quoteDeserializer)
    {
        var onOutOfSync = OutOfSync;
        onOutOfSync?.Invoke(quoteDeserializer);
    }

    public T PublishedQuote { get; protected set; }

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

    public virtual bool HasTimedOutAndNeedsSnapshot(DateTime utcNow) => false;

    public virtual bool CheckResync(DateTime utcNow) => false;

    public unsafe void UpdateQuote(IMessageBufferContext readContext, T ent, uint sequenceId)
    {
        if (readContext is SocketBufferReadContext sockBuffContext)
        {
            ent.ClientReceivedTime = sockBuffContext?.DetectTimestamp ?? DateTime.MinValue;
            ent.SocketReceivingTime = sockBuffContext?.ReceivingTimestamp ?? DateTime.MinValue;
            ent.ProcessedTime = sockBuffContext?.DeserializerTime ?? DateTime.Now;
        }

        using var fixedBuffer = readContext.EncodedBuffer!;
        var fptr = fixedBuffer.ReadBuffer;
        var offset = readContext.EncodedBuffer!.BufferRelativeReadCursor;
        //Console.Out.WriteLine($"{TimeContext.LocalTimeNow:O} Deserializing {sequenceId} with {length} bytes.");
        var storageFlags = StorageFlags.IncludesSequenceId;
        byte version;
        byte* end;
        uint msgSize;
        var ptr = fptr + offset;
        if (serializationFlags != PQSerializationFlags.ForSocketPublish)
        {
            storageFlags = (StorageFlags)(*ptr++);

            if ((storageFlags & StorageFlags.ThreeByteMessageSize) == StorageFlags.ThreeByteMessageSize)
            {
                uint firstByte = *ptr++;
                if (BitConverter.IsLittleEndian)
                {
                    msgSize = firstByte << 16;
                    msgSize |= StreamByteOps.ToUShort(ref ptr);
                }
                else
                {
                    msgSize = StreamByteOps.ToUShort(ref ptr);
                    msgSize <<= 8;
                    msgSize |= firstByte;
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
            end = ptr + msgSize;
        }
        else
        {
            msgSize = readContext.MessageHeader.MessageSize;
            version = readContext.MessageHeader.Version;
            end = ptr + msgSize - PQQuoteMessageHeader.HeaderSize;
        }

        if (version < SupportFromVersion || version > SupportToVersion)
        {
            logger.Warn("Received unsupported message version {0} will skip processing", version);
            return;
        }

        if (serializationFlags == PQSerializationFlags.ForSocketPublish || (storageFlags & StorageFlags.IncludesSequenceId) > 0)
            sequenceId = StreamByteOps.ToUInt(ref ptr);
        ent.PQSequenceId = sequenceId;

        while (ptr < end)
        {
            var flags = *ptr++;
            ushort id;
            if ((flags & PQFieldFlags.IsExtendedFieldId) == 0)
                id = *ptr++;
            else
                id = StreamByteOps.ToUShort(ref ptr);
            var pqFieldUpdate = new PQFieldUpdate(id, StreamByteOps.ToUInt(ref ptr), flags);
            // logger.Info("Received PQDeserializerBase<> received pqFieldUpdate: {0}", pqFieldUpdate);
            var moreBytes = ent.UpdateField(pqFieldUpdate);
            if (moreBytes <= 0 || ptr + moreBytes + 4 > end) continue;
            var stringUpdate = new PQStringUpdate
            {
                DictionaryId = StreamByteOps.ToInt(ref ptr), Value = StreamByteOps.ToString(ref ptr, moreBytes)
                , Command = (pqFieldUpdate.Flag & PQFieldFlags.IsUpsert) == PQFieldFlags.IsUpsert ?
                    CrudCommand.Upsert :
                    CrudCommand.Delete
            };
            var fieldStringUpdate = new PQFieldStringUpdate
            {
                Field = pqFieldUpdate, StringUpdate = stringUpdate
            };
            // logger.Info("Received PQFieldStringUpdate: {0}", fieldStringUpdate);
            ent.UpdateFieldString(fieldStringUpdate);
        }
        // logger.Info("Deserialized Quote {0}: SequenceId:{1} on Deserializer.InstanceNum {2}",
        //     ent.GetType().Name, ent.PQSequenceId, InstanceNumber);
    }

    public void PushQuoteToSubscribers(PQSyncStatus syncStatus,
        IPerfLogger? detectionToPublishLatencyTraceLogger = null)
    {
        if (!Subscribers.Any() && !AllDeserializedNotifiers.Any()) return;
        var tl = PublishPQQuoteDeserializerLatencyTraceLoggerPool.StartNewTrace();
        PublishedQuote.Lock.Acquire();
        try
        {
            PublishedQuote.PQSyncStatus = syncStatus;
            if (!ShouldPublish) return;
            PublishedQuote.DispatchedTime = TimeContext.UtcNow;
            if (tl.Enabled) tl.Add("Ticker", Identifier.Ticker);
            detectionToPublishLatencyTraceLogger?.Add(SocketDataLatencyLogger.BeforePublish);
            // ReSharper disable once ForCanBeConvertedToForeach
            for (var i = 0; i < Subscribers.Count; i++)
                Subscribers[i].OnNext(PublishedQuote);
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
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQDeserializerBase<>));

    private static IPerfLoggerPool PublishPQQuoteDeserializerLatencyTraceLoggerPool =
        PerfLoggingPoolFactory.Instance.GetLatencyTracingLoggerPool("clientCallback",
            TimeSpan.FromMilliseconds(10), typeof(UserCallback));
    // ReSharper restore InconsistentNaming
    // ReSharper restore StaticMemberInGenericType
    // ReSharper restore FieldCanBeMadeReadOnly.Local
}
