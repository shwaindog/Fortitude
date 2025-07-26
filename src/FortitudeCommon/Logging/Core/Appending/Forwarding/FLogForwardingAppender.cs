using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding;

public interface IFLoggerForwardingAppender
{
    IReadOnlyList<IFLogAppender> ForwardToAppenders { get; }
}

public interface IMutableFLoggerForwardingAppender : IFLoggerForwardingAppender
{
    new IReadOnlyList<IFLogAppender> ForwardToAppenders { get; set; }

    void AddAppender(IFLogAppender newAppender);

    void RemoveAppenderName(string removeAppenderName);
}

public class FLogForwardingAppender : FLogAppender, IMutableFLoggerForwardingAppender
{
    protected readonly IFLogAppenderRegistry AppenderRegistry;

    private readonly NotifyAppenderHandler receiveDownstreamAppender;

    private List<IFLogAppender> forwardToAppender = new();

    private CountdownEvent? updatesOccuringCountdownEvent;

    public FLogForwardingAppender(IForwardingAppenderConfig forwardingAppenderConfig, IFLogContext context)
        : base(forwardingAppenderConfig, context)
    {
        AppenderRegistry = context.AppenderRegistry;
        AppenderType     = $"{nameof(FLoggerBuiltinAppenderType.SyncForwarding)}";

        receiveDownstreamAppender = AddAppender;

        ParseAppenderConfig(forwardingAppenderConfig, AppenderRegistry);
    }

    protected virtual void ParseAppenderConfig(IForwardingAppenderConfig forwardingAppenderConfig, IFLogAppenderRegistry fLogAppenderRegistry)
    {
        var forwardingAppendersLookupConfig = forwardingAppenderConfig.ForwardToAppenders;
        var expectedNumberOfAppenders       = forwardingAppendersLookupConfig.Count;
        updatesOccuringCountdownEvent = new CountdownEvent(expectedNumberOfAppenders);
        forwardToAppender.Clear();
        foreach (var downStreamAppenderRef in forwardingAppendersLookupConfig)
        {
            var appenderName      = downStreamAppenderRef.Value.AppenderName ?? "";
            fLogAppenderRegistry.RegistryAppenderInterest(receiveDownstreamAppender, appenderName);
        }
    }

    public override void Append(IFLogEntry logEntry)
    {
        updatesOccuringCountdownEvent?.Wait();
        for (var i = 0; i < ForwardToAppenders.Count; i++)
        {
            var appender = ForwardToAppenders[i];
            appender.ForwardLogEntryTo(logEntry);
        }
        logEntry.DecrementRefCount();
    }

    public override void Append(IReusableList<IFLogEntry> logEntryBatch)
    {
        updatesOccuringCountdownEvent?.Wait();
        for (var i = 0; i < ForwardToAppenders.Count; i++)
        {
            var appender = ForwardToAppenders[i];
            appender.BatchForwardTo(logEntryBatch);
        }
        for (var i = 0; i < logEntryBatch.Count; i++)
        {
            var logEntry = logEntryBatch[i];
            logEntry.DecrementRefCount();
        }
        logEntryBatch.Clear();
        logEntryBatch.DecrementRefCount();
    }

    protected IForwardingAppenderConfig TypeConfig => (IForwardingAppenderConfig)AppenderConfig;

    public override void BatchForwardTo(IReusableList<IFLogEntry> batchLogEntries)
    {
        batchLogEntries.IncrementRefCount();
        updatesOccuringCountdownEvent?.Wait();
        for (var i = 0; i < ForwardToAppenders.Count; i++)
        {
            var appender = ForwardToAppenders[i];
            appender.BatchForwardTo(batchLogEntries);
        }
        batchLogEntries.DecrementRefCount();
    }

    public IReadOnlyList<IFLogAppender> ForwardToAppenders
    {
        get => forwardToAppender;
        set => forwardToAppender = (List<IFLogAppender>)value;
    }

    public void AddAppender(IFLogAppender newAppender)
    {
        updatesOccuringCountdownEvent?.Signal();
        forwardToAppender.Add(newAppender);
    }

    public void RemoveAppenderName(string removeAppenderName)
    {
        for (var i = 0; i < forwardToAppender.Count; i++)
        {
            var appender = forwardToAppender[i];
            if (appender.AppenderName == removeAppenderName)
            {
                forwardToAppender.RemoveAt(i);
                return;
            }
        }
    }

    public override void HandleConfigUpdate(IAppenderDefinitionConfig newAppenderConfig)
    {
        base.HandleConfigUpdate(newAppenderConfig);

        ParseAppenderConfig(TypeConfig, AppenderRegistry);
    }

    public override IForwardingAppenderConfig GetAppenderConfig() => (IForwardingAppenderConfig)AppenderConfig;
}
