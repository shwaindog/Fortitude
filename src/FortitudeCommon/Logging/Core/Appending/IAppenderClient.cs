using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.Appending.Forwarding;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending;

// public interface IFLogEntrySink
// {
//     void Append(IFLogEntry flogEntry);
// }
//
// public interface IBulkFLogEntrySink : IFLogEntrySink
// {
//     void Append(IReusableList<IFLogEntry> batchLogEntries);
// }
//
// public interface IFLogAsyncTargetReceiveQueueAppender : IBulkFLogEntrySink
// {
//     void ProcessReceivedLogEntry(IFLogEntry logEntry);
//
//     void ProcessReceiveBatchLogEntries(IReusableList<IFLogEntry> batchLogEntries);
//
//     void RunJobOnAppenderQueue(Action job);
//
//     void ExecuteJob(Action job);
// }

public interface IFLogAsyncTargetReceiveQueueAppender
{
    IFLogEntryPipelineEndpoint ReceiveEndpoint { get; }

    void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent);

    void RunJobOnAppenderQueue(Action job);

    void ExecuteJob(Action job);
}

public interface IAppenderClient : IFLogAsyncTargetReceiveQueueAppender
{
    string AppenderName { get; }
    string AppenderType { get; }

    int ReceiveOnAsyncQueueNumber { get; }

    IAppenderDefinitionConfig GetAppenderConfig();
}

public interface IMutableAppenderClient : IAppenderClient
{
    IFLogAppender BackingAppender { get; set; }
}

public class AppenderClient(IFLogAppender originalAppender) : IMutableAppenderClient
{
    public IFLogAppender BackingAppender { get; set; } = originalAppender;

    public string AppenderName => BackingAppender.AppenderName;
    public string AppenderType => BackingAppender.AppenderType;

    public int ReceiveOnAsyncQueueNumber => BackingAppender.ReceiveOnAsyncQueueNumber;

    public void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent)
    {
        BackingAppender.ProcessReceivedLogEntryEvent(logEntryEvent);
    }

    public IFLogEntryPipelineEndpoint ReceiveEndpoint => BackingAppender.ReceiveEndpoint;

    public IAppenderDefinitionConfig GetAppenderConfig() => BackingAppender.GetAppenderConfig();

    public void RunJobOnAppenderQueue(Action job)
    {
        BackingAppender.RunJobOnAppenderQueue(job);
    }

    public void ExecuteJob(Action job)
    {
        BackingAppender.ExecuteJob(job);
    }
}

public class NullAppenderClient : IMutableAppenderClient
{
    public static readonly NullAppenderClient NullClientInstance = new();

    public IFLogAppender BackingAppender { get; set; } = NullAppender.NullInstance;

    public string AppenderName => INullAppenderConfig.NullAppenderName;
    public string AppenderType => INullAppenderConfig.NullAppenderType;

    public int ReceiveOnAsyncQueueNumber => 0;

    public IAppenderDefinitionConfig GetAppenderConfig() => BackingAppender.GetAppenderConfig();

    public IFLogEntryPipelineEndpoint ReceiveEndpoint => BackingAppender.ReceiveEndpoint;

    public void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent)
    {
        BackingAppender.ProcessReceivedLogEntryEvent(logEntryEvent);
    }

    public void RunJobOnAppenderQueue(Action job) { }

    public void ExecuteJob(Action job) { }
}

public class LoggerAppenderClient(IFLogAppender originalAppender, IFLoggerCommon issuedTo)
    : AppenderClient(originalAppender)
{
    public IFLoggerCommon IssuedTo { get; } = issuedTo;
}

public class ForwardingAppenderClient(IFLogAppender originalAppender, IFLogForwardingAppender issuedTo)
    : AppenderClient(originalAppender)
{
    public IFLogForwardingAppender IssuedTo { get; } = issuedTo;
}
