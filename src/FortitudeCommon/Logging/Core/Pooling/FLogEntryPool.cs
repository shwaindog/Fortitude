using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config.LoggersHierarchy;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable;

namespace FortitudeCommon.Logging.Core.Pooling;


public class FLogEntryPoolRegistry
{
    private const string GlobalName = "Global";

    private readonly ConcurrentDictionary<PoolScope, ConcurrentDictionary<string, FLogEntryPool>> poolLookup = new();

    public FLogEntryPool ResolveFLogEntryPool(ExplicitLogEntryPoolDefinition logEntryPoolDefinition)
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

public class FLogEntryPool
{
    private readonly Recycler backingPool = new ();

    public FLogEntryPool(ExplicitLogEntryPoolDefinition logEntryPoolDefinition)
    {
        PoolOwnerName = logEntryPoolDefinition.PoolName;
        PoolScope     = logEntryPoolDefinition.PoolScope;

        var initialLogEntryBuilderSize = logEntryPoolDefinition.NewItemCapacity;
        backingPool.RegisterFactory(() =>
        {
            var mutString = new MutableString(initialLogEntryBuilderSize);
            return new FLogEntry(mutString);
        });
        var batchProcessingSize = logEntryPoolDefinition.ItemBatchSize;
        backingPool.RegisterFactory(() => new ReusableList<IFLogEntry>(batchProcessingSize));
    }

    public string PoolOwnerName { get; }

    public PoolScope PoolScope { get; }

    public FLogEntry SourceLogEntry()
    {
        return backingPool.Borrow<FLogEntry>();
    }

    public ReusableList<IFLogEntry> SourceBatchLogEntryContainer(int minimumSize)
    {
        var container = backingPool.Borrow<ReusableList<IFLogEntry>>();
        container.Capacity = Math.Max(container.Capacity,  minimumSize);
        return container;
    }
}