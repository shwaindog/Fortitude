// Licensed under the MIT license.
// Copyright Alexis Sawenko 2025 all rights reserved

using System.Text.Json.Serialization;
using FortitudeCommon.AsyncProcessing;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeIO.Protocols;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using PQMessageFlags = FortitudeMarkets.Pricing.PQ.Serdes.Serialization.PQMessageFlags;

namespace FortitudeMarkets.Pricing.PQ.Messages;

[Flags]
public enum PQMessageUpdatedFlags : uint
{
    None                                  = 0x00_00_00
  , FeedSyncStatusFlag                    = 0x00_00_01
  , QuoteBehaviorUpdatedFlag              = 0x00_00_02
  , FeedBehaviorUpdatedFlag               = 0x00_00_04
  , FeedConnectivityStatusUpdatedFlag     = 0x00_00_08
  , SocketReceivedDateUpdatedFlag         = 0x00_00_10
  , SocketReceivedSub2MinUpdatedFlag      = 0x00_00_20
  , ProcessedDateUpdatedFlag              = 0x00_00_40
  , ProcessedSub2MinUpdatedFlag           = 0x00_00_80
  , DispatchedDateUpdatedFlag             = 0x00_01_00
  , DispatchedSub2MinUpdatedFlag          = 0x00_02_00
  , ClientReceivedDateUpdatedFlag         = 0x00_04_00
  , ClientReceivedSub2MinUpdatedFlag      = 0x00_08_00
  , AdapterReceivedTimeDateUpdatedFlag    = 0x00_10_00
  , AdapterReceivedTimeSub2MinUpdatedFlag = 0x00_20_00
  , AdapterSentTimeDateUpdatedFlag        = 0x00_40_00
  , AdapterSentTimeSub2MinUpdatedFlag     = 0x00_80_00
}

public abstract class PQReusableMessage : ReusableObject<IFeedEventStatusUpdate>, IPQMessage
{
    private FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good;

    protected readonly ISyncLock SyncLock = new SpinLockLight();

    protected DateTime InboundProcessedTimeField;
    protected DateTime InboundSocketReceivingTimeField;
    protected DateTime ClientReceivedTimeField;
    protected DateTime DispatchedTimeField;
    protected DateTime AdapterReceivedTimeField;
    protected DateTime AdapterSentTimeField;

    protected PQMessageUpdatedFlags                MessageUpdatedFlags;
    private   FeedConnectivityStatusFlags          feedMarketConnectivityStatus;
    private   PublishableQuoteInstantBehaviorFlags quoteBehavior;

    protected PQReusableMessage() { }

    protected PQReusableMessage(IFeedEventStatusUpdate? toClone)
    {
        if (toClone == null) return;
        PQSequenceId   = toClone.UpdateSequenceId;
        FeedSyncStatus = toClone.FeedSyncStatus;

        SubscriberDispatchedTime   = toClone.SubscriberDispatchedTime;
        InboundProcessedTime       = toClone.InboundProcessedTime;
        InboundSocketReceivingTime = toClone.InboundSocketReceivingTime;
        ClientReceivedTime         = toClone.ClientReceivedTime;
        AdapterSentTime            = toClone.AdapterSentTime;
        AdapterReceivedTime        = toClone.AdapterReceivedTime;
    }

    protected PQReusableMessage(IPQMessage? toClone)
    {
        if (toClone == null) return;
        PQSequenceId   = toClone.PQSequenceId;
        FeedSyncStatus = toClone.FeedSyncStatus;

        LastPublicationTime        = toClone.LastPublicationTime;
        SubscriberDispatchedTime   = toClone.SubscriberDispatchedTime;
        InboundProcessedTime       = toClone.InboundProcessedTime;
        InboundSocketReceivingTime = toClone.InboundSocketReceivingTime;
        ClientReceivedTime         = toClone.ClientReceivedTime;
        AdapterSentTime            = toClone.AdapterSentTime;
        AdapterReceivedTime        = toClone.AdapterReceivedTime;

        OverrideSerializationFlags = toClone.OverrideSerializationFlags;
        SetFlagsSame(toClone);
    }

    public FeedEvents.Quotes.PQMessageFlags? OverrideSerializationFlags { get; set; }

    public abstract uint MessageId { get; }

    ISourceTickerInfo? ICanHaveSourceTickerDefinition.SourceTickerInfo => SourceTickerInfo;

    ISourceTickerInfo? IMutableCanHaveSourceTickerDefinition.SourceTickerInfo
    {
        get => SourceTickerInfo;
        set => SourceTickerInfo = (IPQSourceTickerInfo?)value;
    }

    public abstract IPQSourceTickerInfo? SourceTickerInfo { get; set; }

    public virtual uint StreamId => SourceTickerInfo!.SourceInstrumentId;

    public virtual string StreamName => SourceTickerInfo!.InstrumentName;

    public uint PQSequenceId { get; set; }

    public ISyncLock Lock => SyncLock;

    public DateTime LastPublicationTime { get; set; }

    public virtual byte Version => 1;

    public FeedConnectivityStatusFlags FeedMarketConnectivityStatus
    {
        get => feedMarketConnectivityStatus;
        set
        {
            if (feedMarketConnectivityStatus == value) return;
            IsFeedConnectivityStatusUpdated = true;
            feedMarketConnectivityStatus    = value;
        }
    }

    public DateTime SubscriberDispatchedTime
    {
        get => DispatchedTimeField;
        set
        {
            if (DispatchedTimeField == value) return;
            IsDispatchedTimeDateUpdated
                |= DispatchedTimeField.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || PQSequenceId == 0;
            IsDispatchedTimeSub2MinUpdated |= DispatchedTimeField.GetSub2MinComponent() != value.GetSub2MinComponent() || PQSequenceId == 0;
            DispatchedTimeField            =  value == DateTime.UnixEpoch ? default : value;
        }
    }

    public DateTime InboundProcessedTime
    {
        get => InboundProcessedTimeField;
        set
        {
            if (InboundProcessedTimeField == value) return;
            IsProcessedTimeDateUpdated
                |= InboundProcessedTimeField.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || PQSequenceId == 0;
            IsProcessedTimeSub2MinUpdated |= InboundProcessedTimeField.GetSub2MinComponent() != value.GetSub2MinComponent() || PQSequenceId == 0;
            InboundProcessedTimeField     =  value == DateTime.UnixEpoch ? default : value;
        }
    }

    public DateTime InboundSocketReceivingTime
    {
        get => InboundSocketReceivingTimeField;
        set
        {
            IsSocketReceivedTimeDateUpdated
                |= InboundSocketReceivingTimeField.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || PQSequenceId == 0;
            IsSocketReceivedTimeSub2MinUpdated
                |= InboundSocketReceivingTimeField.GetSub2MinComponent() != value.GetSub2MinComponent() || PQSequenceId == 0;
            InboundSocketReceivingTimeField = value == DateTime.UnixEpoch ? default : value;
        }
    }

    public DateTime ClientReceivedTime
    {
        get => ClientReceivedTimeField;
        set
        {
            IsClientReceivedTimeDateUpdated
                |= ClientReceivedTimeField.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || PQSequenceId == 0;
            IsClientReceivedTimeSub2MinUpdated
                |= ClientReceivedTimeField.GetSub2MinComponent() != value.GetSub2MinComponent() || PQSequenceId == 0;
            ClientReceivedTimeField = value == DateTime.UnixEpoch ? default : value;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime AdapterSentTime
    {
        get => AdapterSentTimeField;
        set
        {
            IsAdapterSentTimeDateUpdated |= AdapterSentTimeField.Get2MinIntervalsFromUnixEpoch()
             != value.Get2MinIntervalsFromUnixEpoch() || PQSequenceId == 0;
            IsAdapterSentTimeSub2MinUpdated |= AdapterSentTimeField.GetSub2MinComponent()
             != value.GetSub2MinComponent() || PQSequenceId == 0;
            AdapterSentTimeField = value == DateTime.UnixEpoch ? default : value;
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime AdapterReceivedTime
    {
        get => AdapterReceivedTimeField;
        set
        {
            IsAdapterReceivedTimeDateUpdated |= AdapterReceivedTimeField.Get2MinIntervalsFromUnixEpoch()
             != value.Get2MinIntervalsFromUnixEpoch() || PQSequenceId == 0;
            IsAdapterReceivedTimeSub2MinUpdated |= AdapterReceivedTimeField.GetSub2MinComponent()
             != value.GetSub2MinComponent() || PQSequenceId == 0;
            AdapterReceivedTimeField = value == DateTime.UnixEpoch ? default : value;
        }
    }

    public FeedSyncStatus FeedSyncStatus
    {
        get => feedSyncStatus;
        set
        {
            if (feedSyncStatus == value) return;
            IsFeedSyncStatusUpdated = true;
            feedSyncStatus          = value;
        }
    }

    public virtual PublishableQuoteInstantBehaviorFlags QuoteBehavior
    {
        get => quoteBehavior;
        set
        {
            if (quoteBehavior == value) return;
            IsQuoteBehaviorFlagsUpdated = true;
            quoteBehavior               = value;
        }
    }

    public bool IsQuoteBehaviorFlagsUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.QuoteBehaviorUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.QuoteBehaviorUpdatedFlag;

            else if (IsQuoteBehaviorFlagsUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.QuoteBehaviorUpdatedFlag;
        }
    }

    public bool IsFeedBehaviorFlagsUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.FeedBehaviorUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.FeedBehaviorUpdatedFlag;

            else if (IsFeedBehaviorFlagsUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.FeedBehaviorUpdatedFlag;
        }
    }

    public bool IsFeedConnectivityStatusUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.FeedConnectivityStatusUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.FeedConnectivityStatusUpdatedFlag;

            else if (IsFeedConnectivityStatusUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.FeedConnectivityStatusUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSocketReceivedTimeDateUpdated
    {
        get =>
            !QuoteBehavior.HasNoClientInboundSocketTimeUpdatesFlag()
         && (MessageUpdatedFlags & PQMessageUpdatedFlags.SocketReceivedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.SocketReceivedDateUpdatedFlag;

            else if (IsSocketReceivedTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.SocketReceivedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSocketReceivedTimeSub2MinUpdated
    {
        get =>
            !QuoteBehavior.HasNoClientInboundSocketTimeUpdatesFlag()
         && (MessageUpdatedFlags & PQMessageUpdatedFlags.SocketReceivedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.SocketReceivedSub2MinUpdatedFlag;

            else if (IsSocketReceivedTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.SocketReceivedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsProcessedTimeDateUpdated
    {
        get => !QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && (MessageUpdatedFlags & PQMessageUpdatedFlags.ProcessedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.ProcessedDateUpdatedFlag;

            else if (IsProcessedTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.ProcessedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsProcessedTimeSub2MinUpdated
    {
        get => !QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && (MessageUpdatedFlags & PQMessageUpdatedFlags.ProcessedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.ProcessedSub2MinUpdatedFlag;

            else if (IsProcessedTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.ProcessedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsDispatchedTimeDateUpdated
    {
        get => !QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && (MessageUpdatedFlags & PQMessageUpdatedFlags.DispatchedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.DispatchedDateUpdatedFlag;

            else if (IsDispatchedTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.DispatchedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsDispatchedTimeSub2MinUpdated
    {
        get => !QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && (MessageUpdatedFlags & PQMessageUpdatedFlags.DispatchedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.DispatchedSub2MinUpdatedFlag;

            else if (IsDispatchedTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.DispatchedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsClientReceivedTimeDateUpdated
    {
        get => !QuoteBehavior.HasNoClientReceiveTimeUpdatesFlag() && (MessageUpdatedFlags & PQMessageUpdatedFlags.ClientReceivedDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.ClientReceivedDateUpdatedFlag;

            else if (IsClientReceivedTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.ClientReceivedDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsClientReceivedTimeSub2MinUpdated
    {
        get =>
            !QuoteBehavior.HasNoClientReceiveTimeUpdatesFlag()
         && (MessageUpdatedFlags & PQMessageUpdatedFlags.ClientReceivedSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.ClientReceivedSub2MinUpdatedFlag;

            else if (IsClientReceivedTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.ClientReceivedSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterSentTimeDateUpdated
    {
        get =>
            !QuoteBehavior.HasNoAdapterSentTimeUpdatesFlag()
         && (MessageUpdatedFlags & PQMessageUpdatedFlags.AdapterSentTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.AdapterSentTimeDateUpdatedFlag;

            else if (IsAdapterSentTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.AdapterSentTimeDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterSentTimeSub2MinUpdated
    {
        get =>
            !QuoteBehavior.HasNoAdapterSentTimeUpdatesFlag()
         && (MessageUpdatedFlags & PQMessageUpdatedFlags.AdapterSentTimeSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.AdapterSentTimeSub2MinUpdatedFlag;

            else if (IsAdapterSentTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.AdapterSentTimeSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterReceivedTimeDateUpdated
    {
        get =>
            !QuoteBehavior.HasNoAdapterReceiveTimeUpdatesFlag()
         && (MessageUpdatedFlags & PQMessageUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag;

            else if (IsAdapterReceivedTimeDateUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.AdapterReceivedTimeDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsAdapterReceivedTimeSub2MinUpdated
    {
        get =>
            !QuoteBehavior.HasNoAdapterReceiveTimeUpdatesFlag()
         && (MessageUpdatedFlags & PQMessageUpdatedFlags.AdapterReceivedTimeSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.AdapterReceivedTimeSub2MinUpdatedFlag;

            else if (IsAdapterReceivedTimeSub2MinUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.AdapterReceivedTimeSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsFeedSyncStatusUpdated
    {
        get => (MessageUpdatedFlags & PQMessageUpdatedFlags.FeedSyncStatusFlag) > 0;
        set
        {
            if (value)
                MessageUpdatedFlags |= PQMessageUpdatedFlags.FeedSyncStatusFlag;

            else if (IsFeedSyncStatusUpdated) MessageUpdatedFlags ^= PQMessageUpdatedFlags.FeedSyncStatusFlag;
        }
    }

    [JsonIgnore] IPQMessage? IDoublyLinkedListNode<IPQMessage>.Previous { get; set; }
    [JsonIgnore] IPQMessage? IDoublyLinkedListNode<IPQMessage>.Next { get; set; }

    [JsonIgnore]
    public IPQMessage? Previous
    {
        get => GetPrevious<IPQMessage?>();
        set => SetPrevious(value);
    }
    [JsonIgnore]
    public IPQMessage? Next
    {
        get => GetNext<IPQMessage?>();
        set => SetNext(value);
    }

    protected T? GetPrevious<T>()
    {
        return (T?)(((IDoublyLinkedListNode<IPQMessage>)this).Previous);
    }

    protected void SetPrevious<T>(T value)
    {
        ((IDoublyLinkedListNode<IPQMessage>)this).Previous = (IPQMessage?)value;
    }

    protected T? GetNext<T>()
    {
        return (T?)(((IDoublyLinkedListNode<IPQMessage>)this).Next);
    }

    protected void SetNext<T>(T value)
    {
        ((IDoublyLinkedListNode<IPQMessage>)this).Next = (IPQMessage?)value;
    }

    public virtual bool HasUpdates
    {
        get => MessageUpdatedFlags != PQMessageUpdatedFlags.None;
        set
        {
            if (value) return;
            MessageUpdatedFlags = PQMessageUpdatedFlags.None;
        }
    }

    public virtual bool IsEmpty
    {
        get =>
            ClientReceivedTimeField == DateTime.MinValue
         && DispatchedTimeField == DateTime.MinValue
         && InboundProcessedTimeField == DateTime.MinValue
         && InboundSocketReceivingTimeField == DateTime.MinValue
         && AdapterSentTimeField == DateTime.MinValue
         && AdapterReceivedTimeField == DateTime.MinValue
         && OverrideSerializationFlags == null
         && FeedSyncStatus == FeedSyncStatus.NotStarted
         && FeedMarketConnectivityStatus == FeedConnectivityStatusFlags.None
         && PQSequenceId == 0;
        set
        {
            if (!value) return;
            ResetWithTracking();
        }
    }

    public uint UpdateSequenceId
    {
        get => PQSequenceId;
        set => PQSequenceId = value;
    }

    public bool IsCompleteUpdate { get; set; }

    public virtual void UpdateStarted(uint updateSequenceId = 0)
    {
        PQSequenceId = updateSequenceId;
    }

    public virtual void TriggerTimeUpdates(DateTime atDateTime) { }


    public virtual void UpdateComplete(uint updateSequenceId = 0)
    {
        FeedMarketConnectivityStatus &= ~(FeedConnectivityStatusFlags.FromAdapterSnapshot | FeedConnectivityStatusFlags.IsAdapterReplay);
    }

    public virtual IPQMessage ResetWithTracking()
    {
        OverrideSerializationFlags = null;

        ClientReceivedTimeField         = default;
        LastPublicationTime             = default;
        InboundSocketReceivingTimeField = default;
        InboundProcessedTimeField       = default;
        AdapterReceivedTimeField        = default;
        AdapterSentTimeField            = default;
        DispatchedTimeField             = default;

        FeedMarketConnectivityStatus = FeedConnectivityStatusFlags.None;

        PQSequenceId   = 0;
        feedSyncStatus = FeedSyncStatus.Good;
        return this;
    }

    public override void StateReset()
    {
        ResetWithTracking();
        HasUpdates = false;
        base.StateReset();
    }

    public abstract IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, PQMessageFlags messageFlags);

    public abstract bool UpdateFieldString(PQFieldStringUpdate stringUpdate);

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, PQMessageFlags messageFlags)
    {
        var fullPicture = (messageFlags & PQMessageFlags.Complete) > 0;
        // only copy if changed

        if (fullPicture || IsQuoteBehaviorFlagsUpdated)
        {
            if (QuoteBehavior.HasPublishPublishableQuoteInstantBehaviorFlagsFlag())
            {
                yield return new PQFieldUpdate(PQFeedFields.InstantQuoteBehaviorFlags, (uint)QuoteBehavior);
            }
            else if (QuoteBehavior.HasPublishQuoteInstantBehaviorFlagsFlag() && !QuoteBehavior.HasSuppressPublishOriginalQuoteFlagsFlag())
            {
                yield return new PQFieldUpdate(PQFeedFields.InstantQuoteBehaviorFlags, (uint)(QuoteBehavior.ToQuoteInstantBehaviorMask()));
            }
        }
        if (fullPicture || IsFeedConnectivityStatusUpdated)
            yield return new PQFieldUpdate(PQFeedFields.FeedMarketConnectivityStatus, (uint)FeedMarketConnectivityStatus);
        if (!FeedMarketConnectivityStatus.HasIsAdapterReplay()) AdapterSentTime = snapShotTime;
        if (!QuoteBehavior.HasNoAdapterSentTimeUpdatesFlag() && fullPicture || IsAdapterSentTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.AdapterSentDateTime, AdapterSentTimeField.Get2MinIntervalsFromUnixEpoch());
        if (!QuoteBehavior.HasNoAdapterSentTimeUpdatesFlag() && fullPicture || IsAdapterSentTimeSub2MinUpdated)
        {
            var extended = AdapterSentTimeField.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.AdapterSentSub2MinTime, value, extended);
        }
        if (!QuoteBehavior.HasNoAdapterReceiveTimeUpdatesFlag() && fullPicture || IsAdapterReceivedTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.AdapterReceivedDateTime, AdapterReceivedTimeField.Get2MinIntervalsFromUnixEpoch());
        if (!QuoteBehavior.HasNoAdapterReceiveTimeUpdatesFlag() && fullPicture || IsAdapterReceivedTimeSub2MinUpdated)
        {
            var extended = AdapterReceivedTimeField.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var value);
            yield return new PQFieldUpdate(PQFeedFields.AdapterReceivedSub2MinTime, value, extended);
        }
        var includeReceiverTimes = (messageFlags & PQMessageFlags.IncludeReceiverTimes) > 0;
        if (includeReceiverTimes)
        {
            if (fullPicture || IsFeedSyncStatusUpdated) yield return new PQFieldUpdate(PQFeedFields.PQSyncStatus, (byte)FeedSyncStatus);
            if (!QuoteBehavior.HasNoClientInboundSocketTimeUpdatesFlag() && fullPicture || IsSocketReceivedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ClientSocketReceivingDateTime
                                             , InboundSocketReceivingTime.Get2MinIntervalsFromUnixEpoch());

            if (!QuoteBehavior.HasNoClientInboundSocketTimeUpdatesFlag() && fullPicture || IsSocketReceivedTimeSub2MinUpdated)
            {
                var extended = InboundSocketReceivingTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFeedFields.ClientSocketReceivingSub2MinTime, lower4Bytes, extended);
            }

            if (!QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && fullPicture || IsProcessedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ClientProcessedDateTime, InboundProcessedTime.Get2MinIntervalsFromUnixEpoch());

            if (!QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && fullPicture || IsProcessedTimeSub2MinUpdated)
            {
                var extended = InboundProcessedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFeedFields.ClientProcessedSub2MinTime, lower4Bytes, extended);
            }

            if (!QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && fullPicture || IsDispatchedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ClientDispatchedDateTime, SubscriberDispatchedTime.Get2MinIntervalsFromUnixEpoch());

            if (!QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && fullPicture || IsDispatchedTimeSub2MinUpdated)
            {
                var extended = SubscriberDispatchedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFeedFields.ClientDispatchedSub2MinTime, lower4Bytes, extended);
            }

            if (!QuoteBehavior.HasNoClientReceiveTimeUpdatesFlag() && fullPicture || IsClientReceivedTimeDateUpdated)
                yield return new PQFieldUpdate(PQFeedFields.ClientReceivedDateTime
                                             , ClientReceivedTime.Get2MinIntervalsFromUnixEpoch());

            if (!QuoteBehavior.HasNoClientReceiveTimeUpdatesFlag() && fullPicture || IsClientReceivedTimeSub2MinUpdated)
            {
                var extended = ClientReceivedTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
                yield return new PQFieldUpdate(PQFeedFields.ClientReceivedSub2MinTime, lower4Bytes, extended);
            }
        }
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.PQSyncStatus:
                IsFeedSyncStatusUpdated = true; // in-case of reset and sending 0;
                FeedSyncStatus          = (FeedSyncStatus)pqFieldUpdate.Payload;
                return 0;
            case PQFeedFields.InstantQuoteBehaviorFlags:
                IsQuoteBehaviorFlagsUpdated = true; // in-case of reset and sending 0;

                if (QuoteBehavior.HasInheritAdditionalPublishedFlagsFlag())
                {
                    QuoteBehavior |= (PublishableQuoteInstantBehaviorFlags)pqFieldUpdate.Payload;
                }
                else if (QuoteBehavior.HasRestoreAndOverlayOriginalQuoteFlagsFlag())
                {
                    QuoteBehavior |= ((PublishableQuoteInstantBehaviorFlags)pqFieldUpdate.Payload).ToQuoteInstantBehaviorMask();
                }
                else if (QuoteBehavior.HasRestoreOriginalQuoteFlagsFlag())
                {
                    QuoteBehavior = QuoteBehavior.JustPublishableBehaviorMask() |
                                    ((PublishableQuoteInstantBehaviorFlags)pqFieldUpdate.Payload).ToQuoteInstantBehaviorMask();
                }
                return 0;
            case PQFeedFields.FeedMarketConnectivityStatus:
                IsFeedConnectivityStatusUpdated = true; // in-case of reset and sending 0;
                FeedMarketConnectivityStatus    = (FeedConnectivityStatusFlags)pqFieldUpdate.Payload;
                return 0;
            case PQFeedFields.AdapterSentDateTime:
                if (QuoteBehavior.HasNoAdapterSentTimeUpdatesFlag()) return 0;
                IsAdapterSentTimeDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref AdapterSentTimeField, pqFieldUpdate.Payload);
                if (AdapterSentTimeField == DateTime.UnixEpoch) AdapterSentTimeField = default;
                return 0;
            case PQFeedFields.AdapterSentSub2MinTime:
                if (QuoteBehavior.HasNoAdapterSentTimeUpdatesFlag()) return 0;
                IsAdapterSentTimeSub2MinUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref AdapterSentTimeField, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (AdapterSentTimeField == DateTime.UnixEpoch) AdapterSentTimeField = default;
                return 0;
            case PQFeedFields.AdapterReceivedDateTime:
                if (QuoteBehavior.HasNoAdapterReceiveTimeUpdatesFlag()) return 0;
                IsAdapterReceivedTimeDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref AdapterReceivedTimeField, pqFieldUpdate.Payload);
                if (AdapterReceivedTimeField == DateTime.UnixEpoch) AdapterReceivedTimeField = default;
                return 0;
            case PQFeedFields.AdapterReceivedSub2MinTime:
                if (QuoteBehavior.HasNoAdapterReceiveTimeUpdatesFlag()) return 0;
                IsAdapterReceivedTimeSub2MinUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref AdapterReceivedTimeField, pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (AdapterReceivedTimeField == DateTime.UnixEpoch) AdapterReceivedTimeField = default;
                return 0;
            case PQFeedFields.ClientSocketReceivingDateTime:
                if (QuoteBehavior.HasNoClientInboundSocketTimeUpdatesFlag()) return 0;
                IsSocketReceivedTimeDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref InboundSocketReceivingTimeField, pqFieldUpdate.Payload);
                if (InboundSocketReceivingTimeField == DateTime.UnixEpoch) InboundSocketReceivingTimeField = default;
                return 0;
            case PQFeedFields.ClientSocketReceivingSub2MinTime:
                if (QuoteBehavior.HasNoClientInboundSocketTimeUpdatesFlag()) return 0;
                IsSocketReceivedTimeSub2MinUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref InboundSocketReceivingTimeField,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (InboundSocketReceivingTimeField == DateTime.UnixEpoch) InboundSocketReceivingTimeField = default;
                return 0;
            case PQFeedFields.ClientProcessedDateTime:
                if (QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag()) return 0;
                IsProcessedTimeDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref InboundProcessedTimeField, pqFieldUpdate.Payload);
                if (InboundProcessedTimeField == DateTime.UnixEpoch) InboundProcessedTimeField = default;
                return 0;
            case PQFeedFields.ClientProcessedSub2MinTime:
                if (QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag()) return 0;
                IsProcessedTimeSub2MinUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref InboundProcessedTimeField,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (InboundProcessedTimeField == DateTime.UnixEpoch) InboundProcessedTimeField = default;
                return 0;
            case PQFeedFields.ClientDispatchedDateTime:
                if (QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag()) return 0;
                IsDispatchedTimeDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref DispatchedTimeField, pqFieldUpdate.Payload);
                if (DispatchedTimeField == DateTime.UnixEpoch) DispatchedTimeField = default;
                return 0;
            case PQFeedFields.ClientDispatchedSub2MinTime:
                if (QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag()) return 0;
                IsDispatchedTimeSub2MinUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref DispatchedTimeField,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (DispatchedTimeField == DateTime.UnixEpoch) DispatchedTimeField = default;
                return 0;
            case PQFeedFields.ClientReceivedDateTime:
                if (QuoteBehavior.HasNoClientReceiveTimeUpdatesFlag()) return 0;
                IsClientReceivedTimeDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref ClientReceivedTimeField, pqFieldUpdate.Payload);
                if (ClientReceivedTimeField == DateTime.UnixEpoch) ClientReceivedTimeField = default;
                return 0;
            case PQFeedFields.ClientReceivedSub2MinTime:
                if (QuoteBehavior.HasNoClientReceiveTimeUpdatesFlag()) return 0;
                IsClientReceivedTimeSub2MinUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref ClientReceivedTimeField,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (ClientReceivedTimeField == DateTime.UnixEpoch) ClientReceivedTimeField = default;
                return 0;
        }

        return -1;
    }

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => (IVersionedMessage)Clone();

    IPQMessage IPQMessage.Clone() => (IPQMessage)Clone();

    IPQMessage ICloneable<IPQMessage>.Clone() => (IPQMessage)Clone();

    IMutableFeedEventStatusUpdate ICloneable<IMutableFeedEventStatusUpdate>.Clone() => (IMutableFeedEventStatusUpdate)Clone();

    IMutableFeedEventStatusUpdate IMutableFeedEventStatusUpdate.Clone() => (IMutableFeedEventStatusUpdate)Clone();

    public abstract override IFeedEventStatusUpdate Clone();

    IFeedEventStatusUpdate ITransferState<IFeedEventStatusUpdate>.CopyFrom
        (IFeedEventStatusUpdate source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IVersionedMessage ITransferState<IVersionedMessage>.CopyFrom
        (IVersionedMessage? source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IFeedEventStatusUpdate?)source!, copyMergeFlags);

    IReusableObject<IVersionedMessage> ITransferState<IReusableObject<IVersionedMessage>>.CopyFrom
        (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IFeedEventStatusUpdate?)source, copyMergeFlags);

    IPQMessage IPQMessage.CopyFrom(IPQMessage source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    IPQMessage ITransferState<IPQMessage>.CopyFrom(IPQMessage source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override PQReusableMessage CopyFrom(IFeedEventStatusUpdate? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source != null)
        {
            if (QuoteBehavior.HasInheritAdditionalPublishedFlagsFlag())
            {
                QuoteBehavior |= source.QuoteBehavior;
            }
            else if (QuoteBehavior.HasRestoreAndOverlayOriginalQuoteFlagsFlag())
            {
                QuoteBehavior |= source.QuoteBehavior.ToQuoteInstantBehaviorMask();
            }
            else if (QuoteBehavior.HasRestoreOriginalQuoteFlagsFlag())
            {
                QuoteBehavior = QuoteBehavior.JustPublishableBehaviorMask() | source.QuoteBehavior.ToQuoteInstantBehaviorMask();
            }
        }
        if (source is IPQMessage pqMessage)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();
            if (pqMessage.IsFeedConnectivityStatusUpdated || isFullReplace)
            {
                IsFeedConnectivityStatusUpdated = true;
                FeedMarketConnectivityStatus    = pqMessage.FeedMarketConnectivityStatus;
            }
            if (!QuoteBehavior.HasNoAdapterSentTimeUpdatesFlag() && pqMessage.IsAdapterSentTimeDateUpdated || isFullReplace)
            {
                var originalAdapterSentTime = AdapterSentTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch
                    (ref AdapterSentTimeField, pqMessage.AdapterSentTime.Get2MinIntervalsFromUnixEpoch());
                IsAdapterSentTimeDateUpdated |= originalAdapterSentTime != AdapterSentTimeField;
                if (AdapterSentTimeField == DateTime.UnixEpoch) AdapterSentTimeField = default;
            }
            if (!QuoteBehavior.HasNoAdapterSentTimeUpdatesFlag() && pqMessage.IsAdapterSentTimeSub2MinUpdated || isFullReplace)
            {
                var originalAdapterSentTime = AdapterSentTimeField;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref AdapterSentTimeField, pqMessage.AdapterSentTime.GetSub2MinComponent());
                IsAdapterSentTimeSub2MinUpdated |= originalAdapterSentTime != AdapterSentTimeField;
                if (AdapterSentTimeField == DateTime.UnixEpoch) AdapterSentTimeField = default;
            }
            if (!QuoteBehavior.HasNoAdapterReceiveTimeUpdatesFlag() && pqMessage.IsAdapterReceivedTimeDateUpdated || isFullReplace)
            {
                var originalAdapterReceivedTime = AdapterReceivedTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch
                    (ref AdapterReceivedTimeField, pqMessage.AdapterReceivedTime.Get2MinIntervalsFromUnixEpoch());
                IsAdapterReceivedTimeDateUpdated |= originalAdapterReceivedTime != AdapterReceivedTimeField;
                if (AdapterReceivedTimeField == DateTime.UnixEpoch) AdapterReceivedTimeField = default;
            }
            if (!QuoteBehavior.HasNoAdapterReceiveTimeUpdatesFlag() && pqMessage.IsAdapterReceivedTimeSub2MinUpdated || isFullReplace)
            {
                var originalAdapterReceivedTime = AdapterReceivedTimeField;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref AdapterReceivedTimeField, pqMessage.AdapterReceivedTime.GetSub2MinComponent());
                IsAdapterReceivedTimeSub2MinUpdated |= originalAdapterReceivedTime != AdapterReceivedTimeField;
                if (AdapterReceivedTimeField == DateTime.UnixEpoch) AdapterReceivedTimeField = default;
            }
            if (!QuoteBehavior.HasNoClientInboundSocketTimeUpdatesFlag() && pqMessage.IsSocketReceivedTimeDateUpdated || isFullReplace)
            {
                var originalSocketReceivingTime = InboundSocketReceivingTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch
                    (ref InboundSocketReceivingTimeField, pqMessage.InboundSocketReceivingTime.Get2MinIntervalsFromUnixEpoch());
                IsSocketReceivedTimeDateUpdated = originalSocketReceivingTime != InboundSocketReceivingTimeField;
                if (InboundSocketReceivingTimeField == DateTime.UnixEpoch) InboundSocketReceivingTimeField = default;
            }
            if (!QuoteBehavior.HasNoClientInboundSocketTimeUpdatesFlag() && pqMessage.IsSocketReceivedTimeSub2MinUpdated || isFullReplace)
            {
                var originalSocketReceivingTime = InboundSocketReceivingTimeField;
                PQFieldConverters.UpdateSub2MinComponent
                    (ref InboundSocketReceivingTimeField, pqMessage.InboundSocketReceivingTime.GetSub2MinComponent());
                IsSocketReceivedTimeSub2MinUpdated = originalSocketReceivingTime != InboundSocketReceivingTimeField;
                if (InboundSocketReceivingTimeField == DateTime.UnixEpoch) InboundSocketReceivingTimeField = default;
            }
            if (!QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && pqMessage.IsProcessedTimeDateUpdated || isFullReplace)
            {
                var originalProcessedTime = InboundProcessedTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch
                    (ref InboundProcessedTimeField, pqMessage.InboundProcessedTime.Get2MinIntervalsFromUnixEpoch());
                IsProcessedTimeDateUpdated = originalProcessedTime != InboundProcessedTimeField;
                if (InboundProcessedTimeField == DateTime.UnixEpoch) InboundProcessedTimeField = default;
            }
            if (!QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && pqMessage.IsProcessedTimeSub2MinUpdated || isFullReplace)
            {
                var originalProcessedTime = InboundProcessedTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref InboundProcessedTimeField, pqMessage.InboundProcessedTime.GetSub2MinComponent());
                IsProcessedTimeSub2MinUpdated = originalProcessedTime != InboundProcessedTimeField;
                if (InboundProcessedTimeField == DateTime.UnixEpoch) InboundProcessedTimeField = default;
            }
            if (!QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && pqMessage.IsDispatchedTimeDateUpdated || isFullReplace)
            {
                var originalDispatchedTime = DispatchedTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch
                    (ref DispatchedTimeField, pqMessage.SubscriberDispatchedTime.Get2MinIntervalsFromUnixEpoch());
                IsDispatchedTimeDateUpdated = originalDispatchedTime != DispatchedTimeField;
                if (DispatchedTimeField == DateTime.UnixEpoch) DispatchedTimeField = default;
            }
            if (!QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag() && pqMessage.IsDispatchedTimeSub2MinUpdated || isFullReplace)
            {
                var originalDispatchedTime = DispatchedTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref DispatchedTimeField, pqMessage.SubscriberDispatchedTime.GetSub2MinComponent());
                IsDispatchedTimeSub2MinUpdated = originalDispatchedTime != DispatchedTimeField;
                if (DispatchedTimeField == DateTime.UnixEpoch) DispatchedTimeField = default;
            }
            if (!QuoteBehavior.HasNoClientReceiveTimeUpdatesFlag() && pqMessage.IsClientReceivedTimeDateUpdated || isFullReplace)
            {
                var originalClientReceivedTime = ClientReceivedTimeField;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch
                    (ref ClientReceivedTimeField, pqMessage.ClientReceivedTime.Get2MinIntervalsFromUnixEpoch());
                IsClientReceivedTimeDateUpdated = originalClientReceivedTime != ClientReceivedTimeField;
                if (ClientReceivedTimeField == DateTime.UnixEpoch) ClientReceivedTimeField = default;
            }
            if (!QuoteBehavior.HasNoClientReceiveTimeUpdatesFlag() && pqMessage.IsClientReceivedTimeSub2MinUpdated || isFullReplace)
            {
                var originalClientReceivedTime = ClientReceivedTimeField;
                PQFieldConverters.UpdateSub2MinComponent(ref ClientReceivedTimeField, pqMessage.ClientReceivedTime.GetSub2MinComponent());
                IsClientReceivedTimeSub2MinUpdated = originalClientReceivedTime != ClientReceivedTimeField;
                if (ClientReceivedTimeField == DateTime.UnixEpoch) ClientReceivedTimeField = default;
            }
            if (pqMessage.IsFeedSyncStatusUpdated || isFullReplace) FeedSyncStatus = pqMessage.FeedSyncStatus;

            OverrideSerializationFlags = pqMessage.OverrideSerializationFlags;

            LastPublicationTime = pqMessage.LastPublicationTime;
            PQSequenceId        = pqMessage.PQSequenceId;
        }
        else if (source is not null)
        {
            OverrideSerializationFlags = null;

            IsCompleteUpdate = source.IsCompleteUpdate;

            FeedMarketConnectivityStatus = source.FeedMarketConnectivityStatus;
            FeedSyncStatus               = source.FeedSyncStatus;

            ClientReceivedTime         = source.ClientReceivedTime;
            AdapterSentTime            = source.AdapterSentTime;
            AdapterReceivedTime        = source.AdapterReceivedTime;
            InboundSocketReceivingTime = source.InboundSocketReceivingTime;
            SubscriberDispatchedTime   = source.SubscriberDispatchedTime;
            InboundProcessedTime       = source.InboundProcessedTime;
        }

        return this;
    }

    public virtual void SetPublisherStateToConnectivityStatus(PublisherStates publisherStates, DateTime atDateTime)
    {
        // Todo map publisherStates to feed states
    }

    public abstract void EnsureRelatedItemsAreConfigured(IPQMessage? item);


    public virtual bool AreEquivalent(IFeedEventStatusUpdate? other, bool exactTypes = false)
    {
        if (other == null) return false;

        var feedConnectivityStatus = FeedMarketConnectivityStatus == other.FeedMarketConnectivityStatus;
        var feedSyncStatusSame     = FeedSyncStatus == other.FeedSyncStatus;

        if (!exactTypes)
        {
            return feedSyncStatusSame && feedConnectivityStatus;
        }
        if (other is not PQReusableMessage pqMessage) return false;

        var adapterReceivedSame = QuoteBehavior.HasIgnoreAdapterReceiveTimeCompareFlag() ||
                                  AdapterReceivedTimeField.Equals(pqMessage.AdapterReceivedTime);
        var adapterSentSame         = QuoteBehavior.HasIgnoreAdapterSentTimeCompareFlag() || AdapterSentTimeField.Equals(pqMessage.AdapterSentTime);
        var ignoreClientTimes       = QuoteBehavior.HasIgnoreClientTimesCompareFlag();
        var clientReceivedSame      = ignoreClientTimes || ClientReceivedTime == other.ClientReceivedTime;
        var socketReceivingTimeSame = ignoreClientTimes || InboundSocketReceivingTime == (pqMessage.InboundSocketReceivingTime);
        var lastPubTimeSame         = ignoreClientTimes || LastPublicationTime == (pqMessage.LastPublicationTime);
        var processingTimeSame      = ignoreClientTimes || InboundProcessedTime == pqMessage.InboundProcessedTime;
        var dispatchTimeSame        = ignoreClientTimes || SubscriberDispatchedTime == pqMessage.SubscriberDispatchedTime;
        var pqSequenceIdSame        = PQSequenceId == pqMessage.PQSequenceId;

        var allAreSame = adapterReceivedSame && adapterSentSame && clientReceivedSame && dispatchTimeSame && processingTimeSame && lastPubTimeSame
                      && socketReceivingTimeSame && pqSequenceIdSame && feedSyncStatusSame && feedConnectivityStatus;
        return allAreSame;
    }

    bool IInterfacesComparable<IPQMessage>.AreEquivalent(IPQMessage? other, bool exactTypes) => AreEquivalent(other, exactTypes);

    protected void SetFlagsSame(IPQMessage toCopyFlags)
    {
        if (toCopyFlags is PQReusableMessage pqToClone) MessageUpdatedFlags = pqToClone.MessageUpdatedFlags;
    }

    public virtual string PQReusableMessageToStringMembers =>
        $"{nameof(ClientReceivedTime)}: {ClientReceivedTime:O}, {nameof(InboundSocketReceivingTime)}: {InboundSocketReceivingTime:O}, " +
        $"{nameof(InboundProcessedTime)}: {InboundProcessedTime:O}, {nameof(SubscriberDispatchedTime)}: {SubscriberDispatchedTime:O}, " +
        $"{nameof(AdapterSentTime)}: {AdapterSentTime:O}, {nameof(AdapterReceivedTime)}: {AdapterReceivedTime:O}, " +
        $"{nameof(IsCompleteUpdate)}: {IsCompleteUpdate}";

    protected string MessageUpdatedFlagsToString => $"{nameof(MessageUpdatedFlags)}: {MessageUpdatedFlags}";
    protected string JustFeedStatusToStringMembers =>
        $"{nameof(FeedMarketConnectivityStatus)}: {FeedMarketConnectivityStatus}, {nameof(FeedSyncStatus)}: {FeedSyncStatus}, {nameof(PQSequenceId)}: {PQSequenceId}";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysAdd(nameof(FeedMarketConnectivityStatus), FeedMarketConnectivityStatus)
            .Field.AlwaysAdd(nameof(FeedSyncStatus), FeedSyncStatus)
            .Field.WhenNonDefaultAdd(nameof(ClientReceivedTime), ClientReceivedTime, DateTime.MinValue, "{0:O}")
            .Field.WhenNonDefaultAdd(nameof(InboundSocketReceivingTime), InboundSocketReceivingTime, DateTime.MinValue, "{0:O}")
            .Field.WhenNonDefaultAdd(nameof(SubscriberDispatchedTime), SubscriberDispatchedTime, DateTime.MinValue, "{0:O}")
            .Field.WhenNonDefaultAdd(nameof(AdapterSentTime), AdapterSentTime, DateTime.MinValue, "{0:O}")
            .Field.WhenNonDefaultAdd(nameof(AdapterReceivedTime), AdapterReceivedTime, DateTime.MinValue, "{0:O}")
            .Field.AlwaysAdd(nameof(IsCompleteUpdate), IsCompleteUpdate)
            .Complete();

    public override string ToString() =>
        $"{nameof(PQReusableMessage)}{{{PQReusableMessageToStringMembers}, {JustFeedStatusToStringMembers}, {MessageUpdatedFlagsToString}}}";
}
