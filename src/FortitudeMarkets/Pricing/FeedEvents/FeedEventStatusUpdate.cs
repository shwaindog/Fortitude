using FortitudeCommon.DataStructures.Memory;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StyledToString;
using FortitudeCommon.Types.StyledToString.StyledTypes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;

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
        FeedSyncStatus   = toClone.FeedSyncStatus;
        IsCompleteUpdate = toClone.IsCompleteUpdate;
        UpdateSequenceId = toClone.UpdateSequenceId;

        FeedMarketConnectivityStatus = toClone.FeedMarketConnectivityStatus;

        ClientReceivedTime         = toClone.ClientReceivedTime;
        InboundSocketReceivingTime = toClone.InboundSocketReceivingTime;
        InboundProcessedTime       = toClone.InboundProcessedTime;
        SubscriberDispatchedTime   = toClone.SubscriberDispatchedTime;
        AdapterReceivedTime        = toClone.AdapterReceivedTime;
        AdapterSentTime            = toClone.AdapterSentTime;

        QuoteBehavior = toClone.QuoteBehavior;
    }

    public FeedConnectivityStatusFlags FeedMarketConnectivityStatus { get; set; }
        = FeedConnectivityStatusFlagsExtensions.ClientDefaultConnectionState;

    public abstract ISourceTickerInfo? SourceTickerInfo { get; set; }

    public FeedSyncStatus FeedSyncStatus { get; set; } = FeedSyncStatus.NotStarted;

    public DateTime ClientReceivedTime         { get; set; }
    public DateTime InboundSocketReceivingTime { get; set; }
    public DateTime InboundProcessedTime       { get; set; }
    public DateTime SubscriberDispatchedTime   { get; set; }
    public DateTime AdapterSentTime            { get; set; }
    public DateTime AdapterReceivedTime        { get; set; }

    public virtual PublishableQuoteInstantBehaviorFlags QuoteBehavior { get; set; }

    public bool IsCompleteUpdate { get; set; }

    public uint UpdateSequenceId { get; set; }

    public abstract override FeedEventStatusUpdate Clone();

    IFeedEventStatusUpdate ICloneable<IFeedEventStatusUpdate>.Clone() => Clone();

    IMutableFeedEventStatusUpdate ICloneable<IMutableFeedEventStatusUpdate>.Clone() => Clone();

    IMutableFeedEventStatusUpdate IMutableFeedEventStatusUpdate.Clone() => Clone();

    public override FeedEventStatusUpdate CopyFrom(IFeedEventStatusUpdate source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (QuoteBehavior.HasInheritAdditionalPublishedFlagsFlag())
        {
            QuoteBehavior |= source.QuoteBehavior;
        }
        var originalSequenceId = UpdateSequenceId;
        UpdateComplete(originalSequenceId);
        UpdateStarted(source.UpdateSequenceId);

        FeedSyncStatus   = source.FeedSyncStatus;
        IsCompleteUpdate = source.IsCompleteUpdate;

        FeedMarketConnectivityStatus = source.FeedMarketConnectivityStatus;

        ClientReceivedTime         = source.ClientReceivedTime;
        InboundSocketReceivingTime = source.InboundSocketReceivingTime;
        SubscriberDispatchedTime   = source.SubscriberDispatchedTime;
        AdapterReceivedTime        = source.AdapterReceivedTime;
        AdapterSentTime            = source.AdapterSentTime;

        // Responsibility
        // UpdatesAppliedAllDeltas(originalSequenceId, UpdateSequenceId);

        return this;
    }

    public virtual void UpdateComplete(uint updateSequenceId = 0)
    {
        FeedMarketConnectivityStatus &= ~(FeedConnectivityStatusFlags.FromAdapterSnapshot | FeedConnectivityStatusFlags.IsAdapterReplay);
    }

    public virtual void TriggerTimeUpdates(DateTime atDateTime)
    {
    }


    public virtual void UpdateStarted(uint updateSequenceId)
    {
        UpdateSequenceId = updateSequenceId;
    }

    public virtual uint UpdateCount => UpdateSequenceId;

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

        var feedSyncSame          = FeedSyncStatus == other.FeedSyncStatus;
        var connectStatusSame     = FeedMarketConnectivityStatus == other.FeedMarketConnectivityStatus;
        var clientReceivedSame    = true;
        var inboundSocketRecvSame = true;
        var dispatchSame      = true;
        var adapterRecvSame         = true;
        var adapterSentSame         = true;
        if (exactTypes)
        {
            clientReceivedSame    = ClientReceivedTime == other.ClientReceivedTime;
            inboundSocketRecvSame = InboundSocketReceivingTime == other.InboundSocketReceivingTime;
            dispatchSame      = SubscriberDispatchedTime == other.SubscriberDispatchedTime;
            adapterRecvSame         = AdapterReceivedTime == other.AdapterReceivedTime;
            adapterSentSame         = AdapterSentTime == other.AdapterSentTime;
        }
        var allAreSame = feedSyncSame && connectStatusSame && clientReceivedSame && inboundSocketRecvSame 
                      && dispatchSame && adapterRecvSame && adapterSentSame;

        return allAreSame;
    }

    public override bool Equals(object? other) => ReferenceEquals(this, other) || AreEquivalent(other as IFeedEventStatusUpdate, true);

    protected string AllFeedEventStatusToStringMembers =>
        $"{nameof(ClientReceivedTime)}: {ClientReceivedTime:O}, {nameof(InboundSocketReceivingTime)}: {InboundSocketReceivingTime:O}, " +
        $"{nameof(InboundProcessedTime)}: {InboundProcessedTime:O}, {nameof(SubscriberDispatchedTime)}: {SubscriberDispatchedTime:O}, " +
        $"{nameof(AdapterSentTime)}: {AdapterSentTime:O}, {nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}, {nameof(IsCompleteUpdate)}: {IsCompleteUpdate}";

    protected string JustFeedSyncConnectivityStatusToStringMembers =>
        $"{nameof(FeedMarketConnectivityStatus)}: {FeedMarketConnectivityStatus}, {nameof(FeedSyncStatus)}: {FeedSyncStatus}";

    public override int GetHashCode()
    {
        return HashCode.Combine(FeedMarketConnectivityStatus, (int)FeedSyncStatus);
    }

    public virtual StyledTypeBuildResult ToString(IStyledTypeStringAppender stsa) => 
        stsa.StartComplexType(this)
            .Field.AlwaysAdd(nameof(FeedMarketConnectivityStatus), FeedMarketConnectivityStatus)
            .Field.AlwaysAdd(nameof(FeedSyncStatus), FeedSyncStatus)
            .Complete();

    public override string ToString() => $"{nameof(FeedMarketConnectivityStatus)}{{{JustFeedSyncConnectivityStatusToStringMembers}, {AllFeedEventStatusToStringMembers}}}";
}

public static class FeedEventStatusUpdateExtensions
{
    public static bool IsSourceReplay(this IMutableFeedEventStatusUpdate feedUpdate) => feedUpdate.FeedMarketConnectivityStatus.HasIsSourceReplay();

    public static bool IsAdapterReplay(this IMutableFeedEventStatusUpdate feedUpdate) => feedUpdate.FeedMarketConnectivityStatus.HasIsAdapterReplay();

    public static IMutableFeedEventStatusUpdate WithIsSourceReplay(this IMutableFeedEventStatusUpdate feedUpdate, bool newSourceReplay)
    {
        var toToggle = FeedConnectivityStatusFlags.IsSourceReplay;
        var flags    = feedUpdate.FeedMarketConnectivityStatus;
        feedUpdate.FeedMarketConnectivityStatus = newSourceReplay ? flags.Set(toToggle) : flags.Unset(toToggle);

        return feedUpdate;
    }

    public static IMutableFeedEventStatusUpdate WithIsAdapterReplay(this IMutableFeedEventStatusUpdate feedUpdate, bool newAdapterReplay)
    {
        var toToggle = FeedConnectivityStatusFlags.IsAdapterReplay;
        var flags    = feedUpdate.FeedMarketConnectivityStatus;
        feedUpdate.FeedMarketConnectivityStatus = newAdapterReplay ? flags.Set(toToggle) : flags.Unset(toToggle);
        return feedUpdate;
    }

    public static bool FromSourceSnapshot
        (this IMutableFeedEventStatusUpdate feedUpdate) =>
        feedUpdate.FeedMarketConnectivityStatus.HasFromSourceSnapshot();

    public static bool FromAdapterSnapshot
        (this IMutableFeedEventStatusUpdate feedUpdate) =>
        feedUpdate.FeedMarketConnectivityStatus.HasFromSourceSnapshot();
}
