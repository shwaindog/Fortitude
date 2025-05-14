using FortitudeCommon.Types;

namespace FortitudeMarkets.Pricing.FeedEvents;

public interface IFeedEventStatusUpdate : IInterfacesComparable<IFeedEventStatusUpdate>
{
    DateTime       ClientReceivedTime { get; }
    FeedSyncStatus FeedSyncStatus     { get; }
}

public interface IMutableFeedEventStatusUpdate : IFeedEventStatusUpdate
{
    new DateTime       ClientReceivedTime { get; set; }
    new FeedSyncStatus FeedSyncStatus     { get; set; }
}