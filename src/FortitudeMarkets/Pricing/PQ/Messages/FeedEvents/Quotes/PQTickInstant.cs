// Licensed under the MIT license.
// Copyright Alexis Sawenko 2024 all rights reserved

#region

using System.Text.Json.Serialization;
using FortitudeCommon.DataStructures.Collections;
using FortitudeCommon.DataStructures.Lists.LinkedLists;
using FortitudeCommon.DataStructures.MemoryPools;
using FortitudeCommon.Types;
using FortitudeCommon.Types.Mutable;
using FortitudeCommon.Types.StringsOfPower;
using FortitudeCommon.Types.StringsOfPower.DieCasting;
using FortitudeIO.Protocols;
using FortitudeIO.Storage.TimeSeries;
using FortitudeMarkets.Pricing.FeedEvents;
using FortitudeMarkets.Pricing.FeedEvents.Quotes;
using FortitudeMarkets.Pricing.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.DeltaUpdates;
using FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.TickerInfo;
using FortitudeMarkets.Pricing.TimeSeries;

#endregion

namespace FortitudeMarkets.Pricing.PQ.Messages.FeedEvents.Quotes;

public interface IPQTickInstant : IMutableTickInstant, IRelatedItems<ISourceTickerInfo>, IRelatedItems<ITickInstant>,
    IPQSupportsNumberPrecisionFieldUpdates, IPQSupportsStringUpdates, IEmptyable
{
    bool IsSourceTimeDateUpdated    { get; set; }
    bool IsSourceTimeSub2MinUpdated { get; set; }
    bool IsSingleValueUpdated       { get; set; }

    new IPQTickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags);

    new IPQTickInstant Clone();
}

public interface IPQPublishableTickInstant : IPQTickInstant, IMutablePublishableTickInstant, IPQMessage,
    IDoublyLinkedListNode<IPQPublishableTickInstant>, ITrackableReset<IPQPublishableTickInstant>
{
    new bool IsSourceTimeDateUpdated { get; set; }

    new bool IsSourceTimeSub2MinUpdated { get; set; }

    new IPQPublishableTickInstant? Next { get; set; }

    new IPQPublishableTickInstant? Previous { get; set; }

    new IPQSourceTickerInfo? SourceTickerInfo { get; set; }

    new IPQTickInstant AsNonPublishable { get; }

    new int UpdateField(PQFieldUpdate pqFieldUpdate);

    new IPQPublishableTickInstant ResetWithTracking();

    new bool UpdateFieldString(PQFieldStringUpdate fieldStringUpdate);

    new IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags
      , IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null);

    new IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags);

    new IPQPublishableTickInstant CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags);

    new IPQPublishableTickInstant Clone();
}

public class PQTickInstant : ReusableObject<ITickInstant>, IPQTickInstant, ICloneable<IPQTickInstant>
{
    protected QuoteFieldUpdatedFlags UpdatedFlags;
    protected PQQuoteBooleanValues   QuoteBooleanFields = PQQuoteBooleanValues.DefaultEmptyQuoteFlags;

    protected PQFieldFlags PriceScalingPrecision;
    protected PQFieldFlags VolumeScalingPrecision;

    protected uint SequenceId = uint.MaxValue;

    private decimal  singleValue;
    private DateTime sourceTime;

    public PQTickInstant()
    {
        if (GetType() == typeof(PQPublishableTickInstant)) SequenceId = 0;
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQTickInstant(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m) { }

    public PQTickInstant(ISourceTickerInfo sourceTickerInfo, decimal singlePrice = 0m, DateTime? sourceTime = null)
    {
        SingleTickValue = singlePrice;
        SourceTime      = sourceTime ?? DateTime.MinValue;

        PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(sourceTickerInfo.RoundingPrecision);
        VolumeScalingPrecision = PQScaling.FindVolumeScaleFactor(Math.Min(sourceTickerInfo.MinSubmitSize, sourceTickerInfo.IncrementSize));

        if (GetType() == typeof(PQTickInstant)) SequenceId = 0;
    }

    public PQTickInstant(ITickInstant toClone)
    {
        SingleTickValue = toClone.SingleTickValue;
        SourceTime      = toClone.SourceTime;
        if (toClone is PQTickInstant pqTickInstant)
        {
            QuoteBooleanFields = pqTickInstant.QuoteBooleanFields;
            if (GetType() == typeof(PQTickInstant))
            {
                SequenceId = pqTickInstant.SequenceId;
            }
        }
        else if (GetType() == typeof(PQTickInstant))
        {
            SequenceId = 0;
        }
        SetFlagsSame(toClone);
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual decimal SingleTickValue
    {
        get => singleValue;
        set
        {
            IsSingleValueUpdated |= singleValue != value || SequenceId == 0;
            singleValue          =  value;
        }
    }

    [JsonIgnore]
    public bool IsSingleValueUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SingleValueUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SingleValueUpdatedFlag;
            else
                UpdatedFlags &= ~QuoteFieldUpdatedFlags.SingleValueUpdatedFlag;
        }
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual DateTime SourceTime
    {
        get => sourceTime;
        set
        {
            IsSourceTimeDateUpdated    |= sourceTime.Get2MinIntervalsFromUnixEpoch() != value.Get2MinIntervalsFromUnixEpoch() || SequenceId == 0;
            IsSourceTimeSub2MinUpdated |= sourceTime.GetSub2MinComponent() != value.GetSub2MinComponent() || SequenceId == 0;
            sourceTime                 =  value == DateTime.UnixEpoch ? default : value;
        }
    }

    public virtual void IncrementTimeBy(TimeSpan toChangeBy)
    {
        sourceTime += toChangeBy;
    }

    [JsonIgnore]
    public bool IsSourceTimeDateUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceSentDateUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceSentDateUpdatedFlag;

            else if (IsSourceTimeDateUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceSentDateUpdatedFlag;
        }
    }

    [JsonIgnore]
    public bool IsSourceTimeSub2MinUpdated
    {
        get => (UpdatedFlags & QuoteFieldUpdatedFlags.SourceSentSub2MinUpdatedFlag) > 0;
        set
        {
            if (value)
                UpdatedFlags |= QuoteFieldUpdatedFlags.SourceSentSub2MinUpdatedFlag;

            else if (IsSourceTimeSub2MinUpdated) UpdatedFlags ^= QuoteFieldUpdatedFlags.SourceSentSub2MinUpdatedFlag;
        }
    }

    [JsonIgnore]
    public virtual bool HasUpdates
    {
        get => UpdatedFlags != QuoteFieldUpdatedFlags.None || (QuoteBooleanFields & PQQuoteBooleanValues.BooleanUpdatesMask) > 0;
        set
        {
            UpdatedFlags = value ? UpdatedFlags.AllFlags() : QuoteFieldUpdatedFlags.None;
            if (value) return;
            QuoteBooleanFields &= PQQuoteBooleanValues.BooleanValuesMask;
        }
    }

    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<ITickInstant> quoteStorageResolver) return quoteStorageResolver.ResolveStorageTime(this);
        return QuoteStorageTimeResolver.Instance.ResolveStorageTime(this);
    }

    public uint UpdateSequenceId => SequenceId;

    public virtual void UpdateStarted(uint updateSequenceId)
    {
        SequenceId = updateSequenceId;
    }

    public virtual void UpdateComplete(uint updateSequenceId = 0)
    {
        if (HasUpdates) SequenceId++;
        HasUpdates = false;
    }

    public virtual bool IsEmpty
    {
        get =>
            singleValue == 0m
         && sourceTime == DateTime.MinValue
         && (QuoteBooleanFields & PQQuoteBooleanValues.BooleanValuesMask) == PQQuoteBooleanValues.DefaultEmptyQuoteFlags;
        set
        {
            if (!value) return;
            ResetWithTracking();
        }
    }

    public virtual IMutableTickInstant ResetWithTracking()
    {
        SequenceId  = 0;
        singleValue = 0m;
        sourceTime  = DateTime.MinValue;

        QuoteBooleanFields = PQQuoteBooleanValues.DefaultEmptyQuoteFlags;
        return this;
    }

    public override void StateReset()
    {
        ResetWithTracking();
        UpdatedFlags = QuoteFieldUpdatedFlags.None;
        base.StateReset();
    }

    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags,
        IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        var fullPicture = (messageFlags & Serdes.Serialization.PQMessageFlags.Complete) > 0;

        if (fullPicture || IsSingleValueUpdated)
            yield return new PQFieldUpdate(PQFeedFields.SingleTickValue, SingleTickValue
                                         , quotePublicationPrecisionSettings?.PriceScalingPrecision ?? PriceScalingPrecision);
        if (fullPicture || IsSourceTimeDateUpdated)
            yield return new PQFieldUpdate(PQFeedFields.SourceQuoteSentDateTime, sourceTime.Get2MinIntervalsFromUnixEpoch());
        if (fullPicture || IsSourceTimeSub2MinUpdated)
        {
            var extended = sourceTime.GetSub2MinComponent().BreakLongToUShortAndScaleFlags(out var lower4Bytes);
            yield return new PQFieldUpdate(PQFeedFields.SourceQuoteSentSub2MinTime, lower4Bytes, extended);
        }
        if (fullPicture || IsBooleanFlagsChanged())
        {
            var booleanFields = GenerateBooleanFlags(fullPicture);
            yield return new PQFieldUpdate(PQFeedFields.QuoteBooleanFlags, (uint)booleanFields);
        }
    }

    public virtual int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        switch (pqFieldUpdate.Id)
        {
            case PQFeedFields.SingleTickValue:
                IsSingleValueUpdated = true; // in-case of reset and sending 0;
                SingleTickValue      = PQScaling.Unscale(pqFieldUpdate.Payload, pqFieldUpdate.Flag);
                return 0;
            case PQFeedFields.SourceQuoteSentDateTime:
                IsSourceTimeDateUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref sourceTime, pqFieldUpdate.Payload);
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
                return 0;
            case PQFeedFields.SourceQuoteSentSub2MinTime:
                IsSourceTimeSub2MinUpdated = true; // in-case of reset and sending 0;
                PQFieldConverters.UpdateSub2MinComponent(ref sourceTime,
                                                         pqFieldUpdate.Flag.AppendScaleFlagsToUintToMakeLong(pqFieldUpdate.Payload));
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
                return 0;
            case PQFeedFields.QuoteBooleanFlags:
                SetBooleanFields((PQQuoteBooleanValues)pqFieldUpdate.Payload & PQQuoteBooleanValues.QuoteValuesAndFlagsOnlySet);
                return 0;
        }

        return -1;
    }

    public virtual void EnsureRelatedItemsAreConfigured(ITickInstant? item)
    {
        if (item is PQTickInstant pqTickInstant)
        {
            PriceScalingPrecision  = pqTickInstant.PriceScalingPrecision;
            VolumeScalingPrecision = pqTickInstant.VolumeScalingPrecision;
        }
        if (item is IPublishableTickInstant { SourceTickerInfo: not null } pubInstance)
        {
            PriceScalingPrecision = PQScaling.FindPriceScaleFactor(pubInstance.SourceTickerInfo.RoundingPrecision);
            VolumeScalingPrecision = PQScaling.FindVolumeScaleFactor
                (Math.Min(pubInstance.SourceTickerInfo.MinSubmitSize, pubInstance.SourceTickerInfo.IncrementSize));
        }
    }

    public virtual void EnsureRelatedItemsAreConfigured(ISourceTickerInfo? srcTickerInfo)
    {
        if (srcTickerInfo is IPQSourceTickerInfo pqSrcTickerInfo)
        {
            PriceScalingPrecision  = pqSrcTickerInfo.PriceScalingPrecision;
            VolumeScalingPrecision = pqSrcTickerInfo.VolumeScalingPrecision;
        }
        else if (srcTickerInfo is not null)
        {
            PriceScalingPrecision = PQScaling.FindPriceScaleFactor(srcTickerInfo.RoundingPrecision);
            VolumeScalingPrecision = PQScaling.FindVolumeScaleFactor
                (Math.Min(srcTickerInfo.MinSubmitSize, srcTickerInfo.IncrementSize));
        }
    }

    public virtual IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags)
    {
        return [];
    }

    public virtual bool UpdateFieldString(PQFieldStringUpdate stringUpdate) => false;

    public virtual PQTickInstant SetSourceTickerInfo(ISourceTickerInfo toSet)
    {
        if (toSet is IPQSourceTickerInfo pqSourceTickerInfo)
        {
            PriceScalingPrecision  = pqSourceTickerInfo.PriceScalingPrecision;
            VolumeScalingPrecision = pqSourceTickerInfo.VolumeScalingPrecision;
        }
        else
        {
            PriceScalingPrecision  = PQScaling.FindPriceScaleFactor(toSet.RoundingPrecision);
            VolumeScalingPrecision = PQScaling.FindVolumeScaleFactor(Math.Min(toSet.MinSubmitSize, toSet.IncrementSize));
        }
        return this;
    }

    protected virtual bool IsBooleanFlagsChanged() => false;

    protected virtual PQQuoteBooleanValues GenerateBooleanFlags(bool fullUpdate)
    {
        return PQQuoteBooleanValues.None;
    }

    protected virtual void SetBooleanFields(PQQuoteBooleanValues quoteBooleanFlags) { }

    ITickInstant ICloneable<ITickInstant>.Clone() => Clone();

    IMutableTickInstant IMutableTickInstant.Clone() => Clone();

    IPQTickInstant IPQTickInstant.Clone() => Clone();

    IPQTickInstant ICloneable<IPQTickInstant>.Clone() => Clone();

    public override PQTickInstant Clone() =>
        Recycler?.Borrow<PQTickInstant>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new PQTickInstant(this);

    IPQTickInstant IPQTickInstant.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) => CopyFrom(source, copyMergeFlags);

    public override PQTickInstant CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPQTickInstant ipq0)
        {
            // only copy if changed
            var isFullReplace = copyMergeFlags.HasFullReplace();

            if (ipq0.IsSingleValueUpdated || isFullReplace)
            {
                IsSingleValueUpdated = true;
                SingleTickValue      = ipq0.SingleTickValue;
            }
            if (ipq0.IsSourceTimeDateUpdated || isFullReplace)
            {
                var originalSourceTime = sourceTime;
                PQFieldConverters.Update2MinuteIntervalsFromUnixEpoch(ref sourceTime, ipq0.SourceTime.Get2MinIntervalsFromUnixEpoch());
                IsSourceTimeDateUpdated = originalSourceTime != sourceTime;
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
            }
            if (ipq0.IsSourceTimeSub2MinUpdated || isFullReplace)
            {
                var originalSourceTime = sourceTime;
                PQFieldConverters.UpdateSub2MinComponent(ref sourceTime, ipq0.SourceTime.GetSub2MinComponent());
                IsSourceTimeSub2MinUpdated = originalSourceTime != sourceTime;
                if (sourceTime == DateTime.UnixEpoch) sourceTime = default;
            }

            if (isFullReplace) SetFlagsSame(ipq0);
        }
        else
        {
            SingleTickValue = source.SingleTickValue;
            SourceTime      = source.SourceTime;
        }

        return this;
    }

    public virtual bool AreEquivalent(ITickInstant? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var singlePriceSame = singleValue == other.SingleTickValue;
        var sourceTimeSame  = SourceTime == other.SourceTime;

        var updatedFlagsSame  = true;
        var booleanFieldsSame = true;
        var pqTickInstant     = other as PQTickInstant;
        if (exactTypes)
        {
            updatedFlagsSame  = UpdatedFlags == pqTickInstant!.UpdatedFlags;
            booleanFieldsSame = QuoteBooleanFields == pqTickInstant.QuoteBooleanFields;
        }

        var allAreSame = singlePriceSame && sourceTimeSame && updatedFlagsSame && booleanFieldsSame;
        return allAreSame;
    }

    protected void SetFlagsSame(ITickInstant toCopyFlags)
    {
        if (toCopyFlags is PQTickInstant pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent(obj as ITickInstant, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)UpdatedFlags;
            hashCode = (hashCode * 397) ^ QuoteBooleanFields.GetHashCode();
            hashCode = (hashCode * 397) ^ singleValue.GetHashCode();
            return hashCode;
        }
    }

    public virtual string QuoteToStringMembers => $"{nameof(SingleTickValue)}: {SingleTickValue}";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public virtual StateExtractStringRange RevealState(ITheOneString tos) => 
    tos.StartComplexType(this)
        .Field.AlwaysAdd(nameof(SingleTickValue), SingleTickValue)
        .Field.WhenNonDefaultAdd(nameof(UpdatedFlags), UpdatedFlags)
        .Complete();

    public override string ToString() => $"{GetType().Name}({QuoteToStringMembers}, {UpdatedFlagsToString})";
}

public class PQPublishableTickInstant : PQReusableMessage, IPQPublishableTickInstant, ICloneable<PQPublishableTickInstant>
  , IDoublyLinkedListNode<PQPublishableTickInstant>
{
    protected IPQTickInstant PQQuoteContainer;

    protected PublishableQuoteFieldUpdatedFlags UpdatedFlags;

    protected IPQSourceTickerInfo? PQSourceTickerInfo;

    public PQPublishableTickInstant()
    {
        PQQuoteContainer = CreateEmptyQuoteContainerInstant();
    }

    // Reflection invoked constructor (PQServer<T>)
    public PQPublishableTickInstant(ISourceTickerInfo sourceTickerInfo) : this(sourceTickerInfo, singlePrice: 0m) { }

    protected PQPublishableTickInstant(IPQTickInstant? initializedQuoteContainer, ISourceTickerInfo sourceTickerInfo)
        : this(initializedQuoteContainer, sourceTickerInfo, feedSyncStatus: FeedSyncStatus.Good) { }

    public PQPublishableTickInstant
    (ISourceTickerInfo sourceTickerInfo, decimal singlePrice = 0m, DateTime? sourceTime = null
      , FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good, FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None)
        : this(new PQTickInstant(sourceTickerInfo, singlePrice, sourceTime), sourceTickerInfo, feedSyncStatus, feedConnectivityStatus) { }

    protected PQPublishableTickInstant
    (IPQTickInstant? initializedQuoteContainer, ISourceTickerInfo sourceTickerInfo,
        FeedSyncStatus feedSyncStatus = FeedSyncStatus.Good
      , FeedConnectivityStatusFlags feedConnectivityStatus = FeedConnectivityStatusFlags.None)
    {
        PQQuoteContainer = initializedQuoteContainer ?? CreateQuoteContainerFromTickerInfo(sourceTickerInfo);

        if (sourceTickerInfo is IPQSourceTickerInfo pqSourceTickerInfo)
        {
            SourceTickerInfo = pqSourceTickerInfo;
        }
        else
        {
            SourceTickerInfo = new PQSourceTickerInfo(sourceTickerInfo);
        }
        PQQuoteContainer.EnsureRelatedItemsAreConfigured(SourceTickerInfo);
        FeedSyncStatus               = feedSyncStatus;
        FeedMarketConnectivityStatus = feedConnectivityStatus;
    }

    public PQPublishableTickInstant(IPublishableTickInstant toClone) : this(toClone, null) { }

    public PQPublishableTickInstant(IPublishableTickInstant toClone, IPQTickInstant? initializedQuoteContainer) : base(toClone)
    {
        if (toClone is PQPublishableTickInstant pqToClone)
        {
            PQQuoteContainer = initializedQuoteContainer ?? pqToClone.PQQuoteContainer.Clone();
        }
        else
        {
            PQQuoteContainer = initializedQuoteContainer ?? CreateCloneQuoteContainerInstant(toClone);
        }
        FeedMarketConnectivityStatus = toClone.FeedMarketConnectivityStatus;
        ClientReceivedTime           = toClone.ClientReceivedTime;
        FeedSyncStatus               = toClone.FeedSyncStatus;

        SourceTickerInfo = new PQSourceTickerInfo(toClone.SourceTickerInfo!);
        if (toClone is IPQPublishableTickInstant ipqTickInstant)
        {
            OverrideSerializationFlags = ipqTickInstant.OverrideSerializationFlags;

            SourceTickerInfo           = ipqTickInstant.SourceTickerInfo;
            PQSequenceId               = ipqTickInstant.PQSequenceId;
            LastPublicationTime        = ipqTickInstant.LastPublicationTime;
            InboundSocketReceivingTime = ipqTickInstant.InboundSocketReceivingTime;
            InboundProcessedTime       = ipqTickInstant.InboundProcessedTime;
            SubscriberDispatchedTime   = ipqTickInstant.SubscriberDispatchedTime;
        }

        SetFlagsSame(toClone);
    }

    protected virtual IPQTickInstant CreateEmptyQuoteContainerInstant() => new PQTickInstant();

    protected virtual IPQTickInstant CreateCloneQuoteContainerInstant(ITickInstant tickInstant) => new PQTickInstant(tickInstant);

    protected virtual IPQTickInstant CreateQuoteContainerFromTickerInfo(ISourceTickerInfo tickerInfo) => new PQTickInstant(tickerInfo);

    ITickInstant IPublishableTickInstant.              AsNonPublishable => AsNonPublishable;

    IMutableTickInstant IMutablePublishableTickInstant.AsNonPublishable => AsNonPublishable;

    public virtual IPQTickInstant                      AsNonPublishable => PQQuoteContainer;

    [JsonIgnore] public virtual TickerQuoteDetailLevel TickerQuoteDetailLevel => TickerQuoteDetailLevel.SingleValue;

    [JsonIgnore] public override uint MessageId => (uint)PQMessageIds.Quote;

    [JsonIgnore] public override byte Version => 1;

    [JsonIgnore] ISourceTickerInfo? ICanHaveSourceTickerDefinition.SourceTickerInfo => PQSourceTickerInfo;

    ISourceTickerInfo? IMutableCanHaveSourceTickerDefinition.SourceTickerInfo
    {
        get => SourceTickerInfo;
        set => SourceTickerInfo = ConvertToPQSourceTickerInfo(value!, PQSourceTickerInfo);
    }

    public override IPQSourceTickerInfo? SourceTickerInfo
    {
        get => PQSourceTickerInfo;
        set
        {
            if (ReferenceEquals(value, PQSourceTickerInfo)) return;
            if (value is PQSourceTickerInfo pqSourceTickerInfo) // share SourceTickerInfo if possible
                PQSourceTickerInfo = pqSourceTickerInfo;
            if (value != null && PQSourceTickerInfo != null)
            {
                PQSourceTickerInfo.CopyFrom(value);
                return;
            }

            PQSourceTickerInfo = ConvertToPQSourceTickerInfo(value!, PQSourceTickerInfo);
        }
    }


    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public virtual decimal SingleTickValue
    {
        get => PQQuoteContainer.SingleTickValue;
        set => PQQuoteContainer.SingleTickValue = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsSingleValueUpdated
    {
        get => PQQuoteContainer.IsSingleValueUpdated;
        set => PQQuoteContainer.IsSingleValueUpdated = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public DateTime SourceTime
    {
        get => PQQuoteContainer.SourceTime;
        set => PQQuoteContainer.SourceTime = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsSourceTimeDateUpdated
    {
        get => PQQuoteContainer.IsSourceTimeDateUpdated;
        set => PQQuoteContainer.IsSourceTimeDateUpdated = value;
    }

    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public bool IsSourceTimeSub2MinUpdated
    {
        get => PQQuoteContainer.IsSourceTimeSub2MinUpdated;
        set => PQQuoteContainer.IsSourceTimeSub2MinUpdated = value;
    }


    [JsonIgnore]
    public new PQPublishableTickInstant? Previous
    {
        get => GetPrevious<PQPublishableTickInstant?>();
        set => SetPrevious(value);
    }

    [JsonIgnore]
    public new PQPublishableTickInstant? Next
    {
        get => GetNext<PQPublishableTickInstant?>();
        set => SetNext(value);
    }

    IPQPublishableTickInstant? IDoublyLinkedListNode<IPQPublishableTickInstant>.Previous
    {
        get => GetPrevious<IPQPublishableTickInstant?>();
        set => SetPrevious(value);
    }
    IPQPublishableTickInstant? IDoublyLinkedListNode<IPQPublishableTickInstant>.Next
    {
        get => GetNext<IPQPublishableTickInstant?>();
        set => SetNext(value);
    }

    [JsonIgnore]
    IPQPublishableTickInstant? IPQPublishableTickInstant.Previous
    {
        get => GetPrevious<IPQPublishableTickInstant?>();
        set => SetPrevious(value);
    }

    [JsonIgnore]
    IPQPublishableTickInstant? IPQPublishableTickInstant.Next
    {
        get => GetNext<IPQPublishableTickInstant?>();
        set => SetNext(value);
    }

    [JsonIgnore]
    IPublishableTickInstant? IDoublyLinkedListNode<IPublishableTickInstant>.Previous
    {
        get => GetPrevious<IPublishableTickInstant?>();
        set => SetPrevious(value);
    }
    [JsonIgnore]
    IPublishableTickInstant? IDoublyLinkedListNode<IPublishableTickInstant>.Next
    {
        get => GetNext<IPublishableTickInstant?>();
        set => SetNext(value);
    }

    [JsonIgnore]
    public override bool HasUpdates
    {
        get =>
            base.HasUpdates
         || UpdatedFlags != PublishableQuoteFieldUpdatedFlags.None
         || PQQuoteContainer.HasUpdates;
        set
        {
            base.HasUpdates = value;
            if (PQSourceTickerInfo != null) PQSourceTickerInfo.HasUpdates = value;
            PQQuoteContainer.HasUpdates = value;
            if (value) return;
            UpdatedFlags = PublishableQuoteFieldUpdatedFlags.None;
        }
    }

    public override bool IsEmpty
    {
        get => base.IsEmpty && PQQuoteContainer.IsEmpty;
        set
        {
            PQQuoteContainer.IsEmpty = value;
            base.IsEmpty             = value;
        }
    }

    public override void UpdateStarted(uint updateSequenceId = 0)
    {
        PQQuoteContainer.UpdateStarted(updateSequenceId);
    }

    public override void UpdateComplete(uint updateSequenceId = 0)
    {
        PQSourceTickerInfo?.UpdateComplete(updateSequenceId);
        base.UpdateComplete(updateSequenceId);
        PQQuoteContainer.UpdateComplete(updateSequenceId);
        HasUpdates = false;
    }

    IMutableTickInstant ITrackableReset<IMutableTickInstant>.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableTickInstant ITrackableReset<IMutablePublishableTickInstant>.ResetWithTracking() => ResetWithTracking();

    IMutablePublishableTickInstant IMutablePublishableTickInstant.ResetWithTracking() => ResetWithTracking();

    public override IPQPublishableTickInstant ResetWithTracking()
    {
        PQQuoteContainer.ResetWithTracking();

        base.ResetWithTracking();

        UpdatedFlags = PublishableQuoteFieldUpdatedFlags.None;
        return this;
    }

    IEnumerable<PQFieldUpdate> IPQPublishableTickInstant.GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags
      , IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings) =>
        GetDeltaUpdateFields(snapShotTime, messageFlags, quotePublicationPrecisionSettings);

    public override IEnumerable<PQFieldUpdate> GetDeltaUpdateFields(DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags) =>
        GetDeltaUpdateFields(snapShotTime, messageFlags);

    // ReSharper disable once MethodOverloadWithOptionalParameter
    public virtual IEnumerable<PQFieldUpdate> GetDeltaUpdateFields
    (DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags
      , IPQPriceVolumePublicationPrecisionSettings? quotePublicationPrecisionSettings = null)
    {
        foreach (var quoteContainerUpdates in base.GetDeltaUpdateFields(snapShotTime, messageFlags))
        {
            yield return quoteContainerUpdates;
        }

        if (PQSourceTickerInfo != null)
            foreach (var field in PQSourceTickerInfo.GetDeltaUpdateFields
                         (snapShotTime, messageFlags, quotePublicationPrecisionSettings ?? PQSourceTickerInfo))
                yield return field;

        foreach (var quoteContainerUpdates in PQQuoteContainer.GetDeltaUpdateFields
                     (snapShotTime, messageFlags, quotePublicationPrecisionSettings ?? PQSourceTickerInfo))
        {
            yield return quoteContainerUpdates;
        }
    }

    public override int UpdateField(PQFieldUpdate pqFieldUpdate)
    {
        var infoResult = PQSourceTickerInfo!.UpdateField(pqFieldUpdate);
        if (infoResult >= 0) return infoResult;
        infoResult = PQQuoteContainer.UpdateField(pqFieldUpdate);
        if (infoResult >= 0) return infoResult;

        return base.UpdateField(pqFieldUpdate);
    }

    public override IEnumerable<PQFieldStringUpdate> GetStringUpdates(DateTime snapShotTime, Serdes.Serialization.PQMessageFlags messageFlags)
    {
        if (PQSourceTickerInfo != null)
            foreach (var field in PQSourceTickerInfo.GetStringUpdates(snapShotTime, messageFlags))
                yield return field;
    }

    public override bool UpdateFieldString(PQFieldStringUpdate stringUpdate) =>
        PQSourceTickerInfo != null && PQSourceTickerInfo.UpdateFieldString(stringUpdate);

    public override void EnsureRelatedItemsAreConfigured(IPQMessage? item)
    {
        if (item is IPublishableTickInstant pubTickInstant)
        {
            EnsureRelatedItemsAreConfigured(pubTickInstant.SourceTickerInfo);
            if (item is PQPublishableTickInstant pqPubTickInstant)
            {
                PQQuoteContainer.EnsureRelatedItemsAreConfigured(pqPubTickInstant.PQQuoteContainer);
            }
        }
    }

    public virtual void EnsureRelatedItemsAreConfigured(ITickInstant? referenceInstance)
    {
        if (referenceInstance is IPublishableTickInstant { SourceTickerInfo: IPQSourceTickerInfo pqSrcTkrQuoteInfo })
            SourceTickerInfo = pqSrcTkrQuoteInfo;
        PQQuoteContainer.EnsureRelatedItemsAreConfigured(referenceInstance);
    }

    public virtual void EnsureRelatedItemsAreConfigured(ISourceTickerInfo? srcTickerInfo)
    {
        if (srcTickerInfo == null || ReferenceEquals(SourceTickerInfo, srcTickerInfo)) return;
        if (srcTickerInfo.AreEquivalent(SourceTickerInfo)) return;

        SourceTickerInfo = new PQSourceTickerInfo(srcTickerInfo);
        PQQuoteContainer.EnsureRelatedItemsAreConfigured(srcTickerInfo);
    }

    public virtual void IncrementTimeBy(TimeSpan toChangeBy)
    {
        PQQuoteContainer.IncrementTimeBy(toChangeBy);

        if (!QuoteBehavior.HasNoClientInboundSocketTimeUpdatesFlag()) InboundSocketReceivingTime += toChangeBy;
        if (!QuoteBehavior.HasNoClientProcessedTimeUpdatesFlag())
        {
            InboundProcessedTime     += toChangeBy;
            SubscriberDispatchedTime += toChangeBy;
        }
        if (!QuoteBehavior.HasNoClientReceiveTimeUpdatesFlag()) ClientReceivedTime   += toChangeBy;
        if (!QuoteBehavior.HasNoAdapterReceiveTimeUpdatesFlag()) AdapterReceivedTime += toChangeBy;
        if (!QuoteBehavior.HasNoAdapterSentTimeUpdatesFlag()) AdapterSentTime        += toChangeBy;
    }

    public DateTime StorageTime(IStorageTimeResolver? resolver)
    {
        if (resolver is IStorageTimeResolver<IPublishableTickInstant> quoteStorageResolver) return quoteStorageResolver.ResolveStorageTime(this);
        return QuoteStorageTimeResolver.Instance.ResolveStorageTime(this);
    }

    public override void SetPublisherStateToConnectivityStatus(PublisherStates publisherStates, DateTime atDateTime)
    {
        ResetWithTracking();
        SourceTime = atDateTime;
    }

    private IPQSourceTickerInfo ConvertToPQSourceTickerInfo
        (ISourceTickerInfo value, IPQSourceTickerInfo? originalQuoteInfo)
    {
        if (originalQuoteInfo == null)
        {
            originalQuoteInfo = new PQSourceTickerInfo(value);
            return originalQuoteInfo;
        }

        originalQuoteInfo.CopyFrom(value);
        return originalQuoteInfo;
    }

    public virtual PQPublishableTickInstant SetSourceTickerInfo(ISourceTickerInfo toSet)
    {
        ((IMutablePublishableTickInstant)this).SourceTickerInfo = toSet;
        return this;
    }

    IPublishableTickInstant IPublishableTickInstant.Clone() => Clone();

    IPublishableTickInstant ICloneable<IPublishableTickInstant>.Clone() => Clone();

    IVersionedMessage ICloneable<IVersionedMessage>.Clone() => Clone();

    IMutablePublishableTickInstant IMutablePublishableTickInstant.Clone() => Clone();

    IPQPublishableTickInstant IPQPublishableTickInstant.Clone() => Clone();

    ITickInstant ICloneable<ITickInstant>.Clone() => Clone();

    IMutableTickInstant IMutableTickInstant.Clone() => Clone();

    IPQTickInstant IPQTickInstant.Clone() => Clone();

    public override PQPublishableTickInstant Clone() =>
        Recycler?.Borrow<PQPublishableTickInstant>().CopyFrom(this, CopyMergeFlags.FullReplace) ??
        new PQPublishableTickInstant(this);

    IReusableObject<ITickInstant> ITransferState<IReusableObject<ITickInstant>>.CopyFrom
        (IReusableObject<ITickInstant> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPQMessage)source, copyMergeFlags);

    ITickInstant ITransferState<ITickInstant>.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPQMessage)source, copyMergeFlags);

    IReusableObject<IVersionedMessage> ITransferState<IReusableObject<IVersionedMessage>>.CopyFrom
        (IReusableObject<IVersionedMessage> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPQMessage)source, copyMergeFlags);

    IReusableObject<IPublishableTickInstant> ITransferState<IReusableObject<IPublishableTickInstant>>.CopyFrom
        (IReusableObject<IPublishableTickInstant> source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableTickInstant)source, copyMergeFlags);


    IPublishableTickInstant IPublishableTickInstant.CopyFrom
        (IPublishableTickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    IPQPublishableTickInstant IPQPublishableTickInstant.CopyFrom
        (IPublishableTickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);


    ITransferState IPublishableTickInstant.CopyFrom(ITransferState source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPQMessage)source, copyMergeFlags);

    IPQTickInstant IPQTickInstant.CopyFrom(ITickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom((IPublishableTickInstant)source, copyMergeFlags);

    IPublishableTickInstant ITransferState<IPublishableTickInstant>.CopyFrom
        (IPublishableTickInstant source, CopyMergeFlags copyMergeFlags) =>
        CopyFrom(source, copyMergeFlags);

    public virtual PQPublishableTickInstant CopyFrom(IPublishableTickInstant source, CopyMergeFlags copyMergeFlags)
    {
        base.CopyFrom(source, copyMergeFlags);
        if (source is IPQPublishableTickInstant ipq0)
        {
            var isFullReplace = copyMergeFlags.HasFullReplace();
            if (PQSourceTickerInfo != null)
                PQSourceTickerInfo.CopyFrom(ipq0.SourceTickerInfo!, copyMergeFlags);
            else
                SourceTickerInfo = ipq0.SourceTickerInfo;

            if (ipq0.IsFeedConnectivityStatusUpdated || isFullReplace)
            {
                IsFeedConnectivityStatusUpdated = true;
                FeedMarketConnectivityStatus    = ipq0.FeedMarketConnectivityStatus;
            }
            if (source is PQPublishableTickInstant pq0)
            {
                PQQuoteContainer.CopyFrom(pq0.PQQuoteContainer, copyMergeFlags);
                // only copy if changed
                if (isFullReplace) UpdatedFlags = pq0.UpdatedFlags;
            }
            else
            {
                PQQuoteContainer.CopyFrom(ipq0, copyMergeFlags);
            }
        }
        else
        {
            PQQuoteContainer.CopyFrom(source, copyMergeFlags);

            FeedMarketConnectivityStatus = source.FeedMarketConnectivityStatus;

            if (source.SourceTickerInfo != null)
            {
                SourceTickerInfo ??= new PQSourceTickerInfo(source.SourceTickerInfo);
                SourceTickerInfo.CopyFrom(source.SourceTickerInfo);
            }
            else
            {
                SourceTickerInfo?.StateReset();
            }
        }

        return this;
    }

    public override PQPublishableTickInstant CopyFrom(IFeedEventStatusUpdate? source, CopyMergeFlags copyMergeFlags = CopyMergeFlags.Default)
    {
        if (source is IPublishableTickInstant pubTickInstant)
        {
            CopyFrom(pubTickInstant, copyMergeFlags);
        }
        else
        {
            base.CopyFrom(source, copyMergeFlags);
        }
        return this;
    }

    public bool AreEquivalent(ITickInstant? other, bool exactTypes = false) => AreEquivalent(other as IPQMessage, exactTypes);

    public virtual bool AreEquivalent(IPublishableTickInstant? other, bool exactTypes = false)
    {
        if (other == null) return false;
        if (exactTypes && other.GetType() != GetType()) return false;
        var baseSame = base.AreEquivalent(other, exactTypes);

        var quoteValuesSame = PQQuoteContainer.AreEquivalent
            ((ITickInstant?)((other as PQPublishableTickInstant)?.PQQuoteContainer) ?? other, exactTypes);
        var tickerInfoSame =
            PQSourceTickerInfo?.AreEquivalent(other.SourceTickerInfo, exactTypes)
         ?? other.SourceTickerInfo == null;

        var allAreSame = quoteValuesSame && tickerInfoSame && baseSame;
        return allAreSame;
    }

    public override bool AreEquivalent(IFeedEventStatusUpdate? other, bool exactTypes = false)
    {
        if (other is IPublishableTickInstant pubTickOther)
        {
            return AreEquivalent(pubTickOther, exactTypes);
        }
        return false;
    }

    public override bool Equals(object? obj) => ReferenceEquals(this, obj) || AreEquivalent((PQPublishableTickInstant?)obj, true);

    public override int GetHashCode()
    {
        unchecked
        {
            var hashCode = (int)UpdatedFlags;
            hashCode = (hashCode * 397) ^ (int)PQSequenceId;
            hashCode = (hashCode * 397) ^ SourceTickerInfo?.GetHashCode() ?? 0;
            hashCode = (hashCode * 397) ^ PQQuoteContainer.GetHashCode();
            hashCode = (hashCode * 397) ^ LastPublicationTime.GetHashCode();
            hashCode = (hashCode * 397) ^ InboundSocketReceivingTime.GetHashCode();
            hashCode = (hashCode * 397) ^ InboundProcessedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ SubscriberDispatchedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ ClientReceivedTime.GetHashCode();
            hashCode = (hashCode * 397) ^ (int)FeedMarketConnectivityStatus;
            hashCode = (hashCode * 397) ^ (int)FeedSyncStatus;
            hashCode = (hashCode * 397) ^ (SourceTickerInfo?.GetHashCode() ?? 0);
            return hashCode;
        }
    }

    public virtual string QuoteToStringMembers =>
        $"{nameof(PQSourceTickerInfo)}: {PQSourceTickerInfo}, {nameof(PQSequenceId)}: {PQSequenceId}, " +
        $"{nameof(FeedSyncStatus)}: {FeedSyncStatus}, {nameof(LastPublicationTime)}: {LastPublicationTime}, " +
        $"{nameof(IsFeedSyncStatusUpdated)}: {IsFeedSyncStatusUpdated}, {nameof(HasUpdates)}: {HasUpdates}, " +
        $"{PQReusableMessageToStringMembers}, {MessageUpdatedFlagsToString} ";

    protected string UpdatedFlagsToString => $"{nameof(UpdatedFlags)}: {UpdatedFlags}";

    public override StateExtractStringRange RevealState(ITheOneString tos) =>
        tos.StartComplexType(this)
            .Field.AlwaysReveal(nameof(PQSourceTickerInfo), PQSourceTickerInfo)
            .Field.AlwaysAdd(nameof(PQSequenceId), PQSequenceId)
            .AddBaseStyledToStringFields(this)
            .Field.AlwaysAdd(nameof(FeedSyncStatus), FeedSyncStatus)
            .Field.WhenNonDefaultAdd(nameof(LastPublicationTime), LastPublicationTime, DateTime.MinValue, "{0:O}")
            .Field.WhenNonDefaultAdd(nameof(IsFeedSyncStatusUpdated), IsFeedSyncStatusUpdated)
            .Field.WhenNonDefaultAdd(nameof(HasUpdates), HasUpdates)
            .Complete();

    public override string ToString() =>
        $"{GetType().Name}({QuoteToStringMembers}, {PQQuoteContainer.QuoteToStringMembers}, {JustFeedStatusToStringMembers}, {UpdatedFlagsToString})";

    protected void SetFlagsSame(IPublishableTickInstant toCopyFlags)
    {
        if (toCopyFlags is PQPublishableTickInstant pqToClone) UpdatedFlags = pqToClone.UpdatedFlags;
    }
}
