// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting.LogEntryLayout;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

#endregion

namespace FortitudeCommon.Logging.Core.Appending;

public interface IFLogAppender : IFLogEntryBatchSink
{
    string AppenderName { get; }

    string? AppenderConfigFilePath { get; }

    int RunOnAsyncQueueNumber { get; }

    string AppenderType { get; }


    void Append(IReusableList<IFLogEntry> batchLogEntries);
    void Append(IFLogEntry flogEntry);
    
    IAppenderDefinitionConfig GetAppenderConfig();
}

public interface IMutableFLogAppender : IFLogAppender
{
    new string AppenderName { get; set; }

    new string? AppenderConfigFilePath { get; set; }

    void HandleConfigUpdate(IAppenderDefinitionConfig newLoggerState);
}

public interface IFLogAsyncTargetReceiveQueueAppender : IMutableFLogAppender
{
    IAppenderAsyncClient AsyncClient { get; set; }

    void ProcessReceivedLogEntry(IFLogEntry logEntry);

    void ProcessReceiveBatchLogEntries(IReusableList<IFLogEntry> batchLogEntries);
}

public abstract class FLogAppender : RecyclableObject, IFLogAsyncTargetReceiveQueueAppender
{
    protected IAppenderDefinitionConfig AppenderConfig;

    protected FLogAppender(IAppenderDefinitionConfig appenderDefinitionConfig, IFLogContext context)
    {
        AppenderConfig = appenderDefinitionConfig;
        AppenderName   = appenderDefinitionConfig.AppenderName;
        AppenderType   = appenderDefinitionConfig.AppenderType;

        AsyncClient = CreateAppenderAsyncClient(appenderDefinitionConfig, context.AsyncRegistry);

        ForwardToCallback            = ForwardLogEntryTo;
        AppenderBatchForwardCallBack = BatchForwardTo;
        
        context.AppenderRegistry.RegisterAppenderCallback(this);
    }

    protected virtual IAppenderAsyncClient CreateAppenderAsyncClient
        (IAppenderDefinitionConfig appenderDefinitionConfig, IFLoggerAsyncRegistry asyncRegistry)
    {
        var processAsync = appenderDefinitionConfig.RunOnAsyncQueueNumber;

        var appenderAsyncClient = new AppenderAsyncClient(this, processAsync, asyncRegistry);
        return appenderAsyncClient;
    }

    public string AppenderName { get; set; }

    public string? AppenderConfigFilePath { get; set; }

    public string AppenderType { get; protected set; }

    public IAppenderAsyncClient AsyncClient { get; set; }

    public virtual void ForwardLogEntryTo(IFLogEntry logEntry)
    {
        logEntry.IncrementRefCount();
        AsyncClient.ProcessLogEntryWithValidAsyncSettings(logEntry);
    }

    public void ProcessReceivedLogEntry(IFLogEntry logEntry)
    {
        Append(logEntry);
    }

    public ForwardLogEntry ForwardToCallback { get; }

    public BatchForwardLogEntry AppenderBatchForwardCallBack { get; }

    public virtual void BatchForwardTo(IReusableList<IFLogEntry> batchLogEntries)
    {
        batchLogEntries.IncrementRefCount();
        AsyncClient.ProcessBatchLogEntriesWithValidAsyncSettings(batchLogEntries);
    }

    public virtual void ProcessReceiveBatchLogEntries(IReusableList<IFLogEntry> batchLogEntries)
    {
        foreach (var logEntry in batchLogEntries) ForwardLogEntryTo(logEntry);
        batchLogEntries.DecrementRefCount();
    }

    public int RunOnAsyncQueueNumber => AsyncClient.AppenderReceiveQueueNum;

    public virtual void HandleConfigUpdate(IAppenderDefinitionConfig newAppenderConfig)
    {
        AppenderConfig = newAppenderConfig;

        AsyncClient.AppenderReceiveQueueNum = newAppenderConfig.RunOnAsyncQueueNumber;
    }

    protected void HandleMoveAppenderQueue(int from, int to) { }
    
    public abstract void Append(IReusableList<IFLogEntry> batchFLogEntries);

    public abstract void Append(IFLogEntry logEntry);

    public abstract IAppenderDefinitionConfig GetAppenderConfig();
}
