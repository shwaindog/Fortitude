#region

using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

#endregion

namespace FortitudeMarketsCore.Pricing.PQ.Publication;

public interface IPQServerHeartBeatSender
{
    bool HasStarted { get; }
    IDoublyLinkedList<IPQLevel0Quote>? ServerLinkedQuotes { get; set; }
    ISyncLock? ServerLinkedLock { get; set; }
    IPQUpdateServer? UpdateServer { get; set; }
    void StartSendingHeartBeats();
    void StopAndWaitUntilFinished();
}
