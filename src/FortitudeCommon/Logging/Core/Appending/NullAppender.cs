using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.Appending.Forwarding;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;
using RecyclableObject = FortitudeCommon.DataStructures.Memory.RecyclableObject;

namespace FortitudeCommon.Logging.Core.Appending;

public class NullAppender(INullAppenderConfig appenderDefinitionConfig)
    : FLogEntrySinkBase, IMutableFLogAppender
{
    public const string NullAppenderName = $"{nameof(NullAppender)}";

    public static NullAppender NullInstance = new(new NullAppenderConfig());

    private uint totalLogEntriesReceived;
    private uint totalLogEntriesProcessed;
    private uint totalLogEntriesDropped;

    private object? notificationSyncLock;

    private ReusableList<NotifyWhenEntriesCountReaches>? registeredCountNotifications;
    
    public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.Sink;

    public override FLogEntryProcessChainState LogEntryProcessState
    {
        get => FLogEntryProcessChainState.Terminating;
        protected set => _ = value;
    }

    public override string Name
    {
        get => NullAppenderName;
        protected set => _ = value;
    }

    public string AppenderName
    {
        get => NullAppenderName;
        set => _ = value;
    }

    public string AppenderType => $"{nameof(FLoggerBuiltinAppenderType.Null)}";

    public IAppenderAsyncClient AsyncClient
    {
        get => NullAsyncClient.NullAsyncClientInstance;
        set => _ = value;
    }

    public int ReceiveOnAsyncQueueNumber => 0;

    public uint TotalLogEntriesReceived => totalLogEntriesReceived;
    public uint TotalLogEntriesProcessed => totalLogEntriesProcessed;
    public uint TotalLogEntriesDropped => totalLogEntriesDropped;
    public uint ContextInstanceNumber => 0;
    

    public IAppenderClient CreateAppenderClientFor(IFLoggerCommon logger) => NullAppenderClient.NullClientInstance;

    public IAppenderClient CreateAppenderClientFor(IFLogForwardingAppender appender) => NullAppenderClient.NullClientInstance;

    public void ExecuteJob(Action job) { }

    public IAppenderDefinitionConfig GetAppenderConfig() => appenderDefinitionConfig;

    public void HandleConfigUpdate(IAppenderDefinitionConfig newLoggerState) { }

    public IFLogEntryPipelineEndpoint ReceiveEndpoint => NullAppenderClient.NullClientInstance;

    public void ReceiveOldAppenderTypeClients(List<IMutableAppenderClient> issuedAppenders) { }

    public override void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        var countEntries = logEntryEvent.EntriesCount();
        IncrementLogEntriesReceived(countEntries);
        IncrementLogEntriesProcessed(countEntries);
        IncrementLogEntriesDropped(countEntries);
        logEntryEvent.DecrementRefCount();
    }

    public void RunJobOnAppenderQueue(Action job) { }


    protected void IncrementLogEntriesReceived(uint byAmount = 1)
    {
        var totalAmount = byAmount == 1 ? Interlocked.Increment(ref totalLogEntriesReceived) : Interlocked.Add(ref totalLogEntriesReceived, byAmount);
        CheckRegisteredCountListeners(LogEntriesCountType.Received, totalAmount);
    }

    protected void IncrementLogEntriesProcessed(uint byAmount = 1)
    {
        var totalAmount = byAmount == 1
            ? Interlocked.Increment(ref totalLogEntriesProcessed)
            : Interlocked.Add(ref totalLogEntriesProcessed, byAmount);
        CheckRegisteredCountListeners(LogEntriesCountType.Processed, totalAmount);
    }

    public void IncrementLogEntriesDropped(uint byAmount = 1)
    {
        var totalAmount = byAmount == 1 ? Interlocked.Increment(ref totalLogEntriesDropped) : Interlocked.Add(ref totalLogEntriesDropped, byAmount);
        CheckRegisteredCountListeners(LogEntriesCountType.Dropped, totalAmount);
    }

    public void RegisterCallbackWhenReceivedCount(uint reaches, Action<uint, IFLogAppender> callback)
    {
        if (totalLogEntriesReceived > reaches)
        {
            callback.Invoke(totalLogEntriesReceived, this);
            return;
        }
        RegisterCallbackWhenCountType(LogEntriesCountType.Received, reaches, callback);
    }

    public void RegisterCallbackWhenProcessedCount(uint reaches, Action<uint, IFLogAppender> callback)
    {
        if (totalLogEntriesProcessed > reaches)
        {
            callback.Invoke(totalLogEntriesProcessed, this);
            return;
        }
        RegisterCallbackWhenCountType(LogEntriesCountType.Processed, reaches, callback);
    }

    public void RegisterCallbackWhenDroppedCount(uint reaches, Action<uint, IFLogAppender> callback)
    {
        if (totalLogEntriesDropped > reaches)
        {
            callback.Invoke(totalLogEntriesDropped, this);
            return;
        }
        RegisterCallbackWhenCountType(LogEntriesCountType.Dropped, reaches, callback);
    }
    
    private void RegisterCallbackWhenCountType(LogEntriesCountType countType, uint reaches, Action<uint, IFLogAppender> callback)
    {
        if (notificationSyncLock == null)
        {
            lock (this)
            {
                notificationSyncLock ??= new object();
            }
        }
        
        var notifyOnThreshold =
            DataStructures.Memory.Recycler.ThreadStaticRecycler
                          .Borrow<NotifyWhenEntriesCountReaches>()
                          .Initialize(countType, reaches, callback, this);
        lock (notificationSyncLock)
        {
            registeredCountNotifications ??= DataStructures.Memory.Recycler.ThreadStaticRecycler.Borrow<ReusableList<NotifyWhenEntriesCountReaches>>();
            registeredCountNotifications.Add(notifyOnThreshold);
        }
    }

    public void UnregisterCallbackWhenReceivedCount(Action<uint, IFLogAppender> callback)
    {
        UnregisterCallbackWhenCountType(LogEntriesCountType.Received, callback);
    }

    public void UnregisterCallbackWhenProcessedCount(Action<uint, IFLogAppender> callback)
    {
        UnregisterCallbackWhenCountType(LogEntriesCountType.Processed, callback);
    }

    public void UnregisterCallbackWhenDroppedCount(Action<uint, IFLogAppender> callback)
    {
        UnregisterCallbackWhenCountType(LogEntriesCountType.Dropped, callback);
    }
    
    private void UnregisterCallbackWhenCountType(LogEntriesCountType countType, Action<uint, IFLogAppender> callback)
    {
        if (notificationSyncLock == null) return;
        lock (notificationSyncLock)
        {
            ReusableList<NotifyWhenEntriesCountReaches>? countNotifications = registeredCountNotifications;
            countNotifications?.IncrementRefCount();
            if(countNotifications == null) return;
            for (var i = 0; i < countNotifications.Count; i++)
            {
                var notification = countNotifications[i];
                if (countType == notification.CountType && callback == notification.NotifyCallback)
                {
                    countNotifications.RemoveAt(i);
                    i--;
                }
            }
            countNotifications.DecrementRefCount();
            if (registeredCountNotifications is { Count: 0 })
            {
                notificationSyncLock = null;
                registeredCountNotifications.DecrementRefCount();
                registeredCountNotifications = null;
            }
        }
    }

    private void CheckRegisteredCountListeners(LogEntriesCountType logEntriesCountType, uint currentValue)
    {
        if (notificationSyncLock == null) return;
        ReusableList<NotifyWhenEntriesCountReaches>? countNotifications;
        lock (notificationSyncLock)
        {
            countNotifications = registeredCountNotifications;
            countNotifications?.IncrementRefCount();
        }
        if (countNotifications == null) return;

        for (var i = 0; i < countNotifications.Count; i++)
        {
            var notification = countNotifications[i];

            var updatedNotificationCount = notification.CheckAndNotify(logEntriesCountType, currentValue);
            if (updatedNotificationCount != null)
            {
                countNotifications.DecrementRefCount();
                countNotifications = updatedNotificationCount;
                countNotifications.IncrementRefCount();
                registeredCountNotifications = updatedNotificationCount;
                i--;
            }
        }
        countNotifications.DecrementRefCount();

        lock (notificationSyncLock ?? this)
        {
            if (registeredCountNotifications is { Count: 0 })
            {
                notificationSyncLock = null;
                registeredCountNotifications.DecrementRefCount();
                registeredCountNotifications = null;
            }
        }
    }
    
    private enum LogEntriesCountType : byte
    {
        Received
      , Processed
      , Dropped
    }

    private class NotifyWhenEntriesCountReaches : RecyclableObject
    {
        private uint notifyThreshold;

        public Action<uint, IFLogAppender>? NotifyCallback;

        private NullAppender? countingAppender;

        public NotifyWhenEntriesCountReaches Initialize(LogEntriesCountType countType, uint threshold, Action<uint, IFLogAppender> callback
          , NullAppender appender)
        {
            CountType       = countType;
            notifyThreshold = threshold;
            NotifyCallback  = callback;

            countingAppender = appender;

            return this;
        }

        public ReusableList<NotifyWhenEntriesCountReaches>? CheckAndNotify(LogEntriesCountType countType, uint currentValue)
        {
            if (countType != CountType) return null;
            if (currentValue >= notifyThreshold)
            {
                ReusableList<NotifyWhenEntriesCountReaches> newNotifications;
                lock (countingAppender!.notificationSyncLock!)
                {
                    if (!countingAppender.registeredCountNotifications?.Contains(this) ?? false) return null;
                    NotifyCallback?.Invoke(currentValue, countingAppender!);
                    var oldList = countingAppender.registeredCountNotifications;
                    if (oldList is not { Count: > 1 }) return null;
                    newNotifications = Recycler!.Borrow<ReusableList<NotifyWhenEntriesCountReaches>>();
                    foreach (var existing in oldList)
                    {
                        if (ReferenceEquals(existing, this))
                        {
                            newNotifications.Add(existing);
                        }
                    }
                }
                return newNotifications;
            }
            return null;
        }

        public LogEntriesCountType CountType { get; private set; }

        public override void StateReset()
        {
            notifyThreshold  = 0;
            NotifyCallback   = null!;
            countingAppender = null!;
            base.StateReset();
        }
    }
}
