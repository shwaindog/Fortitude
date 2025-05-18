using System.Text.Unicode;
using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;

namespace FortitudeMarkets.Pricing.FeedEvents;

public abstract class FeedEventStatusUpdate : ReusableObject<IFeedEventStatusUpdate>, IMutableFeedEventStatusUpdate
{
    protected FeedEventStatusUpdate() { }

    protected FeedEventStatusUpdate(FeedSyncStatus feedSyncStatus, FeedConnectivityStatusFlags feedConnectivityStatus)
    {
        FeedSyncStatus = feedSyncStatus;

        FeedMarketConnectivityStatus = feedConnectivityStatus;
    }

    protected FeedEventStatusUpdate(IFeedEventStatusUpdate toClone)
    {
        FeedSyncStatus = toClone.FeedSyncStatus;

        FeedMarketConnectivityStatus = toClone.FeedMarketConnectivityStatus;

        ClientReceivedTime         = toClone.ClientReceivedTime;
        InboundSocketReceivingTime = toClone.InboundSocketReceivingTime;
        InboundProcessedTime       = toClone.InboundProcessedTime;
        SubscriberDispatchedTime   = toClone.SubscriberDispatchedTime;
        AdapterReceivedTime        = toClone.AdapterReceivedTime;
        AdapterSentTime            = toClone.AdapterSentTime;
    }

    public FeedConnectivityStatusFlags FeedMarketConnectivityStatus { get; set; }
        = FeedConnectivityStatusFlagsExtensions.ClientDefaultConnectionState;


    public FeedSyncStatus FeedSyncStatus { get; set; } = FeedSyncStatus.NotStarted;

    public DateTime ClientReceivedTime         { get; set; }
    public DateTime InboundSocketReceivingTime { get; set; }
    public DateTime InboundProcessedTime       { get; set; }
    public DateTime SubscriberDispatchedTime   { get; set; }
    public DateTime AdapterSentTime            { get; set; }
    public DateTime AdapterReceivedTime        { get; set; }

    public abstract override FeedEventStatusUpdate Clone();

    IFeedEventStatusUpdate ICloneable<IFeedEventStatusUpdate>.              Clone() => Clone();
    IMutableFeedEventStatusUpdate ICloneable<IMutableFeedEventStatusUpdate>.Clone() => Clone();

    IMutableFeedEventStatusUpdate IMutableFeedEventStatusUpdate.Clone() => Clone();

    public override FeedEventStatusUpdate CopyFrom(IFeedEventStatusUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        FeedSyncStatus               = source.FeedSyncStatus;
        FeedMarketConnectivityStatus = source.FeedMarketConnectivityStatus;

        ClientReceivedTime         = source.ClientReceivedTime;
        InboundSocketReceivingTime = source.InboundSocketReceivingTime;
        SubscriberDispatchedTime   = source.SubscriberDispatchedTime;
        AdapterReceivedTime        = source.AdapterReceivedTime;
        AdapterSentTime            = source.AdapterSentTime;

        return this;
    }

    public override void StateReset()
    {
        ClientReceivedTime         = DateTime.MinValue;
        InboundSocketReceivingTime = DateTime.MinValue;
        SubscriberDispatchedTime   = DateTime.MinValue;
        AdapterReceivedTime        = DateTime.MinValue;
        AdapterSentTime            = DateTime.MinValue;

        FeedSyncStatus               = FeedSyncStatus.NotStarted;
        FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.AwaitingConnectionStart;

        base.StateReset();
    }

    public bool AreEquivalent(IFeedEventStatusUpdate? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && GetType() != other.GetType()) return false;

        var feedSyncSame       = FeedSyncStatus == other.FeedSyncStatus;
        var connectStatusSame  = FeedMarketConnectivityStatus == other.FeedMarketConnectivityStatus;
        var clientReceivedSame = true;
        var inboundSocketRecvSame = true;
        var subrDispatchSame = true;
        var adptrRecvSame = true;
        var adptrSentSame = true;
        if (exactTypes)
        {
            clientReceivedSame    = ClientReceivedTime == other.ClientReceivedTime;
            inboundSocketRecvSame = InboundSocketReceivingTime == other.InboundSocketReceivingTime;
            subrDispatchSame      = SubscriberDispatchedTime == other.SubscriberDispatchedTime;
            adptrRecvSame         = AdapterReceivedTime == other.AdapterReceivedTime;
            adptrSentSame         = AdapterSentTime == other.AdapterSentTime;
        }
        var allAreSame = feedSyncSame && connectStatusSame && clientReceivedSame && inboundSocketRecvSame && subrDispatchSame && adptrRecvSame && adptrSentSame;

        return allAreSame;
    }

    public override bool Equals(object? other) => ReferenceEquals(this, other) || AreEquivalent(other as IFeedEventStatusUpdate, true);

    
    protected string AllFeedEventStatusToStringMembers =>
        $"{nameof(FeedMarketConnectivityStatus)}: {FeedMarketConnectivityStatus}, {nameof(FeedSyncStatus)}: {FeedSyncStatus}, {nameof(ClientReceivedTime)}: " +
        $"{ClientReceivedTime:O}, {nameof(InboundSocketReceivingTime)}: {InboundSocketReceivingTime:O}, {nameof(InboundProcessedTime)}: {InboundProcessedTime:O}, " +
        $"{nameof(SubscriberDispatchedTime)}: {SubscriberDispatchedTime:O}, {nameof(AdapterSentTime)}: {AdapterSentTime:O}, " +
        $"{nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}";

    protected string JustFeedSyncConnectivityStatusToStringMembers =>
        $"{nameof(FeedMarketConnectivityStatus)}: {FeedMarketConnectivityStatus}, {nameof(FeedSyncStatus)}: {FeedSyncStatus}";

    public override int GetHashCode()
    {
        return HashCode.Combine(FeedMarketConnectivityStatus, (int)FeedSyncStatus);
    }

    public override string ToString() => $"{nameof(FeedMarketConnectivityStatus)}{{{AllFeedEventStatusToStringMembers}}}";
}

public static class FeedEventStatusUpdateExtensions
{
    public static bool IsSourceReplay(this IMutableFeedEventStatusUpdate feedUpdate) => feedUpdate.FeedMarketConnectivityStatus.HasIsSourceReplay();

    public static bool IsAdapterReplay(this IMutableFeedEventStatusUpdate feedUpdate) => feedUpdate.FeedMarketConnectivityStatus.HasIsAdapterReplay();

    public static IMutableFeedEventStatusUpdate WithIsSourceReplay(this IMutableFeedEventStatusUpdate feedUpdate, bool newSourceReplay)
    {
        var toToggle = FeedConnectivityStatusFlags.IsSourceReplay;
        var flags    = feedUpdate.FeedMarketConnectivityStatus;
        feedUpdate.FeedMarketConnectivityStatus =  newSourceReplay ?  flags.Set(toToggle) : flags.Unset(toToggle);

        return feedUpdate;
    }

    public static IMutableFeedEventStatusUpdate WithIsAdapterReplay(this IMutableFeedEventStatusUpdate feedUpdate, bool newAdapterReplay)
    {
        var toToggle = FeedConnectivityStatusFlags.IsAdapterReplay;
        var flags    = feedUpdate.FeedMarketConnectivityStatus;
        feedUpdate.FeedMarketConnectivityStatus =  newAdapterReplay ?  flags.Set(toToggle) : flags.Unset(toToggle);
        return feedUpdate;
    }

    public static bool FromSourceSnapshot
        (this IMutableFeedEventStatusUpdate feedUpdate) =>
        feedUpdate.FeedMarketConnectivityStatus.HasFromSourceSnapshot();

    public static bool FromAdapterSnapshot
        (this IMutableFeedEventStatusUpdate feedUpdate) =>
        feedUpdate.FeedMarketConnectivityStatus.HasFromSourceSnapshot();
}
