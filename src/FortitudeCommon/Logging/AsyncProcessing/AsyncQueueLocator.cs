using FortitudeCommon.Logging.AsyncProcessing.ProxyQueue;
using FortitudeCommon.Logging.Config.Initialization.AsyncQueues;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.AsyncProcessing;

public interface IAsyncQueueLocator
{
    IReadOnlyCollection<IFLogAsyncQueue> AsyncQueues { get; }

    IReadOnlyCollection<IFLogAsyncQueuePublisher> ClientPublisherQueues { get; }

    int MaxAvailableQueues { get; }

    IFLogAsyncQueue GetOrCreateQueue(int queueNumber);

    IFLogAsyncQueuePublisher GetClientPublisherQueue(int queueNumber);

    int QueueBackLogSize(int queueNumber);

    void StartAsyncQueues();
}

public class AsyncQueueLocator(IMutableAsyncQueuesInitConfig asyncInitConfig) : IAsyncQueueLocator
{
    private static readonly object SyncLock = new();

    private readonly AsyncProcessingType defaultAsyncProcessing = asyncInitConfig.AsyncProcessingType;

    private readonly Dictionary<int, IFLogAsyncQueue>                 runningQueues       = new();
    private readonly Dictionary<int, IFLogAsyncSwitchableQueueClient> clientProxiesQueues = new();

    public IReadOnlyCollection<IFLogAsyncQueue>          AsyncQueues           => runningQueues.Values;

    public IReadOnlyCollection<IFLogAsyncQueuePublisher> ClientPublisherQueues => clientProxiesQueues.Values;

    public void StartAsyncQueues()
    {
        foreach (var asyncQueueConfig in asyncInitConfig.AsyncQueues)
        {
            if (asyncQueueConfig.Value.LaunchAtFlogStart)
            {
                var queue = GetOrCreateQueue(asyncQueueConfig.Key);
                queue.StartQueueReceiver();
            }
        }
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
        var modQueueNumber = (byte)(queueNumber % Math.Max(MaxAvailableQueues, 1));
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

    public IFLogAsyncQueuePublisher GetClientPublisherQueue(int queueNumber)
    {
        if (clientProxiesQueues.TryGetValue(queueNumber, out var foundExisting)) return foundExisting;
        lock (SyncLock)
        {
            if (clientProxiesQueues.TryGetValue(queueNumber, out foundExisting)) return foundExisting;

            var actualQueue = GetOrCreateQueue(queueNumber);

            var proxyQueue = new FLogAsyncSwitchableProxyQueue(queueNumber, actualQueue);
            clientProxiesQueues.Add(queueNumber, proxyQueue);
            return proxyQueue;
        }
    }

    public int MaxAvailableQueues { get; } = asyncInitConfig.MaxAsyncProcessing;

    public int QueueBackLogSize(int queueNumber) => GetOrCreateQueue(queueNumber).QueueBackLogSize;

    public void Execute(int queueNumber, Action job)
    {
        var queue = GetOrCreateQueue(queueNumber);
        queue.Execute(job);
    }

    public void FlushBufferToAppender(int queueNumber, IBufferedFormatWriter toFlush, IFLogBufferingFormatAppender fromAppender)
    {
        var queue = GetOrCreateQueue(queueNumber);
        queue.FlushBufferToAppender(toFlush, fromAppender);
    }

    public void SendLogEntryEventTo(int queueNumber, LogEntryPublishEvent logEntryEvent, IReadOnlyList<IFLogEntrySink> logEntrySinks, IFLogEntrySource publishSource)
    {
        var queue = GetOrCreateQueue(queueNumber);
        queue.SendLogEntryEventTo(logEntryEvent, logEntrySinks, publishSource);
    }

    public void SendLogEntryEventTo(int queueNumber, LogEntryPublishEvent logEntryEvent, IFLogEntrySink logEntrySink, IFLogEntrySource publishSource)
    {
        var queue = GetOrCreateQueue(queueNumber);
        queue.SendLogEntryEventTo(logEntryEvent, logEntrySink,publishSource);
    }
}
