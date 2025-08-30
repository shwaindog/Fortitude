using FortitudeCommon.DataStructures.Lists;

namespace FortitudeCommon.Logging.Core.LogEntries.PublishChains.PipelineSpies;

public class SnapLogEntrySpyRingInvestigation(int maxLiveIntelSize = 64)
{
    private readonly List<SnapLogEntryEventStateSpy> currentSpies = new();

    private readonly CappedSizeDroppingAppendList<string> currentIntel = new (maxLiveIntelSize);

    private readonly List<string?> snapLatestIntel = new ();

    public SnapLogEntryEventStateSpy TrainNewSpy(string spyName)
    {
        var newSpy = new SnapLogEntryEventStateSpy(currentIntel, spyName);
        currentSpies.Add(newSpy);
        return newSpy;
    }

    public void AbortOperation()
    {
        foreach (var compromisedSpy in currentSpies)
        {
            compromisedSpy.InBound = null;
        }
    }

    public List<string?> DeadDropLatestIntelToHq()
    {
        snapLatestIntel.Clear();
        snapLatestIntel.AddRange(currentIntel);
        return snapLatestIntel;
    }
}