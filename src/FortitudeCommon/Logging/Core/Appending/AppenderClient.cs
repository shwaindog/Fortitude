using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.Appending.Forwarding;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending;

public interface IAppenderClient : IFLogEntryPipelineEndpoint
{
    IFLogAppender BackingAppender { get; }

    int ReceiveOnAsyncQueueNumber { get; }

    IAppenderDefinitionConfig GetAppenderConfig();
}

public interface IMutableAppenderClient : IAppenderClient
{
    new IFLogAppender BackingAppender { get; set; }
}

public class AppenderClient : FLogEntryPipelineEndpoint, IMutableAppenderClient
{
    // ReSharper disable once ConvertToPrimaryConstructor
    public AppenderClient(IFLogAppender originalAppender, string name) : base(name, originalAppender.ReceiveEndpoint)
    {
        BackingAppender = originalAppender;
    }

    public IFLogAppender BackingAppender { get; set; }

    public int ReceiveOnAsyncQueueNumber => BackingAppender.ReceiveOnAsyncQueueNumber;

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

public class NullAppenderClient() : FLogEntryPipelineEndpoint("NullAppenderClient", null!), IMutableAppenderClient
{
    public static readonly NullAppenderClient NullClientInstance = new();

    public IFLogAppender BackingAppender { get; set; } = NullAppender.NullInstance;

    public string AppenderName => INullAppenderConfig.NullAppenderName;
    public string AppenderType => INullAppenderConfig.NullAppenderType;

    public int ReceiveOnAsyncQueueNumber => 0;

    public IAppenderDefinitionConfig GetAppenderConfig() => BackingAppender.GetAppenderConfig();

    public IFLogEntryPipelineEndpoint ReceiveEndpoint => BackingAppender.ReceiveEndpoint;

    public void RunJobOnAppenderQueue(Action job) { }

    public void ExecuteJob(Action job) { }
}

public class LoggerAppenderClient(IFLogAppender originalAppender, IFLoggerCommon issuedTo)
    : AppenderClient(originalAppender, $"{issuedTo.FullName} - To - {originalAppender.AppenderName}")
{
    public IFLoggerCommon IssuedTo { get; } = issuedTo;
}

public class ForwardingAppenderClient(IFLogAppender originalAppender, IFLogForwardingAppender issuedTo)
    : AppenderClient(originalAppender, $"{issuedTo.AppenderName} - To - {originalAppender.AppenderName}")
{
    public IFLogForwardingAppender IssuedTo { get; } = issuedTo;
}
