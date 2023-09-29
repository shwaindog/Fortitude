using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.LinkedLists;
using FortitudeMarketsCore.Pricing.PQ.Quotes;

namespace FortitudeMarketsCore.Pricing.PQ.Publication
{
    public interface IPQServerHeartBeatSender
    {
        bool HasStarted { get; }
        IDoublyLinkedList<IPQLevel0Quote> ServerLinkedQuotes { get; set; }
        ISyncLock ServerLinkedLock { get; set; }
        IPQUpdateServer UpdateServer { get; set; }
        void StartSendingHeartBeats();
        void StopAndWaitUntilFinished();
    }
}