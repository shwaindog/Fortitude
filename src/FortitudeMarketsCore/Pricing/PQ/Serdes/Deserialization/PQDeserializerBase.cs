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
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Serdes.Deserialization;

public abstract class PQDeserializerBase(IUniqueSourceTickerIdentifier identifier) : MessageDeserializer<PQLevel0Quote>
    , IPQDeserializer
{
    public static IPQImplementationFactory ConcreteFinder { get; set; } = new PQImplementationFactory();
    public IUniqueSourceTickerIdentifier Identifier { get; } = identifier;
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

    public virtual bool HasTimedOutAndNeedsSnapshot(DateTime utcNow) => false;

    public virtual bool CheckResync(DateTime utcNow) => false;
}

public abstract class PQDeserializerBase<T> : PQDeserializerBase, IPQDeserializer<T>
    where T : class, IPQLevel0Quote
{
    private const byte SupportFromVersion = 1;
    private const byte SupportToVersion = 1;
    private static IFLogger logger = FLoggerFactory.Instance.GetLogger(typeof(PQDeserializerBase<>));

    // ReSharper disable once StaticMemberInGenericType
    private static IPerfLoggerPool PublishPQQuoteDeserializerLatencyTraceLoggerPool =
        PerfLoggingPoolFactory.Instance.GetLatencyTracingLoggerPool("clientCallback",
            TimeSpan.FromMilliseconds(10), typeof(UserCallback));

    protected readonly IList<IObserver<T>> Subscribers = new List<IObserver<T>>();

    protected Func<ISourceTickerQuoteInfo, T> QuoteFactory;

    protected PQDeserializerBase(ISourceTickerQuoteInfo identifier) : base(identifier)
    {
        QuoteFactory = sqi => ConcreteFinder.GetConcreteMapping<T>(sqi);
        PublishedQuote = ConcreteFinder.GetConcreteMapping<T>(identifier);
    }

    protected virtual bool ShouldPublish => PublishedQuote.HasUpdates;
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

    public unsafe void UpdateQuote(IBufferContext readContext, T ent, uint sequenceId)
    {
        var sockBuffContext = readContext as SocketBufferReadContext;
        ent.ClientReceivedTime = sockBuffContext?.DetectTimestamp ?? DateTime.MinValue;
        ent.SocketReceivingTime = sockBuffContext?.ReceivingTimestamp ?? DateTime.MinValue;
        ent.ProcessedTime = sockBuffContext?.DeserializerTimestamp ?? DateTime.Now;
        ent.PQSequenceId = sequenceId;
        var offset = readContext.EncodedBuffer!.ReadCursor;
        //Console.Out.WriteLine($"{TimeContext.LocalTimeNow:O} Deserializing {sequenceId} with {length} bytes.");
        fixed (byte* fptr = readContext.EncodedBuffer.Buffer)
        {
            var msgHeader = fptr + offset;
            var msgSizePtr = msgHeader + PQQuoteMessageHeader.MessageSizeOffset;
            var msgSize = StreamByteOps.ToUInt(ref msgSizePtr);
            var end = msgHeader + msgSize;
            var version = *msgHeader;
            if (version < SupportFromVersion || version > SupportToVersion)
            {
                logger.Warn("Received unsupported message version {0} will skip processing", version);
                return;
            }

            var ptr = msgHeader + PQQuoteMessageHeader.HeaderSize;

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
                    , Command = (pqFieldUpdate.Flag & PQFieldFlags.IsUpdate) == PQFieldFlags.IsUpdate ?
                        CrudCommand.Update :
                        (pqFieldUpdate.Flag & PQFieldFlags.IsDelete) == PQFieldFlags.IsDelete ?
                            CrudCommand.None :
                            CrudCommand.Insert
                };
                var fieldStringUpdate = new PQFieldStringUpdate
                {
                    Field = pqFieldUpdate, StringUpdate = stringUpdate
                };
                ent.UpdateFieldString(fieldStringUpdate);
            }
        }
    }

    public void PushQuoteToSubscribers(PQSyncStatus syncStatus,
        IPerfLogger? detectionToPublishLatencyTraceLogger = null)
    {
        if (!Subscribers.Any()) return;
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
}
