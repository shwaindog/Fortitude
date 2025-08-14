// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

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

    int ReceiveOnAsyncQueueNumber { get; }

    IFLogEntryPipelineEndpoint ReceiveEndpoint { get; }

    IAppenderClient CreateAppenderClientFor(IFLoggerCommon logger);

    IAppenderClient CreateAppenderClientFor(IFLogForwardingAppender appender);

    void RunJobOnAppenderQueue(Action job);

    void ExecuteJob(Action job);

    IAppenderDefinitionConfig GetAppenderConfig();
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

    protected List<IMutableAppenderClient> IssuedAppenderClients = new();

    protected FLogAppender(IAppenderDefinitionConfig appenderDefinitionConfig, IFLogContext context)
    {
        AppenderConfig = appenderDefinitionConfig;
        AppenderName   = appenderDefinitionConfig.AppenderName;
        AppenderType   = appenderDefinitionConfig.AppenderType;

        AsyncClient = CreateAppenderAsyncClient(appenderDefinitionConfig, context.AsyncRegistry);

        ReceiveEndpoint = new FLogEntryPipelineEndpoint(AppenderName, this);

        context.AppenderRegistry.RegisterAppenderCallback(this);
    }

    protected virtual IAppenderAsyncClient CreateAppenderAsyncClient
        (IAppenderDefinitionConfig appenderDefinitionConfig, IFLoggerAsyncRegistry asyncRegistry)
    {
        var processAsync = appenderDefinitionConfig.RunOnAsyncQueueNumber;

        var appenderAsyncClient = new ReceiveAsyncClient(this, processAsync, asyncRegistry);
        return appenderAsyncClient;
    }

    public string AppenderName { get; set; }

    public override string Name
    {
        get => AppenderName;
        protected set => AppenderName = value;
    }

    public override FLogEntrySourceSinkType LogEntryLinkType => FLogEntrySourceSinkType.Sink;

    public override FLogEntryProcessChainState LogEntryProcessState { get; protected set; }
        = FLogEntryProcessChainState.Terminating;

    public string AppenderType { get; protected set; }

    public IAppenderAsyncClient AsyncClient { get; set; }

    public abstract void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent);

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
        foreach (var mutableAppenderClient in issuedAppenders)
        {
            mutableAppenderClient.BackingAppender = this;
        }
    }

    public override void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher)
    {
        if (ReceiveOnAsyncQueueNumber == 0 || ReceiveOnAsyncQueueNumber == FLogAsyncQueue.MyCallingQueueNumber)
        {
            ProcessReceivedLogEntryEvent(logEntryEvent);
            logEntryEvent.LogEntry?.DecrementRefCount();
            logEntryEvent.LogEntriesBatch?.DecrementRefCount();
            return;
        }
        AsyncClient.ReceiveLogEntryEventOnConfiguredQueue(logEntryEvent, ReceiveEndpoint);
        logEntryEvent.LogEntry?.DecrementRefCount();
        logEntryEvent.LogEntriesBatch?.DecrementRefCount();
    }

    public IFLogEntryPipelineEndpoint ReceiveEndpoint { get; }

    public void RunJobOnAppenderQueue(Action job)
    {
        AsyncClient.RunJobOnAppenderQueue(job);
    }

    public abstract IAppenderDefinitionConfig GetAppenderConfig();
}
