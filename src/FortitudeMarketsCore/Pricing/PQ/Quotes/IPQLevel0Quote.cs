using System;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.LinkedLists;
using FortitudeIO.Protocols;
using FortitudeMarketsApi.Pricing.Quotes;
using FortitudeMarketsCore.Pricing.PQ.DeltaUpdates;

namespace FortitudeMarketsCore.Pricing.PQ.Quotes
{
    public interface IPQLevel0Quote : IDoublyLinkedListNode<IPQLevel0Quote>, IMutableLevel0Quote, 
        IPQSupportsFieldUpdates<ILevel0Quote>, IPQSupportsStringUpdates<ILevel0Quote>, IRelatedItem<ILevel0Quote>,
        IVersionedMessage
    {
        ISyncLock Lock { get; }
        uint PQSequenceId { get; set; }
        void ResetFields();
        bool IsSourceTimeDateUpdated { get; set; }
        bool IsSourceTimeSubHourUpdated { get; set; }
        bool IsReplayUpdated { get; set; }
        bool IsSinglePriceUpdated { get; set; }
        bool IsSyncStatusUpdated { get; set; }
        DateTime LastPublicationTime { get; set; }
        DateTime SocketReceivingTime { get; set; }
        DateTime ProcessedTime { get; set; }
        DateTime DispatchedTime { get; set; }
        PQSyncStatus PQSyncStatus { get; set; }
        new IPQLevel0Quote Clone();
    }
}