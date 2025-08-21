using FortitudeCommon.Logging.Config.Appending;
using FortitudeCommon.Logging.Config.Appending.Forwarding;
using FortitudeCommon.Logging.Core.Hub;
using FortitudeCommon.Logging.Core.LogEntries.PublishChains;

namespace FortitudeCommon.Logging.Core.Appending.Forwarding;

public interface IFLogForwardingAppender : IFLogAppender
{
    IReadOnlyList<KeyValuePair<int, List<IAppenderClient>>> ForwardToAppenders { get; }
}

public interface IMutableFLogForwardingAppender : IFLogForwardingAppender, IMutableFLogAppender
{
    new IReadOnlyList<KeyValuePair<int, List<IAppenderClient>>> ForwardToAppenders { get; set; }

    void AddAppender(IAppenderClient newAppender);

    bool RemoveAppenderName(string removeAppenderName);
}

public class FLogForwardingAppender : FLogAppender, IMutableFLogForwardingAppender
{
    protected readonly IFLogAppenderRegistry AppenderRegistry;

    private List<KeyValuePair<int, List<IAppenderClient>>> allForwardToAppendersCache = new();

    public FLogForwardingAppender(IForwardingAppenderConfig forwardingAppenderConfig, IFLogContext context)
        : base(forwardingAppenderConfig, context)
    {
        AppenderRegistry = context.AppenderRegistry;
        AppenderType     = $"{nameof(FLoggerBuiltinAppenderType.Forwarding)}";

        ParseAppenderConfig(forwardingAppenderConfig, AppenderRegistry);
    }

    protected virtual void ParseAppenderConfig(IForwardingAppenderConfig forwardingAppenderConfig, IFLogAppenderRegistry fLogAppenderRegistry)
    {
        var forwardingAppendersLookupConfig = forwardingAppenderConfig.ForwardToAppenders;
        var newAppenderForwardToList = new List<KeyValuePair<int, List<IAppenderClient>>>();
        foreach (var downStreamAppenderRef in forwardingAppendersLookupConfig)
        {
            var appenderName   = downStreamAppenderRef.Value.AppenderName;
            var appenderClient = fLogAppenderRegistry.GetAppenderClient(appenderName, this);
            AddAppender(appenderClient);
        }
        allForwardToAppendersCache = newAppenderForwardToList;
    }

    protected override IAppenderForwardingAsyncClient CreateAppenderAsyncClient
        (IAppenderDefinitionConfig appenderDefinitionConfig, IFLoggerAsyncRegistry asyncRegistry)
    {
        var processAsync = appenderDefinitionConfig.RunOnAsyncQueueNumber;

        var appenderAsyncClient = new ForwardingAsyncClient(this, processAsync, asyncRegistry);
        return appenderAsyncClient;
    }

    protected IAppenderForwardingAsyncClient ForwardingAsyncClient => (IAppenderForwardingAsyncClient)AsyncClient;

    public override void ProcessReceivedLogEntryEvent(LogEntryPublishEvent logEntryEvent)
    {
        var forwardToList = ForwardToAppenders;
        for (var i = 0; i < forwardToList.Count; i++)
        {
            var appendersByQueueNum = forwardToList[i];
            for (var j = 0; j < appendersByQueueNum.Value.Count; j++)
            {
                var appender = appendersByQueueNum.Value[j];
                appender.PublishLogEntryEvent(logEntryEvent);
            }
        }
    }

    protected IForwardingAppenderConfig TypeConfig => (IForwardingAppenderConfig)AppenderConfig;

    public IReadOnlyList<KeyValuePair<int, List<IAppenderClient>>> ForwardToAppenders
    {
        get => allForwardToAppendersCache;
        set => allForwardToAppendersCache = (List<KeyValuePair<int, List<IAppenderClient>>>)value;
    }

    public void AddAppender(IAppenderClient newAppender)
    {
        var forwardToList = allForwardToAppendersCache;
        var onQueue       = newAppender.ReceiveOnAsyncQueueNumber;
        var foundQueueNumList =
            forwardToList
                .FirstOrDefault(kvp => kvp.Key == onQueue);
        if (Equals(foundQueueNumList, null))
        {
            forwardToList.Add(new KeyValuePair<int, List<IAppenderClient>>(onQueue, [newAppender]));
        }
        else
        {
            foundQueueNumList.Value.Add(newAppender);
        }
    }

    public bool RemoveAppenderName(string removeAppenderName)
    {
        var forwardToList = ForwardToAppenders;
        for (var i = 0; i < forwardToList.Count; i++)
        {
            var appendersByQueueNum = forwardToList[i];
            var appendersOnQueue    = appendersByQueueNum.Value;
            for (var j = 0; j < appendersOnQueue.Count; j++)
            {
                var appender = appendersByQueueNum.Value[j];
                if (appender.BackingAppender.AppenderName == removeAppenderName)
                {
                    appendersOnQueue[i] = NullAppenderClient.NullClientInstance;
                    return true;
                }
            }
        }
        return false;
    }

    public override void HandleConfigUpdate(IAppenderDefinitionConfig newAppenderConfig)
    {
        base.HandleConfigUpdate(newAppenderConfig);

        ParseAppenderConfig(TypeConfig, AppenderRegistry);
    }

    public override IForwardingAppenderConfig GetAppenderConfig() => (IForwardingAppenderConfig)AppenderConfig;
}
