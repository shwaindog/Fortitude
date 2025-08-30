// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using System.Diagnostics;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Core.Pooling;

namespace FortitudeCommon.Logging.Core.Hub;

public delegate void HandleConfigUpdate(IMutableFLoggerDescendantConfig potentialUpdate, IFLogAppenderRegistry appenderRegistry);

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

    public IFLogger Logger { get; init; }

    public event HandleConfigUpdate Updated;

    public void OnUpdated(IMutableFLoggerDescendantConfig potentialUpdate, IFLogAppenderRegistry appenderRegistry)
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

    IFLogAppenderRegistry AppenderRegistry { get; }

    NotifyNewFLoggerHandler RegisterLoggerCallback { get; }

    FLogEntryPool SourceFLogEntryPool(IFLogEntryPoolConfig logEntryPoolDefinition);
}

public interface IMutableFLogLoggerRegistry : IFLogLoggerRegistry
{
    new IMutableFLoggerRoot Root { get; set; }

    void NotifyLoggerConfigUpdate(IMutableFLoggerDescendantConfig potentialUpdate);

    void HandleRootLoggerConfigUpdate(IMutableFLoggerRootConfig newRootLoggerState, IFLogAppenderRegistry appenderRegistry);
}

public class FLogLoggerRegistry : IMutableFLogLoggerRegistry
{
    private readonly ConcurrentDictionary<string, LoggerContainer> embodiedLoggers = new();

    private readonly IFLogEntryPoolRegistry logEntryPoolRegistry;

    private IMutableFLoggerRoot root;

    public FLogLoggerRegistry(IMutableFLoggerRootConfig rootConfig, IFLogAppenderRegistry appenderRegistry, IFLogEntryPoolRegistry entryPoolReg)
    {
        AppenderRegistry = appenderRegistry;

        logEntryPoolRegistry = entryPoolReg;

        root = FLoggerRoot.ImmortalInstance;

        RegisterLoggerCallback = RegisterLogger;
    }

    public IReadOnlyDictionary<string, LoggerContainer> EmbodiedLoggers => embodiedLoggers.AsReadOnly();

    public void NotifyLoggerConfigUpdate(IMutableFLoggerDescendantConfig potentialUpdate)
    {
        if (embodiedLoggers.TryGetValue(potentialUpdate.FullName, out var loggerContainer))
            loggerContainer.OnUpdated(potentialUpdate, FLogContext.Context.AppenderRegistry);
    }

    public NotifyNewFLoggerHandler RegisterLoggerCallback { get; }

    public IFLogAppenderRegistry AppenderRegistry { get; internal set; }

    public void HandleRootLoggerConfigUpdate(IMutableFLoggerRootConfig newRootLoggerState, IFLogAppenderRegistry appenderRegistry)
    {
        AppenderRegistry = appenderRegistry;
        root.HandleRootLoggerConfigUpdate(newRootLoggerState);
    }

    IMutableFLoggerRoot IMutableFLogLoggerRegistry.Root
    {
        get => root;
        set
        {
            if (root == value) return;
            if (root != null) throw new ApplicationException("You can not update a root logger once created");
            root = value!;
        }
    }

    public IFLoggerRoot Root => root;

    public FLogEntryPool SourceFLogEntryPool(IFLogEntryPoolConfig logEntryPoolDefinition) =>
        logEntryPoolRegistry.ResolveFLogEntryPool(logEntryPoolDefinition);

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
}

public static class FLogLoggerRegistryExtensions
{
    public static IMutableFLogLoggerRegistry? UpdateConfig
        (this IFLogLoggerRegistry? maybeCreated, IMutableFLoggerRootConfig rootLoggerConfig, IFLogAppenderRegistry appenderRegistry)
    {
        if (maybeCreated is IMutableFLogLoggerRegistry maybeMutable)
        {
            maybeMutable.HandleRootLoggerConfigUpdate(rootLoggerConfig, appenderRegistry);
            return maybeMutable;
        }
        return null;
    }
}
