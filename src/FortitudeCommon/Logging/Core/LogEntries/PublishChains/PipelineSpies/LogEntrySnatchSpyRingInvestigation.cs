// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using FortitudeCommon.DataStructures.Lists;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains.PipelineSpies;

public class LogEntrySnatchSpyRingInvestigation(int maxLiveIntelSize = 64)
{
    private readonly CappedSizeDroppingAppendList<IFLogEntry> currentAssets = new(maxLiveIntelSize);
    private readonly List<LogEntrySnatchEventStateSpy>        currentSpies  = new();

    private readonly ReusableList<IFLogEntry> snapLatestIntel = new();

    public LogEntrySnatchEventStateSpy TrainNewSpy(string spyName)
    {
        var newSpy = new LogEntrySnatchEventStateSpy(currentAssets, spyName);
        currentSpies.Add(newSpy);
        return newSpy;
    }

    public void AbortOperation()
    {
        foreach (var compromisedSpy in currentSpies) compromisedSpy.InBound = null;
    }

    public IFLogEntry GetPurgeCacheLastLogEntry()
    {
        var toNewIntel = DeadDropLatestIntelToHq();
        currentAssets.Clear();
        return toNewIntel.Single();
    }

    public IReusableList<IFLogEntry> DeadDropPurgeLatestIntelToHq()
    {
        var toNewIntel = DeadDropLatestIntelToHq();
        currentAssets.Clear();
        return toNewIntel;
    }

    public IReusableList<IFLogEntry> DeadDropLatestIntelToHq()
    {
        snapLatestIntel.Clear();
        snapLatestIntel.AddRange(currentAssets);
        return snapLatestIntel;
    }
}
