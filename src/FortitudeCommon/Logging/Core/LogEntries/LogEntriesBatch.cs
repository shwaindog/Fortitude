// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface ILogEntriesBatch : IReusableList<IFLogEntry>, IStyledToStringObject { }

public class LogEntriesBatch : ReusableList<IFLogEntry>, ILogEntriesBatch
{
    private static readonly OrderedCollectionPredicate<IFLogEntry> FirstThree =
        (count, _) => count < 3;
    public LogEntriesBatch() { }
    public LogEntriesBatch(IRecycler recycler, int size = 16) : base(recycler, size) { }
    public LogEntriesBatch(int size = 16) : base(size) { }
    public LogEntriesBatch(ReusableList<IFLogEntry> toClone) : base(toClone) { }

    public IReadOnlyList<IFLogEntry> AsReadOnly => this;
    public IEnumerable<IFLogEntry> AsEnumerable => this;

    public StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa)
    {
        var tb = stsa.StartComplexCollectionType(this);
        tb.LogOnlyField?.AlwaysAdd(nameof(RefCount), RefCount);
        tb.LogOnlyField?.AlwaysAdd(nameof(Count), Count);
        return tb.AddFiltered(AsReadOnly, FirstThree).Complete();
    }
}
