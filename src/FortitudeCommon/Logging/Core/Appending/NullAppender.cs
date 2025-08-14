using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.Appending.Forwarding;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending;

public class NullAppender(INullAppenderConfig appenderDefinitionConfig)
    : FLogEntrySinkBase, IMutableFLogAppender
{
    public const string NullAppenderName = $"{nameof(NullAppender)}";

    public static NullAppender NullInstance = new(new NullAppenderConfig());

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
        set => _ = value!;
    }

    public int ReceiveOnAsyncQueueNumber => 0;

    public IAppenderClient CreateAppenderClientFor(IFLoggerCommon logger) => NullAppenderClient.NullClientInstance;

    public IAppenderClient CreateAppenderClientFor(IFLogForwardingAppender appender) => NullAppenderClient.NullClientInstance;

    public void ExecuteJob(Action job) { }

    public IAppenderDefinitionConfig GetAppenderConfig() => appenderDefinitionConfig;

    public void HandleConfigUpdate(IAppenderDefinitionConfig newLoggerState) { }

    public IFLogEntryPipelineEndpoint ReceiveEndpoint => NullAppenderClient.NullClientInstance;

    public void ReceiveOldAppenderTypeClients(List<IMutableAppenderClient> issuedAppenders) { }

    public override void OnReceiveLogEntry(LogEntryPublishEvent logEntryEvent, ITargetingFLogEntrySource fromPublisher) { }

    public void RunJobOnAppenderQueue(Action job) { }
}
