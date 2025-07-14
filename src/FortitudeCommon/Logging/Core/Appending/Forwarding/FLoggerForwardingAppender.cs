using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding;

public interface IFLoggerForwardingAppender
{
    IReadOnlyList<IFLoggerAppender> ForwardToAppenders { get; }
}

public interface IMutableFLoggerForwardingAppender : IFLoggerForwardingAppender
{
    new IReadOnlyList<IFLoggerAppender> ForwardToAppenders { get; set; }

    void AddAppender(IFLoggerAppender newAppender);

    void RemoveAppenderName(string removeAppenderName);
}

public class FLoggerForwardingAppender : FLoggerAppender, IMutableFLoggerForwardingAppender
{
    protected readonly IAppenderRegistry AppenderRegistry;

    private readonly NotifyAppenderHandler receiveDownstreamAppender;

    private List<IFLoggerAppender> forwardToAppender = new();

    private CountdownEvent? updatesOccuringCountdownEvent;

    public FLoggerForwardingAppender(IForwardingAppenderConfig forwardingAppenderConfig, IAppenderRegistry appenderRegistry)
        : base(forwardingAppenderConfig)
    {
        AppenderRegistry = appenderRegistry;
        AppenderType     = FloggerAppenderType.Forwarding;

        receiveDownstreamAppender = AddAppender;

        ParseAppenderConfig(forwardingAppenderConfig, appenderRegistry);

        if (GetType() == typeof(FLoggerForwardingAppender))
        {
            appenderRegistry.RegisterAppenderCallback(this);
        }
    }

    protected virtual void ParseAppenderConfig(IForwardingAppenderConfig forwardingAppenderConfig, IAppenderRegistry appenderRegistry)
    {
        var forwardingAppendersLookupConfig = forwardingAppenderConfig.ForwardToAppenders;
        var expectedNumberOfAppenders       = forwardingAppendersLookupConfig.Count;
        updatesOccuringCountdownEvent = new CountdownEvent(expectedNumberOfAppenders);
        forwardToAppender.Clear();
        foreach (var downStreamAppenderRef in forwardingAppendersLookupConfig)
        {
            var appenderName      = downStreamAppenderRef.Value.AppenderName ?? "";
            var appenderConfigRef = downStreamAppenderRef.Value.AppenderConfigRef;
            appenderRegistry.RegistryAppenderInterest(receiveDownstreamAppender, appenderName, appenderConfigRef);
        }
    }

    protected override void Append(IFLogEntry logEntry)
    {
        updatesOccuringCountdownEvent?.Wait();
        for (var i = 0; i < ForwardToAppenders.Count; i++)
        {
            var appender = ForwardToAppenders[i];
            appender.ForwardLogEntryTo(logEntry);
        }
        logEntry.DecrementRefCount();
    }

    protected virtual void AppendBatch(IReusableList<IFLogEntry> logEntryBatch)
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

    public IReadOnlyList<IFLoggerAppender> ForwardToAppenders
    {
        get => forwardToAppender;
        set => forwardToAppender = (List<IFLoggerAppender>)value;
    }

    public void AddAppender(IFLoggerAppender newAppender)
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
}
