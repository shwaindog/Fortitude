using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents;

public interface IFeedEventStatusUpdate : IReusableObject<IFeedEventStatusUpdate>, IInterfacesComparable<IFeedEventStatusUpdate>
{
    FeedConnectivityStatusFlags FeedMarketConnectivityStatus { get; }

    FeedSyncStatus FeedSyncStatus { get; }

    DateTime ClientReceivedTime         { get; }
    DateTime InboundSocketReceivingTime { get; }
    DateTime InboundProcessedTime       { get; }
    DateTime SubscriberDispatchedTime   { get; }
    DateTime AdapterSentTime            { get; }
    DateTime AdapterReceivedTime        { get; }

    uint UpdateSequenceId { get; }
}

public interface IMutableFeedEventStatusUpdate : IFeedEventStatusUpdate, ICloneable<IMutableFeedEventStatusUpdate>, IScopedDiscreetUpdatable
{
    new FeedConnectivityStatusFlags FeedMarketConnectivityStatus { get; set; }

    new FeedSyncStatus FeedSyncStatus { get; set; }

    new DateTime ClientReceivedTime         { get; set; }
    new DateTime InboundSocketReceivingTime { get; set; }
    new DateTime InboundProcessedTime       { get; set; }
    new DateTime SubscriberDispatchedTime   { get; set; }
    new DateTime AdapterSentTime            { get; set; }
    new DateTime AdapterReceivedTime        { get; set; }

    new uint UpdateSequenceId { get; set; }

    new IMutableFeedEventStatusUpdate Clone();
}
