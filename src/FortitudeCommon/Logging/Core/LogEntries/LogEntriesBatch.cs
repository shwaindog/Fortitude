// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeCommon.Types.StringsOfPower.DieCasting.CollectionPurification;

namespace FortitudeCommon.Logging.Core.LogEntries;

public interface ILogEntriesBatch : IReusableList<IFLogEntry>, IStringBearer { }

public class LogEntriesBatch : ReusableList<IFLogEntry>, ILogEntriesBatch
{
    private static readonly OrderedCollectionPredicate<IFLogEntry> FirstThree
        = CollectionFilterExtensions.ResolveAcceptFirstOrderedCollectionItemsPredicate<IFLogEntry>(3);
    public LogEntriesBatch() { }
    public LogEntriesBatch(IRecycler recycler, int size = 16) : base(recycler, size) { }
    public LogEntriesBatch(int size = 16) : base(size) { }
    public LogEntriesBatch(ReusableList<IFLogEntry> toClone) : base(toClone) { }

    public IReadOnlyList<IFLogEntry> AsReadOnly => this;
    public IEnumerable<IFLogEntry> AsEnumerable => this;

    public StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexCollectionType(this)
           .LogOnlyField.AlwaysAdd(nameof(RefCount), RefCount)
           .LogOnlyField.AlwaysAdd(nameof(Count), Count)
           .RevealFiltered(AsReadOnly, FirstThree).Complete();
}
