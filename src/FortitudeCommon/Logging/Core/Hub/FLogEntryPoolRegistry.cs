// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Collections.Concurrent;
using FortitudeCommon.Logging.Config.Initialization;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Core.Pooling;

namespace FortitudeCommon.Logging.Core.Hub;

public interface IFLogEntryPoolRegistry
{
    ILogEntryPoolsInitializationConfig LogEntryPoolInitConfig { get; }

    FLogEntryPool ResolveFLogEntryPool(IFLogEntryPoolConfig logEntryPoolDefinition);
}

public interface IMutableFLogEntryPoolRegistry : IFLogEntryPoolRegistry
{
    new IMutableLogEntryPoolsInitializationConfig LogEntryPoolInitConfig { get; set; }
}

public class FLogEntryPoolRegistry(IMutableLogEntryPoolsInitializationConfig poolInitConfig) : IMutableFLogEntryPoolRegistry
{
    private readonly ConcurrentDictionary<PoolScope, ConcurrentDictionary<string, FLogEntryPool>> poolLookup = new();

    private IMutableLogEntryPoolsInitializationConfig logEntryPoolInitConfig = poolInitConfig;

    public ILogEntryPoolsInitializationConfig LogEntryPoolInitConfig => logEntryPoolInitConfig;

    IMutableLogEntryPoolsInitializationConfig IMutableFLogEntryPoolRegistry.LogEntryPoolInitConfig
    {
        get => logEntryPoolInitConfig;
        set => logEntryPoolInitConfig = value;
    }

    public FLogEntryPool ResolveFLogEntryPool(IFLogEntryPoolConfig logEntryPoolDefinition)
    {
        var name       = logEntryPoolDefinition.PoolName;
        var poolScope  = logEntryPoolDefinition.PoolScope;
        var nameLookup = poolLookup.GetOrAdd(poolScope, new ConcurrentDictionary<string, FLogEntryPool>());
        if (!nameLookup.TryGetValue(name, out var entryPool))
        {
            entryPool = new FLogEntryPool(logEntryPoolDefinition);
            nameLookup.TryAdd(name, entryPool);
        }
        return entryPool;
    }
}

public static class FLogEntryPoolRegistryExtensions
{
    public static IMutableFLogEntryPoolRegistry? UpdateConfig
        (this IFLogEntryPoolRegistry? maybeCreated, IMutableLogEntryPoolsInitializationConfig logEntryPoolInitCfg)
    {
        if (maybeCreated is IMutableFLogEntryPoolRegistry maybeMutable)
        {
            maybeMutable.LogEntryPoolInitConfig = logEntryPoolInitCfg;
            return maybeMutable;
        }
        return null;
    }
}
