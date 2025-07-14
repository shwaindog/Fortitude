// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

#region

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.Appending.Formatting;
using FortitudeCommon.Logging.Core.LogEntries;

#endregion

namespace FortitudeCommon.Logging.Core.Appending;

public interface IFLoggerAppender : IFLogEntryBatchForwarder
{
    string AppenderName { get; }

    string? AppenderConfigFilePath { get; }

    int RunOnAsyncQueueNumber { get; }

    FloggerAppenderType AppenderType { get; }

    IFLogEntryFormatter Formatter { get; }
}

public interface IMutableFLoggerAppender : IFLoggerAppender
{
    new string AppenderName { get; set; }

    new string? AppenderConfigFilePath { get; set; }

    new int RunOnAsyncQueueNumber { get; set; }

    new FloggerAppenderType AppenderType { get; }

    new IFLogEntryFormatter Formatter { get; set; }

    void HandleConfigUpdate(IAppenderDefinitionConfig newLoggerState);
}

public abstract class FLoggerAppender : RecyclableObject, IMutableFLoggerAppender
{
    protected IAppenderDefinitionConfig AppenderConfig;

    protected FLoggerAppender(IAppenderDefinitionConfig appenderDefinitionConfig)
    {
        AppenderConfig = appenderDefinitionConfig;
        AppenderName   = appenderDefinitionConfig.AppenderName;

        AppenderConfigFilePath = appenderDefinitionConfig.AppenderConfigRef;
        RunOnAsyncQueueNumber  = appenderDefinitionConfig.RunOnAsyncQueueNumber;

        ForwardToCallback            = ForwardLogEntryTo;
        AppenderBatchForwardCallBack = BatchForwardTo;
    }

    public string AppenderName { get; set; }

    public string? AppenderConfigFilePath { get; set; }

    public FloggerAppenderType AppenderType { get; protected set; }

    public IFLogEntryFormatter Formatter { get; set; } = FLogEntryFormatter.NewDefaultInstance;

    public virtual void ForwardLogEntryTo(IFLogEntry logEntry)
    {
        logEntry.IncrementRefCount();
        Append(logEntry);
    }

    public ForwardLogEntry ForwardToCallback { get; }

    public BatchForwardLogEntry AppenderBatchForwardCallBack { get; }

    public virtual void BatchForwardTo(IReusableList<IFLogEntry> batchLogEntries)
    {
        batchLogEntries.IncrementRefCount();
        foreach (var logEntry in batchLogEntries) ForwardLogEntryTo(logEntry);
        batchLogEntries.DecrementRefCount();
    }

    public int RunOnAsyncQueueNumber { get; set; }

    public virtual void HandleConfigUpdate(IAppenderDefinitionConfig newAppenderConfig)
    {
        AppenderConfig         = newAppenderConfig;
        AppenderConfigFilePath = newAppenderConfig.AppenderConfigRef;
        RunOnAsyncQueueNumber  = newAppenderConfig.RunOnAsyncQueueNumber;
    }

    protected void HandleMoveAppenderQueue(int from, int to)
    {
    }

    protected abstract void Append(IFLogEntry logEntry);
}
