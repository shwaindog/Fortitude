using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Logging.Config.Pooling;
using FortitudeCommon.Logging.Core.LogEntries;
using FortitudeCommon.Types.Mutable.Strings;

namespace FortitudeCommon.Logging.Core.Pooling;

public class FLogEntryPool
{
    private readonly Recycler backingPool = new ();

    public FLogEntryPool(IFLogEntryPoolConfig logEntryPoolDefinition)
    {
        PoolOwnerName = logEntryPoolDefinition.PoolName;
        PoolScope     = logEntryPoolDefinition.PoolScope;

        NewLogEntryCharCapacity  = logEntryPoolDefinition.LogEntryCharCapacity;
        NewBatchLogEntryListSize = logEntryPoolDefinition.LogEntriesBatchSize;

        backingPool.RegisterFactory(() =>
        {
            var mutString = new MutableString(NewLogEntryCharCapacity);
            return new FLogEntry(mutString);
        });
        backingPool.RegisterFactory(() => new LogEntriesBatch(NewBatchLogEntryListSize));
    }

    public IRecycler Recycler => backingPool;

    public string PoolOwnerName { get; }

    public int NewLogEntryCharCapacity { get; set; }

    public int NewBatchLogEntryListSize { get; set; }

    public PoolScope PoolScope { get; }

    public FLogEntry SourceLogEntry()
    {
        return backingPool.Borrow<FLogEntry>();
    }

    public ILogEntriesBatch SourceBatchLogEntryContainer(int minimumSize)
    {
        var container = backingPool.Borrow<LogEntriesBatch>();
        container.Capacity = Math.Max(container.Capacity,  minimumSize);
        return container;
    }
}