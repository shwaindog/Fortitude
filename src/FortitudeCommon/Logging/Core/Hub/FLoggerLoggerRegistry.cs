using System.Collections.Concurrent;
using System.Diagnostics;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Config.Visitor;
using FortitudeCommon.Logging.Core.Pooling;

namespace FortitudeCommon.Logging.Core.Hub;

public delegate void HandleConfigUpdate(IFLoggerDescendantConfig potentialUpdate, IFloggerAppenderRegistry appenderRegistry);

public delegate void NotifyFLoggerHandler(IFLogger logger);

public delegate void NotifyNewFLoggerHandler(IMutableFLogger logger);

public class FLoggerBuiltinLoggers()
{
    //public IFLogger AppenderQueueFullLogger = 
}

public record LoggerContainer
{
    public LoggerContainer(IMutableFLogger logger)
    {
        Logger = logger;

        Updated += logger.HandleConfigUpdate;
    }

    public event HandleConfigUpdate Updated;

    public IFLogger Logger { get; init; }

    public void OnUpdated(IFLoggerDescendantConfig potentialUpdate, IFloggerAppenderRegistry appenderRegistry)
    {
        Updated?.Invoke(potentialUpdate, appenderRegistry);
    }

    public void Deconstruct(out IFLogger logger)
    {
        logger = Logger;
    }
}

public interface IFLoggerLoggerRegistry
{
    IFLoggerRoot Root { get; }

    IReadOnlyDictionary<string, LoggerContainer> EmbodiedLoggers { get; }

    void NotifyLoggerConfigUpdate(IFLoggerDescendantConfig potentialUpdate);

    NotifyNewFLoggerHandler RegisterLoggerCallback { get; }

    FLogEntryPool SourceFLogEntryPool(IFLogEntryPoolConfig logEntryPoolDefinition);
}

public class FLoggerLoggerRegistry : IFLoggerLoggerRegistry
{
    private readonly ConcurrentDictionary<string, LoggerContainer> embodiedLoggers = new();

    private readonly FLogEntryPoolRegistry logEntryPoolRegistry = new();

    public FLoggerLoggerRegistry(IFLoggerRootConfig root)
    {
        Root = new FLoggerRoot(root, this);

        RegisterLoggerCallback = RegisterLogger;
    }

    public IReadOnlyDictionary<string, LoggerContainer> EmbodiedLoggers => embodiedLoggers.AsReadOnly();

    public void NotifyLoggerConfigUpdate(IFLoggerDescendantConfig potentialUpdate)
    {
        if (embodiedLoggers.TryGetValue(potentialUpdate.ResolveFullName(), out var loggerContainer))
        {
            loggerContainer.OnUpdated(potentialUpdate, FLoggerContext.Instance.AppenderRegistry);
        }
    }

    public NotifyNewFLoggerHandler RegisterLoggerCallback { get; }

    public IFLoggerRoot Root { get; internal set; }

    protected void RegisterLogger(IMutableFLogger newLogger)
    {
        if (!embodiedLoggers.TryAdd(newLogger.FullName, new LoggerContainer(newLogger)))
        {
            #if DEBUG
            Debugger.Break();
            #endif
            return;
        }
    }

    public FLogEntryPool SourceFLogEntryPool(IFLogEntryPoolConfig logEntryPoolDefinition) =>
        logEntryPoolRegistry.ResolveFLogEntryPool(logEntryPoolDefinition);
}
