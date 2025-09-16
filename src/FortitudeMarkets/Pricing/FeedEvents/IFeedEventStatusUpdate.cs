using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StyledToString;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

namespace FortitudeMarkets.Pricing.FeedEvents;

public interface IFeedEventStatusUpdate : IReusableObject<IFeedEventStatusUpdate>, IInterfacesComparable<IFeedEventStatusUpdate>
  , IPartialSequenceUpdates, ICanHaveSourceTickerDefinition, IStyledToStringObject
{
    FeedConnectivityStatusFlags FeedMarketConnectivityStatus { get; }

    FeedSyncStatus FeedSyncStatus { get; }

    PublishableQuoteInstantBehaviorFlags QuoteBehavior { get; }

    DateTime ClientReceivedTime         { get; }
    DateTime InboundSocketReceivingTime { get; }
    DateTime InboundProcessedTime       { get; }
    DateTime SubscriberDispatchedTime   { get; }
    DateTime AdapterSentTime            { get; }
    DateTime AdapterReceivedTime        { get; }
}

public interface IMutableFeedEventStatusUpdate : IFeedEventStatusUpdate, ICloneable<IMutableFeedEventStatusUpdate>
  , IScopedTimedUpdatable, IMutableCanHaveSourceTickerDefinition
{
    new FeedConnectivityStatusFlags FeedMarketConnectivityStatus { get; set; }

    new FeedSyncStatus FeedSyncStatus { get; set; }

    new PublishableQuoteInstantBehaviorFlags QuoteBehavior { get; set; }

    new DateTime ClientReceivedTime         { get; set; }
    new DateTime InboundSocketReceivingTime { get; set; }
    new DateTime InboundProcessedTime       { get; set; }
    new DateTime SubscriberDispatchedTime   { get; set; }
    new DateTime AdapterSentTime            { get; set; }
    new DateTime AdapterReceivedTime        { get; set; }

    new IMutableFeedEventStatusUpdate Clone();
}
