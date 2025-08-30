// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.AsyncProcessing;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

#endregion

namespace FortitudeCommon.Logging.Core.Appending;

public interface IFLogAppender
{
    string AppenderName { get; }
    string AppenderType { get; }

    uint TotalLogEntriesReceived { get; }
    uint TotalLogEntriesProcessed { get; }
    uint TotalLogEntriesDropped { get; }

    uint ContextInstanceNumber { get; }

    int ReceiveOnAsyncQueueNumber { get; }

    IFLogEntryPipelineEndpoint ReceiveEndpoint { get; }

    IAppenderClient CreateAppenderClientFor(IFLoggerCommon logger);

    IAppenderClient CreateAppenderClientFor(IFLogForwardingAppender appender);

    void RunJobOnAppenderQueue(Action job);

    void ExecuteJob(Action job);

    IAppenderDefinitionConfig GetAppenderConfig();

    void RegisterCallbackWhenReceivedCount(uint reaches, Action<uint, IFLogAppender> callback);
    void RegisterCallbackWhenProcessedCount(uint reaches, Action<uint, IFLogAppender> callback);
    void RegisterCallbackWhenDroppedCount(uint reaches, Action<uint, IFLogAppender> callback);
    void UnregisterCallbackWhenReceivedCount(Action<uint, IFLogAppender> callback);
    void UnregisterCallbackWhenProcessedCount(Action<uint, IFLogAppender> callback);
    void UnregisterCallbackWhenDroppedCount(Action<uint, IFLogAppender> callback);
}

public interface IMutableFLogAppender : IFLogAppender
{
    new string AppenderName { get; set; }

    IAppenderAsyncClient AsyncClient { get; set; }

    void HandleConfigUpdate(IAppenderDefinitionConfig newLoggerState);

    void ReceiveOldAppenderTypeClients(List<IMutableAppenderClient> issuedAppenders);
}

public abstract class FLogAppender : FLogEntrySinkBase, IMutableFLogAppender
{
    protected IAppenderDefinitionConfig AppenderConfig;

    protected List<IMutableAppenderClient> IssuedAppenderClients = [];


    private object? notificationSyncLock;

    private ReusableList<NotifyWhenEntriesCountReaches>? registeredCountNotifications;

    private uint totalLogEntriesDropped;

    private uint totalLogEntriesProcessed;

    private uint totalLogEntriesReceived;

    protected FLogAppender(IAppenderDefinitionConfig appenderDefinitionConfig, IFLogContext context)
    {
        ContextInstanceNumber = context.ContextInstanceNumber;

        AppenderConfig = appenderDefinitionConfig;
        AppenderName   = appenderDefinitionConfig.AppenderName;
        AppenderType   = appenderDefinitionConfig.AppenderType;

        AsyncClient = CreateAppenderAsyncClient(appenderDefinitionConfig, context.AsyncRegistry);

        ReceiveEndpoint = new FLogEntryPipelineEndpoint(AppenderName, this);

        context.AppenderRegistry.RegisterAppenderCallback(this);
    }

    public override string Name
    {
        get => AppenderName;
        protected set => AppenderName = value;
    }

    public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.Sink;

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }
        = FLogEntryProcessChainState.Terminating;

    public string AppenderName { get; set; }

    public uint TotalLogEntriesReceived => totalLogEntriesReceived;
    public uint TotalLogEntriesProcessed => totalLogEntriesProcessed;
    public uint TotalLogEntriesDropped => totalLogEntriesDropped;

    public uint ContextInstanceNumber { get; }

    public string AppenderType { get; protected init; }

    public IAppenderAsyncClient AsyncClient { get; set; }

    public void ExecuteJob(Action job)
    {
        job();
    }

    public int ReceiveOnAsyncQueueNumber => AsyncClient.AppenderReceiveQueueNum;

    public virtual void HandleConfigUpdate(IAppenderDefinitionConfig newAppenderConfig)
    {
        AppenderConfig = newAppenderConfig;

        AsyncClient.AppenderReceiveQueueNum = newAppenderConfig.RunOnAsyncQueueNumber;
    }

    public virtual IAppenderClient CreateAppenderClientFor(IFLoggerCommon logger)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        var alreadyIssued =
            IssuedAppenderClients
                .FirstOrDefault(ac => ac is LoggerAppenderClient loggerAppender
                                   && loggerAppender.IssuedTo.FullName == logger.FullName);
        if (alreadyIssued != null) return alreadyIssued;
        lock (IssuedAppenderClients)
        {
            alreadyIssued =
                IssuedAppenderClients
                    .FirstOrDefault(ac => ac is LoggerAppenderClient loggerAppender
                                       && loggerAppender.IssuedTo.FullName == logger.FullName);
            if (alreadyIssued != null) return alreadyIssued;

            var newLoggerClient = new LoggerAppenderClient(this, logger);
            IssuedAppenderClients.Add(newLoggerClient);
            return newLoggerClient;
        }
    }

    public virtual IAppenderClient CreateAppenderClientFor(IFLogForwardingAppender appender)
    {
        // ReSharper disable once InconsistentlySynchronizedField
        var alreadyIssued =
            IssuedAppenderClients
                .FirstOrDefault(ac => ac is ForwardingAppenderClient loggerAppender
                                   && loggerAppender.IssuedTo.AppenderName == appender.AppenderName);
        if (alreadyIssued != null) return alreadyIssued;
        lock (IssuedAppenderClients)
        {
            alreadyIssued =
                IssuedAppenderClients
                    .FirstOrDefault(ac => ac is ForwardingAppenderClient loggerAppender
                                       && loggerAppender.IssuedTo.AppenderName == appender.AppenderName);
            if (alreadyIssued != null) return alreadyIssued;

            var newLoggerClient = new ForwardingAppenderClient(this, appender);
            IssuedAppenderClients.Add(newLoggerClient);
            return newLoggerClient;
        }
    }

    public virtual void ReceiveOldAppenderTypeClients(List<IMutableAppenderClient> issuedAppenders)
    {
        IssuedAppenderClients.AddRange(issuedAppenders);
        foreach (var mutableAppenderClient in issuedAppenders) mutableAppenderClient.BackingAppender = this;
    }

    public IFLogEntryPipelineEndpoint ReceiveEndpoint { get; }

    public void RunJobOnAppenderQueue(Action job)
    {
        AsyncClient.RunJobOnAppenderQueue(job);
    }

    public abstract IAppenderDefinitionConfig GetAppenderConfig();

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

    protected virtual IAppenderAsyncClient CreateAppenderAsyncClient
        (IAppenderDefinitionConfig appenderDefinitionConfig, IFLoggerAsyncRegistry asyncRegistry)
    {
        var processAsync = appenderDefinitionConfig.RunOnAsyncQueueNumber;

        var appenderAsyncClient = new ReceiveAsyncClient(this, processAsync, asyncRegistry);
        return appenderAsyncClient;
    }

    public abstract void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent);

    public override void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        IncrementLogEntriesReceived(logEntryEvent.EntriesCount());
        if (ReceiveOnAsyncQueueNumber == 0 || ReceiveOnAsyncQueueNumber == FLogAsyncQueue.MyCallingQueueNumber)
        {
            ProcessReceivedLogEntryEvent(logEntryEvent);
            logEntryEvent.DecrementRefCount();
            return;
        }
        AsyncClient.ReceiveLogEntryEventOnConfiguredQueue(logEntryEvent, ReceiveEndpoint);
        logEntryEvent.DecrementRefCount();
    }

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

    private void RegisterCallbackWhenCountType(LogEntriesCountType countType, uint reaches, Action<uint, IFLogAppender> callback)
    {
        if (notificationSyncLock == null)
            lock (this)
            {
                notificationSyncLock ??= new object();
            }

        var notifyOnThreshold =
            DataStructures.Memory.Recycler.ThreadStaticRecycler
                          .Borrow<NotifyWhenEntriesCountReaches>()
                          .Initialize(countType, reaches, callback, this);
        lock (notificationSyncLock)
        {
            registeredCountNotifications
                ??= DataStructures.Memory.Recycler.ThreadStaticRecycler.Borrow<ReusableList<NotifyWhenEntriesCountReaches>>();
            registeredCountNotifications.Add(notifyOnThreshold);
        }
    }

    private void UnregisterCallbackWhenCountType(LogEntriesCountType countType, Action<uint, IFLogAppender> callback)
    {
        if (notificationSyncLock == null) return;
        lock (notificationSyncLock)
        {
            var countNotifications = registeredCountNotifications;
            countNotifications?.IncrementRefCount();
            if (countNotifications == null) return;
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
        private FLogAppender? countingAppender;

        public  Action<uint, IFLogAppender>? NotifyCallback;
        private uint                         notifyThreshold;

        public LogEntriesCountType CountType { get; private set; }

        public NotifyWhenEntriesCountReaches Initialize(LogEntriesCountType countType, uint threshold, Action<uint, IFLogAppender> callback
          , FLogAppender appender)
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
                        if (ReferenceEquals(existing, this))
                            newNotifications.Add(existing);
                }
                return newNotifications;
            }
            return null;
        }

        public override void StateReset()
        {
            notifyThreshold  = 0;
            NotifyCallback   = null!;
            countingAppender = null!;
            base.StateReset();
        }
    }
}
