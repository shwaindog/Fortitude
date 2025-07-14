using System.Collections.Concurrent;
using System.Diagnostics;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Core.Appending;
using FortitudeCommon.Logging.Core.Pooling;

namespace FortitudeCommon.Logging.Core.Hub;

public delegate void HandleConfigUpdate(ConsolidatedLoggerConfig potentialUpdate);

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

    public void OnUpdated(ConsolidatedLoggerConfig potentialUpdate)
    {
        Updated?.Invoke(potentialUpdate);
    }

    public void Deconstruct(out IFLogger logger)
    {
        logger = Logger;
    }
}

public interface IFLoggerRegistry
{
    IFLoggerRoot Root { get; }

    IReadOnlyDictionary<string, LoggerContainer> EmbodiedLoggers { get; }

    void NotifyLoggerConfigUpdate(ConsolidatedLoggerConfig potentialUpdate);

    NotifyNewFLoggerHandler RegisterLoggerCallback { get; }

    FLogEntryPool SourceFLogEntryPool(ExplicitLogEntryPoolDefinition logEntryPoolDefinition);
}


public class FLoggerRegistry : IFLoggerRegistry
{
    private readonly ConcurrentDictionary<string, LoggerContainer> embodiedLoggers = new();

    private readonly FLogEntryPoolRegistry                         logEntryPoolRegistry = new();

    public FLoggerRegistry(IFLoggerRoot root)
    {
        Root = root;

        RegisterLoggerCallback = RegisterLogger;
    }

    public IReadOnlyDictionary<string, LoggerContainer> EmbodiedLoggers => embodiedLoggers.AsReadOnly();

    public void NotifyLoggerConfigUpdate(ConsolidatedLoggerConfig potentialUpdate)
    {
        if (embodiedLoggers.TryGetValue(potentialUpdate.LoggerFullName, out var loggerContainer))
        {
            loggerContainer.OnUpdated(potentialUpdate);
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

    public FLogEntryPool SourceFLogEntryPool(ExplicitLogEntryPoolDefinition logEntryPoolDefinition) => logEntryPoolRegistry.ResolveFLogEntryPool(logEntryPoolDefinition);
}
