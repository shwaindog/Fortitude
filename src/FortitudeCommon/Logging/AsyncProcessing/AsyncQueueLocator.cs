using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.AsyncProcessing;

public interface IAsyncQueueLocator
{
    void Execute(int queueNumber, Action job);

    void SendLogEntryTo(int queueNumber, IFLogEntry logEntry, IFLogAppender appender);

    void SendLogEntriesTo(int queueNumber, IReusableList<IFLogEntry> batchLogEntries, IFLogAppender appender);

    void FlushBufferToAppender(int queueNumber, IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender);

    IReadOnlyCollection<IFLogAsyncQueue> AsyncQueues { get; }

    int MaxAvailableQueues { get; }

    IFLogAsyncQueue GetOrCreateQueue(int queueNumber);

    int QueueBackLogSize(int queueNumber);

    void StartAsyncQueues();
}

public class AsyncQueueLocator(IMutableAsyncQueuesInitConfig asyncInitConfig) : IAsyncQueueLocator
{
    private static readonly object SyncLock = new();

    private readonly AsyncProcessingType defaultAsyncProcessing = asyncInitConfig.AsyncProcessingType;

    private readonly Dictionary<int, IFLogAsyncQueue> runningQueues = new();

    public IReadOnlyCollection<IFLogAsyncQueue> AsyncQueues => runningQueues.Values;

    public void StartAsyncQueues()
    {
    }

    public IFLogAsyncQueue GetOrCreateQueue(int queueNumber)
    {
        if (runningQueues.TryGetValue(queueNumber, out var foundExisting)) return foundExisting;
        var defaultQueueType = defaultAsyncProcessing;
        if (defaultQueueType == AsyncProcessingType.AllAsyncDisabled)
        {
            lock (SyncLock)
            {
                if (runningQueues.TryGetValue(queueNumber, out foundExisting)) return foundExisting;

                var syncExecuteQueue = FLogCreate.MakeSynchroniseQueue(queueNumber);
                runningQueues.TryAdd(queueNumber, syncExecuteQueue);
                return syncExecuteQueue;
            }
        }
        var queueConfig = GetQueueConfig((byte)queueNumber);
        var queueType   = queueConfig.QueueType;
        if (queueType == AsyncProcessingType.SingleBackgroundAsyncThread)
        {
            var singleBackgroundQueue = runningQueues.Values.FirstOrDefault(aq => aq.QueueType == AsyncProcessingType.SingleBackgroundAsyncThread);
            if (singleBackgroundQueue != null)
            {
                lock (SyncLock)
                {
                    if (runningQueues.TryGetValue(queueNumber, out foundExisting)) return foundExisting;

                    var proxySingleBackgroundQueue = FLogCreate.MakeProxyAsyncQueue(queueNumber, singleBackgroundQueue);
                    runningQueues.TryAdd(queueNumber, proxySingleBackgroundQueue);
                    return proxySingleBackgroundQueue;
                }
            }
        }
        if (queueType == AsyncProcessingType.Synchronise)
        {
            lock (SyncLock)
            {
                if (runningQueues.TryGetValue(queueNumber, out foundExisting)) return foundExisting;

                var syncExecuteQueue = FLogCreate.MakeSynchroniseQueue(queueNumber);
                runningQueues.TryAdd(queueNumber, syncExecuteQueue);
                return syncExecuteQueue;
            }
        }
        var modQueueNumber = (byte)(queueNumber % MaxAvailableQueues);
        foundExisting = runningQueues.Values.FirstOrDefault(aq => aq.QueueNumber == modQueueNumber && aq.QueueType == queueType);
        if (foundExisting != null) return foundExisting;
        lock (SyncLock)
        {
            if (runningQueues.TryGetValue(queueNumber, out foundExisting)) return foundExisting;
            var newQueue = FLogCreate.MakeAsyncQueue(queueConfig);
            if (newQueue != null)
            {
                runningQueues.Add(queueNumber, newQueue);
                return newQueue;
            }

            Console.Out.WriteLine($"Could not create queue {queueNumber} of type {queueNumber} returning Synchronise queue !");

            var syncExecuteQueue = FLogCreate.MakeSynchroniseQueue(queueNumber);
            runningQueues.TryAdd(queueNumber, syncExecuteQueue);
            return syncExecuteQueue;
        }
    }

    protected virtual IAsyncQueueConfig GetQueueConfig(byte queueNumber)
    {
        if (asyncInitConfig.AsyncQueues.TryGetValue(queueNumber, out IAsyncQueueConfig? explicitQueueConfig))
        {
            return explicitQueueConfig;
        }
        var defaultQueueType             = asyncInitConfig.AsyncProcessingType;
        var defaultQueueCapacity         = asyncInitConfig.DefaultBufferQueueSize;
        var defaultQueueFullHandling     = asyncInitConfig.DefaultFullQueueHandling;
        var defaultQueueFullDropInterval = asyncInitConfig.DefaultDropInterval;
        return new AsyncQueueConfig(queueNumber, defaultQueueType, defaultQueueCapacity, false, defaultQueueFullHandling
                                  , defaultQueueFullDropInterval);
    }

    public int MaxAvailableQueues { get; } = asyncInitConfig.MaxAsyncProcessing;

    public int QueueBackLogSize(int queueNumber) => GetOrCreateQueue(queueNumber).QueueBackLogSize;

    public void Execute(int queueNumber, Action job)
    {
        var queue = GetOrCreateQueue(queueNumber);
        queue.Execute(job);
    }

    public void FlushBufferToAppender(int queueNumber, IBufferedFormatWriter toFlush, IFLogAsyncTargetFlushBufferAppender fromAppender)
    {
        var queue = GetOrCreateQueue(queueNumber);
        queue.FlushBufferToAppender(toFlush, fromAppender);
    }

    public void SendLogEntriesTo(int queueNumber, IReusableList<IFLogEntry> batchLogEntries, IFLogAppender appender)
    {
        var queue = GetOrCreateQueue(queueNumber);
        queue.SendLogEntriesTo(batchLogEntries, appender);
    }

    public void SendLogEntryTo(int queueNumber, IFLogEntry logEntry, IFLogAppender appender)
    {
        var queue = GetOrCreateQueue(queueNumber);
        queue.SendLogEntryTo(logEntry, appender);
    }
}
