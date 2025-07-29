using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Core.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending;

public class NullAppender(INullAppenderConfig appenderDefinitionConfig, IFLogContext context) 
    : FLogAppender(appenderDefinitionConfig, context)
{
    public static NullAppender NullInstance = new (new NullAppenderConfig(), FLogContext.Context);

    public override void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent) { }

    public override INullAppenderConfig GetAppenderConfig() => (INullAppenderConfig)AppenderConfig;

    public override IAppenderClient CreateAppenderClientFor(IFLoggerCommon logger) => NullAppenderClient.NullClientInstance;

    public override IAppenderClient CreateAppenderClientFor(IFLogForwardingAppender appender) => NullAppenderClient.NullClientInstance;
}