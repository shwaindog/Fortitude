using System.Collections.Concurrent;
using System.Diagnostics;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Core.Pooling;

namespace FortitudeCommon.Logging.Core.Hub;

public delegate void HandleConfigUpdate(IFLoggerDescendantConfig potentialUpdate, IFLogAppenderRegistry appenderRegistry);

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

    public void OnUpdated(IFLoggerDescendantConfig potentialUpdate, IFLogAppenderRegistry appenderRegistry)
    {
        Updated?.Invoke(potentialUpdate, appenderRegistry);
    }

    public void Deconstruct(out IFLogger logger)
    {
        logger = Logger;
    }
}

public interface IFLogLoggerRegistry
{
    IFLoggerRoot Root { get; }

    IReadOnlyDictionary<string, LoggerContainer> EmbodiedLoggers { get; }

    NotifyNewFLoggerHandler RegisterLoggerCallback { get; }

    FLogEntryPool SourceFLogEntryPool(IFLogEntryPoolConfig logEntryPoolDefinition);
}

public interface IMutableFLogLoggerRegistry : IFLogLoggerRegistry
{
    new IMutableFLoggerRoot? Root { get; set; }

    void NotifyLoggerConfigUpdate(IFLoggerDescendantConfig potentialUpdate);
}

public class FLogLoggerRegistry : IMutableFLogLoggerRegistry
{
    private readonly ConcurrentDictionary<string, LoggerContainer> embodiedLoggers = new();

    private readonly IFLogEntryPoolRegistry logEntryPoolRegistry;

    private IMutableFLoggerRoot root;

    public FLogLoggerRegistry(IFLoggerRootConfig rootConfig, IFLogEntryPoolRegistry entryPoolReg)
    {
        logEntryPoolRegistry = entryPoolReg;

        root = new FLoggerRoot(rootConfig, this);

        RegisterLoggerCallback = RegisterLogger;
    }

    public IReadOnlyDictionary<string, LoggerContainer> EmbodiedLoggers => embodiedLoggers.AsReadOnly();

    public void NotifyLoggerConfigUpdate(IFLoggerDescendantConfig potentialUpdate)
    {
        if (embodiedLoggers.TryGetValue(potentialUpdate.FullName, out var loggerContainer))
        {
            loggerContainer.OnUpdated(potentialUpdate, FLogContext.Context.AppenderRegistry);
        }
    }

    public NotifyNewFLoggerHandler RegisterLoggerCallback { get; }

    IMutableFLoggerRoot? IMutableFLogLoggerRegistry.Root
    {
        get => root;
        set
        {
            if (root != null)
            {
                throw new ApplicationException("You can not update a root logger once created");
            }
            root = value!;
        }
    }

    public IFLoggerRoot Root => root;

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

public static class FLogLoggerRegistryExtensions
{
    public static IMutableFLogLoggerRegistry? UpdateConfig
        (this IFLogLoggerRegistry? maybeCreated, IFLoggerRootConfig rootLoggerConfig, IFLogAppenderRegistry appenderRegistry)
    {
        if (maybeCreated is IMutableFLogLoggerRegistry maybeMutable)
        {
            maybeMutable.Root!.HandleRootLoggerConfigUpdate(rootLoggerConfig, appenderRegistry);
            return maybeMutable;
        }
        return null;
    }
}
