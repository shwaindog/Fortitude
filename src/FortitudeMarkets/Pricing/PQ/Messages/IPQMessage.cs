using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeIO.Protocols;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

namespace FortitudeMarkets.Pricing.PQ.Messages;

public interface IPQMessage : IMutableFeedEventStatusUpdate, IVersionedMessage, IPQSupportsStringUpdates<IPQMessage>
  , IPQSupportsFieldUpdates<IPQMessage>,
    ITrackableReset<IPQMessage>, IRelatedItems<IPQMessage>, IDoublyLinkedListNode<IPQMessage>, IInterfacesComparable<IPQMessage>
  , ICloneable<IPQMessage>, IEmptyable
{
    PQMessageFlags? OverrideSerializationFlags { get; set; }

    uint      StreamId            { get; }
    string    StreamName          { get; }
    ISyncLock Lock                { get; }
    uint      PQSequenceId        { get; set; }
    DateTime  LastPublicationTime { get; set; }

    public bool IsFeedConnectivityStatusUpdated { get; set; }

    bool IsSocketReceivedTimeDateUpdated     { get; set; }
    bool IsSocketReceivedTimeSub2MinUpdated  { get; set; }
    bool IsProcessedTimeDateUpdated          { get; set; }
    bool IsProcessedTimeSub2MinUpdated       { get; set; }
    bool IsDispatchedTimeDateUpdated         { get; set; }
    bool IsDispatchedTimeSub2MinUpdated      { get; set; }
    bool IsClientReceivedTimeDateUpdated     { get; set; }
    bool IsClientReceivedTimeSub2MinUpdated  { get; set; }
    bool IsAdapterSentTimeDateUpdated        { get; set; }
    bool IsAdapterSentTimeSub2MinUpdated     { get; set; }
    bool IsAdapterReceivedTimeDateUpdated    { get; set; }
    bool IsAdapterReceivedTimeSub2MinUpdated { get; set; }
    bool IsFeedSyncStatusUpdated             { get; set; }

    new IPQMessage? Previous { get; set; }
    new IPQMessage? Next     { get; set; }

    new IPQMessage CopyFrom(IPQMessage source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default);

    void SetPublisherStateToConnectivityStatus(PublisherStates publisherStates, DateTime atDateTime);

    new IPQMessage Clone();
}
