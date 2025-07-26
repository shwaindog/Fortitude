using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.Pooling;

public class FLogEntryPool
{
    private readonly Recycler backingPool = new ();

    public FLogEntryPool(IFLogEntryPoolConfig logEntryPoolDefinition)
    {
        PoolOwnerName = logEntryPoolDefinition.PoolName;
        PoolScope     = logEntryPoolDefinition.PoolScope;

        var initialLogEntryBuilderSize = logEntryPoolDefinition.LogEntryCharCapacity;
        backingPool.RegisterFactory(() =>
        {
            var mutString = new MutableString(initialLogEntryBuilderSize);
            return new FLogEntry(mutString);
        });
        var batchProcessingSize = logEntryPoolDefinition.LogEntriesBatchSize;
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