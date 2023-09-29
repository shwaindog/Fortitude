using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using FortitudeCommon.Chronometry;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Monitoring.Logging.Diagnostics.Performance;
using FortitudeIO.Protocols.Serialization;
using FortitudeIO.Transports.Sockets.Logging;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsApi.Pricing.Quotes.SourceTickerInfo;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Serialization.Deserialization
{
    public abstract class PQDeserializerBase : BinaryDeserializer<IPQLevel0Quote>, IPQDeserializer
    {
        public IUniqueSourceTickerIdentifier Identifier { get; }

        protected PQDeserializerBase(IUniqueSourceTickerIdentifier identifier)
        {
            Identifier = identifier;
        }

        public static IPQImplementationFactory ConcreteFinder { get; set; } = new PQImplementationFactory();
        public IPQDeserializer Previous { get; set; }
        public IPQDeserializer Next { get; set; }

        public event Action<IPQDeserializer> ReceivedUpdate;
        public event Action<IPQDeserializer> SyncOk;
        public event Action<IPQDeserializer> OutOfSync;

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

        public virtual bool HasTimedOutAndNeedsSnapshot(DateTime utcNow)
        {
            return false;
        }

        public virtual bool CheckResync(DateTime utcNow)
        {
            return false;
        }
    }

    public abstract class PQDeserializerBase<T> : PQDeserializerBase, IPQDeserializer<T>
        where T : class, IPQLevel0Quote
    {
        protected PQDeserializerBase(ISourceTickerQuoteInfo identifier) : base(identifier)
        {
            QuoteFactory = sqi => ConcreteFinder.GetConcreteMapping<T>(sqi);
            PublishedQuote = ConcreteFinder.GetConcreteMapping<T>(identifier);
        }

        protected readonly IList<IObserver<T>> Subscribers = new List<IObserver<T>>();
        public T PublishedQuote { get; protected set; }

        // ReSharper disable once StaticMemberInGenericType
        private static readonly IPerfLoggerPool PublishPQQuoteDeserializerLatencyTraceLoggerPool =
            PerfLoggingPoolFactory.Instance.GetLatencyTracingLoggerPool("clientCallback", 
                TimeSpan.FromMilliseconds(10), typeof(UserCallback));

        protected Func<ISourceTickerQuoteInfo, T> QuoteFactory;

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
        public unsafe void UpdateQuote(DispatchContext dispatchContext, T ent, uint sequenceId)
        {
            ent.ClientReceivedTime = dispatchContext.DetectTimestamp;
            ent.SocketReceivingTime = dispatchContext.ReceivingTimestamp;
            ent.ProcessedTime = dispatchContext.DeserializerTimestamp;
            ent.PQSequenceId = sequenceId;
            var offset = dispatchContext.EncodedBuffer.ReadCursor;
            //Console.Out.WriteLine($"{TimeContext.LocalTimeNow:O} Deserializing {sequenceId} with {length} bytes.");
            fixed (byte* fptr = dispatchContext.EncodedBuffer.Buffer)
            {
                var ptr = fptr + offset;
                var end = ptr + dispatchContext.MessageSize;
                while (ptr < end)
                {
                    var flags = *ptr++;
                    ushort id;
                    if ((flags & PQFieldFlags.IsExtendedFieldId) == 0)
                    {
                        id = *ptr++;
                    }
                    else
                    {
                        id = StreamByteOps.ToUShort(ref ptr);
                    }
                    var pqFieldUpdate = new PQFieldUpdate(id, StreamByteOps.ToUInt(ref ptr), flags);
                    var moreBytes = ent.UpdateField(pqFieldUpdate);
                    if (moreBytes <= 0 || ptr + moreBytes + 4 > end) continue;
                    var stringUpdate = new PQStringUpdate
                    {
                        DictionaryId = StreamByteOps.ToInt(ref ptr),
                        Value = StreamByteOps.ToString(ref ptr, moreBytes),
                        Command = (pqFieldUpdate.Flag & PQFieldFlags.IsUpdate) == PQFieldFlags.IsUpdate
                            ? CrudCommand.Update
                            : ((pqFieldUpdate.Flag & PQFieldFlags.IsDelete) == PQFieldFlags.IsDelete
                                ? CrudCommand.None
                                : CrudCommand.Insert)
                    };
                    var fieldStringUpdate = new PQFieldStringUpdate
                    {
                        Field = pqFieldUpdate,
                        StringUpdate = stringUpdate
                    };
                    ent.UpdateFieldString(fieldStringUpdate);
                }
            }
        }
        
        public void PushQuoteToSubscribers(PQSyncStatus syncStatus,
            IPerfLogger detectionToPublishLatencyTraceLogger = null)
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

        protected virtual bool ShouldPublish => PublishedQuote.HasUpdates;
    }
}